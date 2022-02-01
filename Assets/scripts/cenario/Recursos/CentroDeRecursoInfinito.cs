using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CentroDeRecursoInfinito : MonoBehaviour, CentroDeRecurso, SalvamentoEntreCenas
{
    [Header("Recurso dropado")]
    [SerializeField] private item item;
    [SerializeField] private GameObject recursoColetavelPreFab;
    [Range(-1,1)]
    [SerializeField] private int direcaoLancamentoX;
    [Range(-1, 1)]
    [SerializeField] private int direcaoLancamentoY;
    [Header("Centro de recursos")]
    [SerializeField] private Sprite iconeCentroDeRecursosPadrao;
    [SerializeField] private Sprite iconeCentroDeRecursosGasto;
    [SerializeField] private TMP_Text Timer;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private Material[] materiais = new Material[2]; 
    [SerializeField] private int quantasVezesPodeSerExtraida;
    [SerializeField] private int qntdDoRecursoDropado = 1;
    [SerializeField] private float forca;
    [SerializeField] private int tempoAteProximaColeta;
    private int tempoRestante = 0;
    private int vezesExtraida;
    [Header("Centro de spawn")]
    [SerializeField] private bool centroDeInimigos;
    [SerializeField] private Sprite iconeCentroDeInimigos;
    [SerializeField] private GameObject inimigoPrefab;
    [SerializeField] private pontoDeFugaScript[] pontosDefugaDoInimigoCriado;
    [SerializeField] private int qntdMaximaDeInimigos;
    [SerializeField] private int qntdMaximaDeInimigosEmCena;
    [SerializeField] private float intervaloEntreSpawns;
    [SerializeField] private int VidaMaxDoCentroDeSpawn;
    [SerializeField] private float distanciaXDeSpawnDosInimigos;
    [SerializeField] private float distanciaYDeSpawnDosInimigos;
    private int VidaAtualDoCentroDeSpawn;
    private int qntdInimigosAtuais = 0;
    private int qntdInimigosJaCriados = 0;
    private Coroutine spawnandoInimigos = null;
    private SpriteRenderer SpriteDoObj;
    private int minutos = 0;
    private int segundos = 0;
    private EfeitoFlash flash;
    private bool jogadorProximo = false;
    private void Awake()
    {
        flash = GetComponent<EfeitoFlash>();
        SpriteDoObj = GetComponent<SpriteRenderer>();
        VidaAtualDoCentroDeSpawn = VidaMaxDoCentroDeSpawn;
        
    }
    private void Start()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            if (!GetComponent<SalvarEstadoDoObjeto>().GetObjNaListaDeSalvos(this.gameObject.name))//faz n ter conflito entre esse start e o do salvarEstadoDoObjeto
                DefineEstado();
        }
        else
            DefineEstado();
       //if (GetComponent<SalvarEstadoDoObjeto>() != null && !GetComponent<SalvarEstadoDoObjeto>().GetObjNaListaDeSalvos(this.gameObject.name))
       //    DefineEstado();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && tempoRestante <= 0)
        {
            jogadorProximo = true;
            SpriteDoObj.material = materiais[1];
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && tempoRestante <= 0)
        {
            jogadorProximo = false;
            SpriteDoObj.material = materiais[0];
        }
    }
    public void RecebeuHit()
    {
        if (centroDeInimigos)
        {
            flash.Flash(Color.white);
            AplicarDano();
        }
        else
        {
            CriaRecurso();
        }
        SalvarEstado();
    }
    private void CriaRecurso()
    {
        if (vezesExtraida < quantasVezesPodeSerExtraida)
        {
            GameObject recurso = Instantiate(recursoColetavelPreFab, transform.position, Quaternion.identity);
            recurso.GetComponent<recurso_coletavel>().DefineItem(item);
            recurso.GetComponent<recurso_coletavel>().DefineQuantidadeItem(qntdDoRecursoDropado);
            recurso.GetComponent<recurso_coletavel>().LancaRecurso(forca, direcaoLancamentoX, direcaoLancamentoY);
            vezesExtraida++;
            if (vezesExtraida == quantasVezesPodeSerExtraida)
            {
                tempoRestante = tempoAteProximaColeta;
                SetupCooldown();
                StartCoroutine(this.RecursoCooldown());
            }
            else
                flash.Flash(Color.white);
        }
    }
    private void AplicarDano()
    {
        VidaAtualDoCentroDeSpawn--;
        if (VidaAtualDoCentroDeSpawn <= 0)
        {
            centroDeInimigos = false;
            StopAllCoroutines();
            DefineSprite(iconeCentroDeRecursosPadrao);
        }
    }
    private void SetupCooldown()
    {
        hitbox.enabled = false;
        Timer.gameObject.SetActive(true);
        SpriteDoObj.material = materiais[2];//aplica o material de escurecer
        ConfiguraTimer();
        if (iconeCentroDeRecursosGasto != null)
            DefineSprite(iconeCentroDeRecursosGasto);
    }
    private void ConfiguraTimer()
    {
        minutos = Mathf.FloorToInt(tempoRestante / 60);
        if (minutos < 0)
            minutos = 0;
        segundos = Mathf.FloorToInt(tempoRestante - minutos * 60);
        if (segundos < 0)
            segundos = 0;
        Timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
    IEnumerator RecursoCooldown()
    {
        while(tempoRestante > 0)
        {
            if (segundos == 0 && minutos > 0)
            {
                minutos--;
                segundos = 59;
            }
            else
            {
                segundos--;
            }
            tempoRestante--;
            Timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
            yield return new WaitForSeconds(1f);
        }
        vezesExtraida = 0;
        Timer.gameObject.SetActive(false);
        if (jogadorProximo)
            SpriteDoObj.material = materiais[1];//aplica o material outline
        else
            SpriteDoObj.material = materiais[0];//aplica o material padrão
        DefineSprite(iconeCentroDeRecursosPadrao);
        hitbox.enabled = true;
    }
    IEnumerator SpawnInimigos()
    {
        if (qntdMaximaDeInimigos > 0)
        {
            if (qntdInimigosJaCriados < qntdMaximaDeInimigos)
            {
                while (qntdInimigosAtuais < qntdMaximaDeInimigosEmCena)
                {
                    if (qntdInimigosJaCriados < qntdMaximaDeInimigos)
                    {
                        yield return new WaitForSeconds(intervaloEntreSpawns);
                        GameObject inimigo = Instantiate(inimigoPrefab, transform.position + new Vector3(Random.Range(-distanciaXDeSpawnDosInimigos, distanciaXDeSpawnDosInimigos), Random.Range(-distanciaYDeSpawnDosInimigos, distanciaYDeSpawnDosInimigos), 0f), Quaternion.identity);
                        inimigo.GetComponent<inimigoScript>().SetCentroDeSpawn(this.gameObject.GetComponent<CentroDeRecursoInfinito>());
                        if (inimigo.GetComponent<inimigoScript>().tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa && pontosDefugaDoInimigoCriado.Length > 0)
                        {
                            foreach(pontoDeFugaScript ponto in pontosDefugaDoInimigoCriado)
                            {
                                ponto.inimigosRelacionadosPorSpawn.Add(inimigo.GetComponent<inimigoScript>());
                            }
                        }
                        qntdInimigosAtuais++;
                        qntdInimigosJaCriados++;
                    }
                    else
                        break;
                }
                spawnandoInimigos = null;
            }
        }
        else
        {
            while (qntdInimigosAtuais < qntdMaximaDeInimigosEmCena)
            {
                yield return new WaitForSeconds(intervaloEntreSpawns);
                GameObject inimigo = Instantiate(inimigoPrefab, transform.position + new Vector3(Random.Range(-distanciaXDeSpawnDosInimigos, distanciaXDeSpawnDosInimigos), Random.Range(-distanciaYDeSpawnDosInimigos, distanciaYDeSpawnDosInimigos), 0f), Quaternion.identity);
                inimigo.GetComponent<inimigoScript>().SetCentroDeSpawn(this.gameObject.GetComponent<CentroDeRecursoInfinito>());
                if (inimigo.GetComponent<inimigoScript>().tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa && pontosDefugaDoInimigoCriado.Length > 0)
                {
                    foreach (pontoDeFugaScript ponto in pontosDefugaDoInimigoCriado)
                    {
                        ponto.inimigosRelacionadosPorSpawn.Add(inimigo.GetComponent<inimigoScript>());
                    }
                }
                qntdInimigosAtuais++;
            }
            spawnandoInimigos = null;
        }
        
    }
    public void InimigoDerrotado(inimigoScript inimigoDerrotado)
    {
        qntdInimigosAtuais--;
        if (pontosDefugaDoInimigoCriado.Length > 0 && inimigoPrefab.GetComponent<inimigoScript>().tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
        {
            foreach (pontoDeFugaScript ponto in pontosDefugaDoInimigoCriado)
            {
                ponto.inimigosRelacionadosPorSpawn.Remove(inimigoDerrotado);
            }
        }
        if (centroDeInimigos && spawnandoInimigos == null)
            spawnandoInimigos =  StartCoroutine(this.SpawnInimigos());
    }
    private void DefineEstado()
    {
        if (centroDeInimigos && spawnandoInimigos == null)
        {
            DefineSprite(iconeCentroDeInimigos);
            spawnandoInimigos = StartCoroutine(this.SpawnInimigos());
        }
        else
        {
            DefineSprite(iconeCentroDeRecursosPadrao);
            if (tempoRestante > 0)
            {
                SetupCooldown();
                StartCoroutine(this.RecursoCooldown());
            }
            else if (tempoRestante == 0)
            {
                vezesExtraida = 0;
            }
        }
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().AtivarCarregamentoDoObjeto();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosCentrosDeRecursos(this, 0);
        }
    }
    public void CarregarDados()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosCentrosDeRecursos(this, 1);
        DefineEstado();
    }
    //funções de set e get
    private void DefineSprite(Sprite sprite)
    {
        SpriteDoObj = GetComponent<SpriteRenderer>();
        SpriteDoObj.sprite = sprite;
    }
    public bool GetCentroDeInimigos()
    {
        return centroDeInimigos;
    }
    public int GetTempoRestanteCooldown()
    {
        return tempoRestante;
    }
    public int GetVidaAtual()
    {
        return VidaAtualDoCentroDeSpawn;
    }
    public int GetVezesExtraida()
    {
        return vezesExtraida;
    }
    public void SetCentroDeInimigos(bool ativado_desativado)
    {
        centroDeInimigos = ativado_desativado;
    }
    public void SetTempoRestanteCooldown(int temp)
    {
        if (temp < 0)
            temp = 0;
        tempoRestante = temp;
    }
    public void SetVidaAtual(int vida)
    {
        VidaAtualDoCentroDeSpawn = vida;
    }
    public void SetVezesExtraida(int vezExtraido)
    {
        vezesExtraida = vezExtraido;
    }
}
