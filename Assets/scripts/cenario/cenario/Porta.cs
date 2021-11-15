using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Porta : MonoBehaviour, SalvamentoEntreCenas
{
    [Header("Configurações da Porta")]
    [SerializeField] private bool aberta;
    [SerializeField] private float tempoParaAbrir;
    [SerializeField] private float tempoParaFechar;
    [SerializeField] private GameObject chaveNecessaria;
    [SerializeField] private int nDeBotoesNecessarios;
    //[Range(-1, 1)]
    //[SerializeField] private int direcaoDeRotacao;
    //[SerializeField] private Transform pontoDeRotacao;
    [SerializeField] private UnityEvent EventosAoAbrirPorta;
    [SerializeField] private UnityEvent EventosAoFecharPorta;
    [Header("Configurações Para Ações com Inimigos")]
    [SerializeField] private bool Criar;
    [SerializeField] private GameObject inimigo;
    [SerializeField] private Transform pontoDeSpawnInimigo;
    private int botoesPrecionados = 0;
    private BoxCollider2D colisao;
    private bool iniciouCorrotina = false;
    private Item chave = null;
    private Animator animator;

    private void Start()
    {
        colisao = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        //if (direcaoDeRotacao == 0)
        //    direcaoDeRotacao = 1;
        if (chaveNecessaria != null)
            chave = chaveNecessaria.GetComponent<recurso_coletavel>().ReferenciaItem();
        //if (aberta)
        //    AbrePorta();
        //else
        //    FechaPorta();
    }
    public void PortaPorBotao(int valorBotao)
    {
        botoesPrecionados += valorBotao;
        if (botoesPrecionados >= nDeBotoesNecessarios)
        {
            AbrePorta();
            //StartCoroutine(this.TempoPorta());
        }
    }
    public void PortaPorChave()
    {
        if (!aberta)
        {
            if (UIinventario.Instance.ProcurarChave(chave))
            {
                StartCoroutine(this.TempoPorta());
            }            
        }
        else
        {
            StartCoroutine(this.TempoPorta());
        }
    }
    private IEnumerator TempoPorta()
    {
        if (!iniciouCorrotina)
        {
            iniciouCorrotina = true;
            if (aberta)
            {
                yield return new WaitForSeconds(tempoParaFechar);
                FechaPorta();
                iniciouCorrotina = false;
            }
            else
            {
                yield return new WaitForSeconds(tempoParaAbrir);
                AbrePorta();
                iniciouCorrotina = false;
            }
            SalvarEstado();
        }
    }
    private void AbrePorta()
    {
        colisao.enabled = false;
        aberta = true;
        animator.SetBool("ABERTO", aberta);
        //transform.RotateAround(pontoDeRotacao.position, new Vector3(0f,0f,1f), direcaoDeRotacao * 90f);
        chave = null;
        EventosAoAbrirPorta.Invoke();
    }
    private void FechaPorta()
    {
        colisao.enabled = true;
        aberta = false;
        animator.SetBool("ABERTO", aberta);
        //if (transform.rotation.z != 0f)
        //    transform.RotateAround(pontoDeRotacao.position, new Vector3(0f, 0f, 1f), -direcaoDeRotacao * 90f);
        EventosAoFecharPorta.Invoke();
    }
    public void acaoComInimigos()
    {
        if (Criar)
        {
            GameObject obj = Instantiate(inimigo, pontoDeSpawnInimigo);
        }
        else
        {
            inimigo.GetComponent<inimigoScript>().enabled = true;
        }
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDasPortas(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDasPortas(this, 1);
        colisao = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        if (aberta)
        {
            chaveNecessaria = null;
            chave = null;
            AbrePorta();
        }
        else
            FechaPorta();
        if (EventosAoAbrirPorta == null)
            acaoComInimigos();
    }
    public bool GetAberto_Fechado()
    {
        return aberta;
    }
    public void SetAberto_Fechado(bool b)
    {
        aberta = b;
    }
    public void AtivarRobo()
    {
        if (GetComponent<DialogoUnico>() != null)
        {
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            GetComponent<DialogoUnico>().AtivarDialogo();
            //acaoComInimigos();
        }
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        acaoComInimigos();
        EventosAoAbrirPorta = null;
        DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
    }
    public void SetEventosAbrirPorta(UnityEvent evento)
    {
        EventosAoAbrirPorta = evento;
    }
    public UnityEvent GetEventosAbrirPorta()
    {
        return EventosAoAbrirPorta;
    }
    public void SetEventosFecharPorta(UnityEvent evento)
    {
        EventosAoFecharPorta = evento;
    }
    public UnityEvent GetEventosFecharPorta()
    {
        return EventosAoFecharPorta;
    }
}