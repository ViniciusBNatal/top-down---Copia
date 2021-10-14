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
    [SerializeField] private int desastre;
    public void Click(GameObject jogador)
    {
        if (iconeDoModulo.enabled == false)
        {
            VisibilidadeSpriteDoModulo(true);
            SetValorResistencia(jogadorScript.Instance.moduloCriado.resistencia);
            SetIndexDesastre(jogadorScript.Instance.moduloCriado.desastre);
            //cria o icone do desastre dependedo do desastre natural do crafting
            SetSpriteDoDesastre(SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteDesastre(desastre));
            //cria o icone de multiplicador dependedo do nivel do crafting
            SetSpriteDoMultiplicador(SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteMultiplicador(resistenciaModulo));
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(1);
            jogadorScript.Instance.MudarEstadoJogador(0);
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
    public int indexDesastre()
    {
        return desastre;
    }
    public void SetValorResistencia(int valor)
    {
        resistenciaModulo = valor;
    }
    public void SetIndexDesastre(int valor)
    {
        desastre = valor;
    }
}
