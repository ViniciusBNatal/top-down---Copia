using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Botao : MonoBehaviour, SalvamentoEntreCenas
{
    private Animator animator;
    [SerializeField] private UnityEvent evento;
    [Header("Compnentes para abrir porta")]
    [SerializeField] private Porta portaRelacionada;
    [SerializeField] private int valorBotao;
    [SerializeField] private bool usoUnico;
    private bool precionado = false;
    [Header("Compnentes para acao com inimigos")]
    [SerializeField] private bool Criar;
    [SerializeField] private GameObject inimigo;
    [SerializeField] private Transform pontoDeSpawnInimigo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("USOUNICO", usoUnico);
    }
    private void Start()
    {
        //animator = GetComponent<Animator>();
        //animator.SetBool("USOUNICO", usoUnico);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EstadoBotao();
            evento.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EstadoBotao();
        }
    }
    public void TentarAbrirPorta()
    {
        if (EstadoBotao())
            portaRelacionada.PortaPorBotao(valorBotao);
    }
    private bool EstadoBotao()
    {
        if (usoUnico)
        {
            if (!precionado)
            {
                precionado = true;
                SalvarEstado();
            }
        }
        else
        {
            precionado = !precionado;
        }
        animator.SetBool("PRECIONADO", precionado);
        return precionado;
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
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosBotoes(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosBotoes(this, 1);
        EstadoBotao();
    }
    public bool GetUsoUnico()
    {
        return usoUnico;
    }
    public void SetUsoUnico(bool b)
    {
        usoUnico = b;
    }
}
