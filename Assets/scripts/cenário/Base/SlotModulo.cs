using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class SlotModulo : MonoBehaviour, Clicavel
{
    [SerializeField] private SpriteRenderer iconeDoModulo;
    [SerializeField] private SpriteRenderer iconeDoDesastre;
    [SerializeField] private SpriteRenderer iconeDeMultiplicador;
    [SerializeField] private int resistenciaModulo;
    [SerializeField] private string desastre;
    [SerializeField] private bool tutorial = true;
    public void Click(GameObject jogador)
    {
        if (iconeDoModulo.enabled == false)
        {
            VisibilidadeSpriteDoModulo(true);
            SetValorResistencia(jogadorScript.Instance.moduloCriado.GetForca());
            SetNomeDesastre(jogadorScript.Instance.moduloCriado.desastre);
            //cria o icone do desastre dependedo do desastre natural do crafting
            SetSpriteDoDesastre(DesastresList.Instance.SelecionaSpriteDesastre(desastre));
            //cria o icone de multiplicador dependedo do nivel do crafting
            SetSpriteDoMultiplicador(DesastresList.Instance.SelecionaSpriteMultiplicador(resistenciaModulo));
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(1);
            jogadorScript.Instance.MudarEstadoJogador(0);
            if (tutorial)
            {
                desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloDuranteTutorial, 0f);
                desastreManager.Instance.qntdDeDesastresParaOcorrer = 1;
                desastreManager.Instance.desastresSorteados[0] = "terremoto";
                desastreManager.Instance.forcasSorteados[0] = 1;
                StartCoroutine(desastreManager.Instance.LogicaDesastres(false));
                tutorial = false;
            }
        }
    }
    public void VisibilidadeSpriteDoModulo(bool Ligar_desligar)
    {
        iconeDoModulo.enabled = Ligar_desligar;
    }
    public void SetSpriteDoDesastre(Sprite Sprite)
    {
        iconeDoDesastre.sprite = Sprite;
    }
    public void SetSpriteDoMultiplicador(Sprite Sprite)
    {
        iconeDeMultiplicador.sprite = Sprite;
    }
    public int valorResistencia()
    {
        return resistenciaModulo;
    }
    public string nomeDesastre()
    {
        return desastre;
    }
    public void SetValorResistencia(int valor)
    {
        resistenciaModulo = valor;
    }
    public void SetNomeDesastre(string nome)
    {
        desastre = nome;
    }
}
