using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CentroDeRecursoInfinito : MonoBehaviour, CentroDeRecurso, SalvamentoEntreCenas
{
    [Header("Componentes do recurso dropado")]
    [SerializeField] private Item item;
    [SerializeField] private GameObject recursoColetavelPreFab;
    [Range(-1,1)]
    [SerializeField] private int direcaoLancamentoX;
    [Range(-1, 1)]
    [SerializeField] private int direcaoLancamentoY;
    [Header("Valores numéricos de centro de recursos")]
    [SerializeField] private Sprite iconeCentroDeRecursosPadrao;
    [SerializeField] private Sprite iconeCentroDeRecursosGasto;
    [SerializeField] private TMP_Text Timer;
    [SerializeField] private Material[] materiais = new Material[2]; 
    [SerializeField] private int quantasVezesPodeSerExtraida;
    [SerializeField] private int qntdDoRecursoDropado = 1;
    [SerializeField] private float forca;
    [SerializeField] private int tempoAteProximaColeta;
    private int tempoRestante = 0;
    private int vezesExtraida;
    [Header("Componentes de centro de spawn")]
    [SerializeField] private bool centroDeInimigos;
    [SerializeField] private Sprite iconeCentroDeInimigos;
    [SerializeField] private GameObject inimigoPrefab;
    [SerializeField] private int qntdMaximaDeInimigos;
    [SerializeField] private float intervaloEntreSpawns;
    [SerializeField] private int VidaMaxDoCentroDeSpawn;
    private int VidaAtualDoCentroDeSpawn;
    private int qntdInimigosAtuais = 0;
    private bool spawnandoInimigos = false;
    private SpriteRenderer SpriteDoObj;
    private int minutos = 0;
    private int segundos = 0;
    private EfeitoFlash flash;
    private void Awake()
    {
        flash = GetComponent<EfeitoFlash>();
        SpriteDoObj = GetComponent<SpriteRenderer>();
        VidaAtualDoCentroDeSpawn = VidaMaxDoCentroDeSpawn;
    }
    private void Start()
    {
        SalvamentoDosCentrosDeRecursosManager.Instance.AdicionarCentroALista(this.gameObject);
        DefineEstado();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SpriteDoObj.material = materiais[1];
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SpriteDoObj.material = materiais[0];
        }
    }
    public void RecebeuHit()
    {
        flash.Flash(Color.white);
        if (centroDeInimigos)
        {
            AplicarDano();
        }
        else
        {
            CriaRecurso();
        }
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
        Timer.gameObject.SetActive(true);
        //SpriteDoObj.material = materiais[1];//aplica o material de escurecer
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
          //SpriteDoObj.material = materiais[0];//aplica o material padrão
          DefineSprite(iconeCentroDeRecursosPadrao);
    }
    IEnumerator SpawnInimigos()
    {
        if (spawnandoInimigos == false)
        {
            spawnandoInimigos = true;
            while(qntdInimigosAtuais < qntdMaximaDeInimigos)
            {
                GameObject inimigo = Instantiate(inimigoPrefab, transform.position, Quaternion.identity);
                inimigo.GetComponent<inimigoScript>().SetCentroDeSpawn(this.gameObject.GetComponent<CentroDeRecursoInfinito>());
                qntdInimigosAtuais++;
                yield return new WaitForSeconds(intervaloEntreSpawns);
            }
            spawnandoInimigos = false;
        }
    }
    public void InimigoDerrotado()
    {
        qntdInimigosAtuais--;
        if (centroDeInimigos)
            StartCoroutine(this.SpawnInimigos());
    }
    private void DefineEstado()
    {
        if (centroDeInimigos)
        {
            DefineSprite(iconeCentroDeInimigos);
            StartCoroutine(this.SpawnInimigos());
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
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosCentrosDeRecursos(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosCentrosDeRecursos(this, 1);
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
