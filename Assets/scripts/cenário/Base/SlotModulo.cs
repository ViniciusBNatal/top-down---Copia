using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class SlotModulo : MonoBehaviour, Clicavel, AcoesNoTutorial
{
    [SerializeField] private SpriteRenderer iconeDoModulo;
    [SerializeField] private SpriteRenderer iconeDoDesastre;
    [SerializeField] private SpriteRenderer iconeDeMultiplicador;
    [SerializeField] private int resistenciaModulo;
    [SerializeField] private string desastre;
    [SerializeField] private bool tutorial = true;

    private void Start()
    {
        BaseScript.Instance.AdicionarModulo(this);
    }
    public void Click(GameObject jogador)
    {
        if (iconeDoModulo.enabled == false)
        {
            ConstruirModulo();
        }
        else
        {
            RemoverModulo();
            ConstruirModulo();
        }
    }
    private void ConstruirModulo()
    {
        VisibilidadeSpriteDoModulo(true);
        SetValorResistencia(jogadorScript.Instance.GetModuloConstruido().GetForca());
        SetNomeDesastre(jogadorScript.Instance.GetModuloConstruido().desastre);
        //cria o icone do desastre dependedo do desastre natural do crafting
        SetSpriteDoDesastre(DesastresList.Instance.SelecionaSpriteDesastre(desastre));
        //cria o icone de multiplicador dependedo do nivel do crafting
        SetSpriteDoMultiplicador(DesastresList.Instance.SelecionaSpriteMultiplicador(resistenciaModulo));
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform);
        jogadorScript.Instance.MudarEstadoJogador(0);
        Tutorial();
    }
    public void RemoverModulo()
    {
        VisibilidadeSpriteDoModulo(false);
        SetSpriteDoDesastre(null);
        SetSpriteDoMultiplicador(null);
        SetValorResistencia(0);
        SetNomeDesastre("");
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
    public int GetvalorResistencia()
    {
        return resistenciaModulo;
    }
    public string GetnomeDesastre()
    {
        return desastre.ToUpper();
    }
    public void SetValorResistencia(int valor)
    {
        resistenciaModulo = valor;
    }
    public void SetNomeDesastre(string nome)
    {
        desastre = nome;
    }
    public void Tutorial()
    {
        if (tutorial)
        {
            DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            TutorialSetUp.Instance.IniciarDialogo();
            tutorial = false;
        }
    }
    public void SetTutorial(bool b)//para desligar a funçao de tutorial de todos os modulos atravéz da basescript
    {
        tutorial = b;
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        TutorialSetUp.Instance.AoTerminoDoDialogoInstaladoOModuloDeDefesa();
    }
    public void CancelarInscricaoEmDialogoFinalizado()
    {
        DialogeManager.Instance.DialogoFinalizado -= AoFinalizarDialogo;
    }
}
