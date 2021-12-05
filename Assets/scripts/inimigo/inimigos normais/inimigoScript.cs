using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class inimigoScript : MonoBehaviour
{
    [Header("Caracteristicas do Inimigo")]
    [SerializeField] private bool imortal;
    public TiposDeMovimentacao tiposDeMovimentacao;
    [SerializeField] private bool disparo;
    [SerializeField] private GameObject projetil;
    [SerializeField] private Transform pontoDisparo;
    //[SerializeField] private List<Transform> pontosDeFuga = new List<Transform>();
    private Dictionary<string, Vector3> pontosDefuga = new Dictionary<string, Vector3>();
    private Rigidbody2D rb;
    private bool movimentacaoFixa = false;
    [Header("Valores numéricos")]
    [SerializeField] private float vidaMaxima;
    [SerializeField] private float velocidade;
    [SerializeField] private float reducaoVelocDuranteAtaqueMelee;
    [SerializeField] private float taxaDisparo;
    //[SerializeField] private float raioVisao;
    [SerializeField] private float velocidadeProjetil;
    public float danoMelee;
    [SerializeField] private float danoRanged;
    public float forcaRepulsao;
    public float tempoDeStunNoJogador = 1f;
    [SerializeField] private Vector2 distanciaMinimaParaFugir;
    public Vector2 direcaoDeMovimentacao;
    //variaveis privadas
    [Header("Não Mexer")]
    [SerializeField] private GameObject primPontoDeNavPrefab;
    [SerializeField] private Collider2D areaDetecao;
    [SerializeField] private GameObject animacaoRelogio;
    [SerializeField] private Material[] materiais;
    private EfeitoFlash flash;
    private inimigoAnimScript inimigoAnimScript;
    private Vector2 direcaoProjetil;
    private Transform alvo;
    private float vidaAtual;
    private bool paralisado = false;
    private bool SendoEmpurrado = false;
    private SpriteRenderer spriteInimigo;
    private Coroutine paralisar = null;
    //private bool trocarOrdemPontosDeFuga = false;
    public enum TiposDeMovimentacao
    {
        estatico,
        movimentacaoLivre,
        movimentacaoFixa,
        movimentacaoEntrePontosAleatorios,
        movimentacaoEntrePontosFixa,
    }
    //private int proximoPontoDeFuga = 0;
    private CentroDeRecursoInfinito CentroDeSpawn = null;
    private List<Vector3> pontosDeNavegacaoDeRetorno = new List<Vector3>();
    private Coroutine salvandoPontosDeNavegacao = null;
    private bool PrecisaRetornarAoPontoInicial = false;
    private GameObject pontoInicial;
    private UnityAction AoReceberDano;
    private string pontoDefugaParaTeleportar;
    private bool escondido = false;
    private bool atirando = false;
    private float forcaEmpurrao;
    private Vector2 direcaoEmpurrao;
    private float reducaoVelocidade = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        flash = GetComponent<EfeitoFlash>();
        rb = GetComponent<Rigidbody2D>();
        inimigoAnimScript = GetComponent<inimigoAnimScript>();
        spriteInimigo = GetComponent<SpriteRenderer>();
        vidaAtual = vidaMaxima;
    }
    void Start()
    {
        if (tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoFixa)
            movimentacaoFixa = true;
        else if (tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoLivre)
        {
            pontoInicial  = Instantiate(primPontoDeNavPrefab, transform.position, Quaternion.identity);
            pontoInicial.transform.SetParent(null);
            pontosDeNavegacaoDeRetorno.Add(pontoInicial.transform.position);
        }     
        if (CentroDeSpawn != null && !CentroDeSpawn.GetCentroDeInimigos())
        {
            CentroDeSpawn.InimigoDerrotado();
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        //orientacaoSprite();
        if (!paralisado)
        {
            if (SendoEmpurrado && tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoLivre)
            {
                rb.velocity = -forcaEmpurrao * direcaoEmpurrao;
            }
            else
            {
                switch (tiposDeMovimentacao)
                {
                    case TiposDeMovimentacao.movimentacaoFixa:
                        rb.velocity = direcaoDeMovimentacao * (velocidade - reducaoVelocidade);
                        break;
                    case TiposDeMovimentacao.movimentacaoLivre:
                        if (alvo != null)
                        {
                            direcaoDeMovimentacao = (alvo.position - transform.position).normalized;
                            rb.velocity = direcaoDeMovimentacao * (velocidade - reducaoVelocidade);
                            if (salvandoPontosDeNavegacao == null)
                                salvandoPontosDeNavegacao = StartCoroutine(this.salvaPontosParaNavegacao());
                        }
                        else if (PrecisaRetornarAoPontoInicial)
                        {
                            direcaoDeMovimentacao = (ProximoPonto() - transform.position).normalized;
                            rb.velocity = direcaoDeMovimentacao * velocidade;
                            VerificarSePontoFoiAlcancado();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && distanciaMinimaParaFugir != Vector2.zero && this.isActiveAndEnabled && alvo != null)
        {
            Vector2 distanciaDoAlvo = alvo.position - transform.position;
            if (Mathf.Abs(distanciaDoAlvo.x) <= distanciaMinimaParaFugir.x && Mathf.Abs(distanciaDoAlvo.y) <= distanciaMinimaParaFugir.y)
            {
                if (!escondido)
                {
                    escondido = true;
                    pontoDefugaParaTeleportar = null;
                    inimigoAnimScript.Surgir(false);
                    inimigoAnimScript.Esconder();
                }
            }
            else
            {
                inimigoAnimScript.Surgir(true);
                escondido = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//jogador entrou da área de detecção
    {
        if(collision.gameObject.tag == "Player" && this.isActiveAndEnabled)
        {
            alvo = collision.transform;
            //retornando = false;
            if (disparo)
            {
                inimigoAnimScript.AtaqueRanged(taxaDisparo);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//jogador saiu da área de detecção
    {
        if (collision.gameObject.tag == "Player" && this.isActiveAndEnabled)
        {
            alvo = null;
            atirando = false;
            rb.velocity = Vector2.zero;
            salvandoPontosDeNavegacao = null;
            PrecisaRetornarAoPontoInicial = true;
            inimigoAnimScript.PararDisparos();
        }
    }
    public void Teleportar(TiposDeMovimentacao tipoDeMovimentacao)// 1 movimentação fixa, 2 movimentação aleatória
    {
        if (pontoDefugaParaTeleportar != null)
        {
            switch (tipoDeMovimentacao)
            {
                case TiposDeMovimentacao.movimentacaoEntrePontosFixa:
                    transform.position = pontosDefuga[pontoDefugaParaTeleportar];
                    //switch (trocarOrdemPontosDeFuga)
                    //{
                    //    case true:
                    //        proximoPontoDeFuga--;
                    //        transform.position = pontosDeFuga[proximoPontoDeFuga].position;
                    //        break;
                    //    case false:
                    //        proximoPontoDeFuga++;
                    //        transform.position = pontosDeFuga[proximoPontoDeFuga].position;
                    //        break;
                    //}
                    //if (proximoPontoDeFuga == pontosDeFuga.Count - 1)
                    //    trocarOrdemPontosDeFuga = true;
                    //else if (proximoPontoDeFuga == 0)
                    //    trocarOrdemPontosDeFuga = false;
                    break;
                case TiposDeMovimentacao.movimentacaoEntrePontosAleatorios:
                    //int r = Random.Range(0, pontosDeFuga.Count);
                    //transform.position = pontosDeFuga[r].position;
                    break;
            }
        }
    }
    public void AtivaReducaoVelocidade()
    {
        reducaoVelocidade = reducaoVelocDuranteAtaqueMelee;
    }
    public void DesativaReducaoVelocidade()
    {
        reducaoVelocidade = 0f;
    }
    public IEnumerator Atirar()
    {
        if (!atirando && alvo != null)
        {
            atirando = true;
            direcaoProjetil = (alvo.position - pontoDisparo.position).normalized;
            inimigoAnimScript.SetDirecaoProjetil(direcaoProjetil);
            GameObject balains = Instantiate(projetil, pontoDisparo.position, Quaternion.identity);
            balains.transform.Rotate(new Vector3(0f, 0f, Mathf.Atan2(direcaoProjetil.y, direcaoProjetil.x) * Mathf.Rad2Deg));
            balains.GetComponent<Rigidbody2D>().velocity = velocidadeProjetil * direcaoProjetil;
            balains.GetComponent<balaHit>().SetDano(danoRanged);
            balains.GetComponent<balaHit>().SetDuracaoStun(tempoDeStunNoJogador);
            yield return new WaitForSeconds(Time.deltaTime);
            atirando = false;
        }
    }
    private void orientacaoSprite()// roda o sprite de acordo com a posição do jogador
    {
        if (direcaoDeMovimentacao.x < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direcaoDeMovimentacao.x > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    public void Fuga(GameObject gobj)
    {
        areaDetecao.enabled = false;
        pontoDefugaParaTeleportar = gobj.name;
        inimigoAnimScript.Surgir(true);
        inimigoAnimScript.Esconder();
    }
    public void LigarDetecao()
    {
        inimigoAnimScript.Surgir(false);
        areaDetecao.enabled = true;
    }
    public void mudancaVida(float dano, string origemHit, float forca, Vector3 origemPosHit, float duracaoStun)
    {
        if (!imortal)
        {
            if (origemHit == "projetil")
            {
                if (paralisar == null)
                {
                    paralisar = StartCoroutine(this.Paralisar(dano));
                    if (TutorialSetUp.Instance != null)
                    {
                        TutorialSetUp.Instance.tirosAcertadosNoInimigo++;
                        if (TutorialSetUp.Instance.GetSequenciaDialogos() == 1 && TutorialSetUp.Instance.tirosAcertadosNoInimigo >= TutorialSetUp.Instance.hitsDeDisparosNoInimigo)
                        {
                            jogadorScript.Instance.EncerrarDisparos();
                            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                            TutorialSetUp.Instance.AoAcertarDisparoNoInimigo();
                        }
                    }
                }
            }
            else
            {
                if (paralisar != null)
                {
                    StopCoroutine(this.Paralisar(0f));
                    FinalizacaoParalisar();
                }
                if (flash != null)
                    flash.Flash(Color.red);
                vidaAtual += dano;
                //SoundManager.Instance.TocarSom(SoundManager.Som.InimigoLevouHit);
                if (vidaAtual > vidaMaxima)
                {
                    vidaAtual = vidaMaxima;
                }
                else if (vidaAtual <= 0)
                {
                    if (CentroDeSpawn != null)
                    {
                        CentroDeSpawn.InimigoDerrotado();
                    }
                    vidaAtual = 0f;
                    if (TutorialSetUp.Instance != null)
                    {
                        if (TutorialSetUp.Instance.GetSequenciaDialogos() == 2)
                        {
                            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                            TutorialSetUp.Instance.AoEliminarOInimigoControlado();
                        }
                        else if (TutorialSetUp.Instance.GetSequenciaDialogos() == 3)
                        {
                            TutorialSetUp.Instance.IniciarDialogo();
                        }
                    }
                    Destroy(this.gameObject);
                }
                Empurrar(forca, origemPosHit, duracaoStun);
            }
        }
    }
    private void Empurrar(float forca, Vector3 direcao, float duracao)
    {
        forcaEmpurrao = forca;
        direcaoEmpurrao = (direcao - this.transform.position).normalized;
        SendoEmpurrado = true;
        StartCoroutine(this.duracaoEmpurrao(duracao));
    }
    IEnumerator duracaoEmpurrao(float temp)
    {
        yield return new WaitForSeconds(temp);
        SendoEmpurrado = false;
        rb.velocity = Vector2.zero;
    }
    IEnumerator Paralisar(float temp)
    {
        paralisado = true;
        rb.velocity = Vector2.zero;
        if (TutorialSetUp.Instance == null)
            inimigoAnimScript.GetAnimator().enabled = false;
        VisualParalisado(true);
        yield return new WaitForSeconds(temp);
        FinalizacaoParalisar();
    }
    private void FinalizacaoParalisar()
    {
        VisualParalisado(false);
        inimigoAnimScript.GetAnimator().enabled = true;
        paralisado = false;
        paralisar = null;
    }
    private void VisualParalisado(bool ligar_desligar)
    {
        if (ligar_desligar)
        {
            animacaoRelogio.SetActive(true);
            spriteInimigo.material = materiais[1];
        }
        else
        {
            animacaoRelogio.SetActive(false);
            spriteInimigo.material = materiais[0];
        }
    }
    public bool GetMovimentacaoFixa()
    {
        return movimentacaoFixa;
    }
    public inimigoAnimScript GetAnimScript()
    {
        return inimigoAnimScript;
    }
    public void SetCentroDeSpawn(CentroDeRecursoInfinito cDeSpawn)
    {
        CentroDeSpawn = cDeSpawn;
    }
    IEnumerator salvaPontosParaNavegacao()
    {
        while (alvo != null)
        {
            yield return new WaitForSeconds(1f);
            pontosDeNavegacaoDeRetorno.Add(transform.position);
        }
    }
    public void AdicionarPontoDeFuga(GameObject gobj)
    {
        Vector3 vec = new Vector3(gobj.GetComponent<pontoDeFugaScript>().pontoDeTeleporte.position.x, gobj.GetComponent<pontoDeFugaScript>().pontoDeTeleporte.position.y, 0f);
        pontosDefuga.Add(gobj.name, vec);
    }
    private Vector3 ProximoPonto()
    {
        //if (pontosDeNavegacaoDeRetorno.Count > 0)
        //{
            return pontosDeNavegacaoDeRetorno[pontosDeNavegacaoDeRetorno.Count - 1];
        //}
        //else
            //return pontosDeNavegacaoDeRetorno[0];
            //return transform.position;
    }
    private void VerificarSePontoFoiAlcancado()
    {
        int paredePos = 0;
        int pontoInicialPos = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, pontoInicial.transform.position - transform.position);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.tag == "obstaculo")
                paredePos = i;
            if (hits[i].collider.gameObject.tag == "pontoDeNavInicial")
                pontoInicialPos = i;
        }
        if (pontoInicialPos < paredePos)
        {
            for (int i = pontosDeNavegacaoDeRetorno.Count - 1; i >= 1; i--)
                pontosDeNavegacaoDeRetorno.RemoveAt(i);
        }
        if (Mathf.Abs(ProximoPonto().x - transform.position.x) <= .5f && Mathf.Abs(ProximoPonto().y - transform.position.y) <= .5f)
        {
            if (pontosDeNavegacaoDeRetorno.Count == 1)
            {
                PrecisaRetornarAoPontoInicial = false;
                //pontosDeNavegacaoDeRetorno.Clear();
                rb.velocity = Vector2.zero;
            }
            else
            {
                pontosDeNavegacaoDeRetorno.RemoveAt(pontosDeNavegacaoDeRetorno.Count - 1);
            }
        }
    }
    public bool GetParalisado()
    {
        return paralisado;
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        switch (TutorialSetUp.Instance.GetSequenciaDialogos())
        {
            case 2:
                jogadorScript.Instance.GetAnimacoesTutorial().GetComponent<Animator>().SetBool("MELEE", true);
                JogadorAnimScript.Instance.Getanimator().SetFloat("HORIZONTAL", 1f);
                JogadorAnimScript.Instance.Getanimator().SetFloat("VERTICAL", 0f);
                jogadorScript.Instance.MudarEstadoJogador(1);
                break;
            case 3:
                jogadorScript.Instance.GetAnimacoesTutorial().GetComponent<Animator>().SetBool("MOV", true);
                break;
        }
    }
}