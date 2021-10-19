using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public static BaseScript Instance { get; private set; }
    [SerializeField] private List<SlotModulo> modulos = new List<SlotModulo>();
    [SerializeField] private int vidaMax;
    [SerializeField] private Transform posicaoDeChegada;
    public bool duranteMelhoria = false;
    public bool tutorial;
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
        for (int i = 0; i < modulos.Count; i++)
        {
            if (modulos[i] == null)
                return;
            for (int a = 0; a < desastreManager.Instance.qntdDeDesastresParaOcorrer; a++)
            {
                if (desastreManager.Instance.desastresSorteados[a] == modulos[i].nomeDesastre() && desastreManager.Instance.forcasSorteados[a] == modulos[i].valorResistencia())
                {
                    modulos[i].VisibilidadeSpriteDoModulo(false);
                    modulos[i].SetSpriteDoDesastre(null);
                    modulos[i].SetSpriteDoMultiplicador(null);
                    modulos[i].SetValorResistencia(0);
                    modulos[i].SetNomeDesastre(null);
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
            if (!jogadorScript.Instance.inventario.InventarioAberto)
            {
                jogadorScript.Instance.inventario.abreMenuDeTempos();
            }
            else
            {
                jogadorScript.Instance.inventario.fechaMenuDeTempos();
            }
        }
    } 
    public Transform GetPosicao()
    {
        return posicaoDeChegada;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (desastreManager.Instance.desastreAcontecendo)
            {
                if (tutorial)
                {
                    Tutorial();
                    return;
                }
                EncerrarDesastresEVerificarDefesa();
                RecomecarDesastres();
            }
        }
    }
    private void Tutorial()
    {
        UIinventario.Instance.GetBtnEspecifico(0).gameObject.SetActive(true);
        EncerrarDesastresEVerificarDefesa();
        RecomecarDesastres();
        tutorial = false;
    }
    private void EncerrarDesastresEVerificarDefesa()
    {
        desastreManager.Instance.desastreAcontecendo = false;
        desastreManager.Instance.encerramentoDesastres();
        VerificarModulos();
        desastreManager.Instance.LimpaArraysDeSorteio();
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
}
