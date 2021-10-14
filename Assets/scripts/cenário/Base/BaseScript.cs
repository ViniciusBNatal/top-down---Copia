using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    public static BaseScript Instance { get; private set; }
    [SerializeField] private List<SlotModulo> modulos = new List<SlotModulo>();
    [SerializeField] private desastreManager desastresManager;
    [SerializeField] private int vidaMax;
    public bool duranteMelhoria = false;
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
            for (int a = 0; a < desastresManager.qntdDeDesastresOcorridos; a++)
            {
                if (desastresManager.desastresSorteados[a] == modulos[i].indexDesastre() && desastresManager.forcasSorteados[a] == modulos[i].valorResistencia())
                {
                    modulos[i].VisibilidadeSpriteDoModulo(false);
                    modulos[i].SetSpriteDoDesastre(null);
                    modulos[i].SetSpriteDoMultiplicador(null);
                    modulos[i].SetValorResistencia(0);
                    modulos[i].SetIndexDesastre(0);
                    defendido++;
                }
            }
        }
        if (desastresManager.qntdDeDesastresOcorridos == defendido)
        {
            Debug.Log("defendido");
        }
        else
        {
            for (int i = 0; i < desastresManager.qntdDeDesastresOcorridos - defendido; i++)
            {
                vidaAtual--;
                if (vidaAtual <= 0)
                {
                    Debug.Log("Perdeu");
                }
            }
            Debug.Log(vidaAtual);
        }
        for (int i = 0; i < desastresManager.desastresSorteados.Length; i++)
        {
            desastresManager.forcasSorteados[i] = 0;
            desastresManager.desastresSorteados[i] = 0;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (desastresManager.desastreAcontecendo)
            {
                desastresManager.desastreAcontecendo = false;
                desastresManager.encerramentoDesastres();
                VerificarModulos();
                if (duranteMelhoria)
                {
                    DefesasFeitas++;
                    if (DefesasFeitas == desastreManager.Instance.QntdDeDefesasNecessarias)
                    {
                        duranteMelhoria = false;
                        DefesasFeitas = 0;
                        desastresManager.ConfigurarTimer(desastresManager.intervaloEntreOsDesastres, desastresManager.tempoAcumulado);
                    }
                    else
                        desastresManager.ConfigurarTimer(desastresManager.intervaloDuranteADefesa, desastresManager.tempoAcumulado);
                }
                else
                    desastresManager.ConfigurarTimer(desastresManager.intervaloEntreOsDesastres, desastresManager.tempoAcumulado);
                desastresManager.tempoAcumulado = 0f;
                StartCoroutine(desastresManager.LogicaDesastres());
            }
        }
    }
}
