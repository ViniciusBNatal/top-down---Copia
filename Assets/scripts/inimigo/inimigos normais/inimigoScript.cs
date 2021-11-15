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
    [SerializeField] private List<Transform> pontosDeFuga = new List<Transform>();
    private Rigidbody2D rb;
    private bool movimentacaoFixa = false;
    [Header("Valores numéricos")]
    [SerializeField] private float vidaMaxima;
    [SerializeField] private float velocidade;
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
    private CircleCollider2D areaDetecao;
    private EfeitoFlash flash;
    private inimigoAnimScript inimigoAnimScript;
    private Vector2 direcaoProjetil;
    private Transform alvo;
    private float vidaAtual;
    private bool atirando = false;
    private bool trocarOrdemPontosDeFuga = false;
    public enum TiposDeMovimentacao
    {
        estatico,
        movimentacaoLivre,
        movimentacaoFixa,
        movimentacaoEntrePontosAleatorios,
        movimentacaoEntrePontosFixa,
    }
    private int proximoPontoDeFuga = 0;
    private CentroDeRecursoInfinito CentroDeSpawn = null;
    private List<Vector3> pontosDeNavegacaoDeRetorno = new List<Vector3>();
    private Coroutine salvandoPontosDeNavegacao = null;
    private bool PrecisaRetornarAoPontoInicial = false;
    private GameObject pontoInicial;
    private UnityAction AoReceberDano;
    // Start is called before the first frame update
    void Start()
    {
        areaDetecao = GetComponent<CircleCollider2D>();
        flash = GetComponent<EfeitoFlash>();
        rb = GetComponent<Rigidbody2D>();
        inimigoAnimScript = GetComponent<inimigoAnimScript>();
        vidaAtual = vidaMaxima;
        if (tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoFixa)
            movimentacaoFixa = true;
        else if (tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoLivre)
        {
            pontoInicial  = Instantiate(primPontoDeNavPrefab, transform.position, Quaternion.identity);
            pontoInicial.transform.SetParent(null);
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
        switch (tiposDeMovimentacao)
        {
            //case TiposDeMovimentacao.estatico:
            //    break;
            case TiposDeMovimentacao.movimentacaoFixa:
                rb.velocity = (direcaoDeMovimentacao * velocidade);
                break;
            case TiposDeMovimentacao.movimentacaoLivre:
                if (alvo != null)
                {
                    direcaoDeMovimentacao = (alvo.position - transform.position).normalized;
                    rb.velocity = (direcaoDeMovimentacao * velocidade);
                    if (salvandoPontosDeNavegacao == null)
                        salvandoPontosDeNavegacao = StartCoroutine(this.salvaPontosParaNavegacao());
                }
                else if (PrecisaRetornarAoPontoInicial)
                {
                    direcaoDeMovimentacao = (ProximoPonto() - transform.position).normalized;
                    rb.velocity = (direcaoDeMovimentacao * velocidade);
                    VerificarSePontoFoiAlcancado();
                }
                break;
            //case TiposDeMovimentacao.movimentacaoEntrePontosFixa:
            //    break;
            default:
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && distanciaMinimaParaFugir != Vector2.zero && this.isActiveAndEnabled)
        {
            Vector2 distanciaDoAlvo = alvo.position - transform.position;
            if (Mathf.Abs(distanciaDoAlvo.x) <= distanciaMinimaParaFugir.x && Mathf.Abs(distanciaDoAlvo.y) <= distanciaMinimaParaFugir.y)
            {
                areaDetecao.enabled = false;
                inimigoAnimScript.Fuga();
                //Teleportar(tiposDeMovimentacao);
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
                if (!atirando)
                    StartCoroutine(this.Disparar());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)//jogador saiu da área de detecção
    {
        if (collision.gameObject.tag == "Player" && this.isActiveAndEnabled)
        {
            alvo = null;
            rb.velocity = Vector2.zero;
            salvandoPontosDeNavegacao = null;
            PrecisaRetornarAoPontoInicial = true;
        }
    }
    public void Teleportar(TiposDeMovimentacao tipoDeMovimentacao)// 1 movimentação fixa, 2 movimentação aleatória
    {
        switch (tipoDeMovimentacao)
        {
            case TiposDeMovimentacao.movimentacaoEntrePontosFixa:
                switch (trocarOrdemPontosDeFuga)
                {
                    case true:
                        proximoPontoDeFuga--;
                        transform.position = pontosDeFuga[proximoPontoDeFuga].position;
                        break;
                    case false:
                        proximoPontoDeFuga++;
                        transform.position = pontosDeFuga[proximoPontoDeFuga].position;
                        break;
                }
                if (proximoPontoDeFuga == pontosDeFuga.Count - 1)
                    trocarOrdemPontosDeFuga = true;
                else if (proximoPontoDeFuga == 0)
                    trocarOrdemPontosDeFuga = false;
                break;
            case TiposDeMovimentacao.movimentacaoEntrePontosAleatorios:
                int r = Random.Range(0, pontosDeFuga.Count);
                transform.position = pontosDeFuga[r].position;
                break;
        }
    }
    IEnumerator Disparar()
    {
        while(alvo != null)
        {
            atirando = true;
            direcaoProjetil = (alvo.position - pontoDisparo.position).normalized;
            inimigoAnimScript.AtaqueRanged(direcaoProjetil);
            GameObject balains = Instantiate(projetil, pontoDisparo.position, Quaternion.identity);
            balains.transform.Rotate(new Vector3(0f, 0f, Mathf.Atan2(direcaoProjetil.y, direcaoProjetil.x) * Mathf.Rad2Deg));
            balains.GetComponent<Rigidbody2D>().velocity = velocidadeProjetil * direcaoProjetil;
            balains.GetComponent<balaHit>().SetDano(danoRanged);
            balains.GetComponent<balaHit>().SetDuracaoStun(tempoDeStunNoJogador);
            yield return new WaitForSeconds(taxaDisparo);
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
    public void mudancaVida(float valor)
    {
        if (!imortal)
        {
            if (flash != null)
                flash.Flash(Color.red);
            vidaAtual += valor;
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
                Destroy(this.gameObject);
            }
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
            pontosDeNavegacaoDeRetorno.Add(transform.position);
            yield return new WaitForSeconds(1f);
        }
    }
    private Vector3 ProximoPonto()
    {
        if (pontosDeNavegacaoDeRetorno.Count > 0)
        {
            return pontosDeNavegacaoDeRetorno[pontosDeNavegacaoDeRetorno.Count - 1];
        }
        else
            return transform.position;
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
        if (Mathf.Abs(ProximoPonto().x) - Mathf.Abs(transform.position.x) <= .5f && Mathf.Abs(ProximoPonto().y) - Mathf.Abs(transform.position.y) <= .5f)
        {
            if (pontosDeNavegacaoDeRetorno.Count == 0)
            {
                PrecisaRetornarAoPontoInicial = false;
                pontosDeNavegacaoDeRetorno.Clear();
                rb.velocity = Vector2.zero;
            }
            else
            {
                pontosDeNavegacaoDeRetorno.RemoveAt(pontosDeNavegacaoDeRetorno.Count - 1);
            }
        }
    }
    public void LigaDetecao()
    {
        areaDetecao.enabled = true;
    }
}