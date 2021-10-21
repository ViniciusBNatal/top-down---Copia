using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour, AcoesNoTutorial
{
    public static BaseScript Instance { get; private set; }
    [SerializeField] private GameObject areaDeInteracao;
    [SerializeField] private int vidaMax;
    [SerializeField] private Transform posicaoDeChegada;
    public bool duranteMelhoria = false;
    public bool tutorial;
    private List<SlotModulo> listaModulos = new List<SlotModulo>();
    private int vidaAtual;
    private int DefesasFeitas = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        vidaAtual = vidaMax;
    }
    public void VerificarModulos()
    {
        int defendido = 0;
        for (int a = 0; a < desastreManager.Instance.qntdDeDesastresParaOcorrer; a++)
        {
            for (int i = 0; i < listaModulos.Count; i++)
            {
                if (desastreManager.Instance.desastresSorteados[a].ToUpper() == listaModulos[i].GetnomeDesastre() && desastreManager.Instance.forcasSorteados[a] == listaModulos[i].GetvalorResistencia())
                {
                    listaModulos[i].VisibilidadeSpriteDoModulo(false);
                    listaModulos[i].SetSpriteDoDesastre(null);
                    listaModulos[i].SetSpriteDoMultiplicador(null);
                    listaModulos[i].SetValorResistencia(0);
                    listaModulos[i].SetNomeDesastre(null);
                    defendido++;
                }
            }
        }
        if (desastreManager.Instance.qntdDeDesastresParaOcorrer == defendido)
        {
            Debug.Log("defendido");
        }
        else
        {
            //if (!tutorial)
            //{
                for (int i = 0; i < desastreManager.Instance.qntdDeDesastresParaOcorrer - defendido; i++)
                {
                    vidaAtual--;
                    if (vidaAtual <= 0)
                    {
                        Debug.Log("Perdeu");
                    }
                }
                Debug.Log(vidaAtual);
            //}
        }
    }
    public void AbreEFechaMenuDeTrocaDeTempo()
    {
        if (duranteMelhoria)
        {
            Debug.Log("espere a defesa acabar para interagir");
        }
        else
        {
            if (!jogadorScript.Instance.InterfaceJogador.InventarioAberto)
            {
                jogadorScript.Instance.InterfaceJogador.abreMenuDeTempos();
            }
            else
            {
                jogadorScript.Instance.InterfaceJogador.fechaMenuDeTempos();
            }
        }
    } 
    public Transform GetPosicaoParaTeleporte()
    {
        return posicaoDeChegada;
    }
    public void AdicionarModulo(SlotModulo modulo)
    {
        listaModulos.Add(modulo);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (desastreManager.Instance.desastreAcontecendo)
            {
                EncerrarDesastresEVerificarDefesa();
                if (tutorial)
                {
                    Tutorial();
                    return;
                }
                RecomecarDesastres();
            }
        }
    }
    public void Tutorial()
    {
        if (tutorial)
        {
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            TutorialSetUp.Instance.IniciarDialogo();
            tutorial = false;
            return;
        }
        areaDeInteracao.SetActive(true);
        //RecomecarDesastres();
    }
    private void EncerrarDesastresEVerificarDefesa()
    {
        desastreManager.Instance.desastreAcontecendo = false;
        VerificarModulos();
        desastreManager.Instance.LimpaArraysDeSorteio();
        desastreManager.Instance.encerramentoDesastres();
    }
    private void RecomecarDesastres()
    {
        if (duranteMelhoria)
        {
            DefesasFeitas++;
            if (DefesasFeitas == desastreManager.Instance.QntdDeDefesasNecessarias)
            {
                duranteMelhoria = false;
                DefesasFeitas = 0;
                desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloEntreOsDesastres, desastreManager.Instance.tempoAcumulado);
                DesastresList.Instance.LiberarNovosDesastres(UIinventario.Instance.TempoAtual + 2);//ativa a possibilidade do evento desse tempo acontecer, +2 por já começar com 2 desastres
            }
            else
                desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloDuranteADefesa, desastreManager.Instance.tempoAcumulado);
        }
        else
        {
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloEntreOsDesastres, desastreManager.Instance.tempoAcumulado);
        }
        desastreManager.Instance.tempoAcumulado = 0f;
        StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        Debug.Log("devo aparecer 1 vez");
        TutorialSetUp.Instance.AoTerminoDoDialogoTerminadoOPrimeiroDesastre();
    }
    public void CancelarInscricaoEmDialogoFinalizado()
    {
        DialogeManager.Instance.DialogoFinalizado -= AoFinalizarDialogo;
    }
    public void DesligarTutorialDosModulos()
    {
        for (int i = 0; i < listaModulos.Count; i++)
        {
            listaModulos[i].SetTutorial(false);
        }
    }
}
