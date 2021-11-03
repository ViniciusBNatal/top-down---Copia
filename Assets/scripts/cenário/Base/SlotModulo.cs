using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlotModulo : MonoBehaviour, Clicavel, AcoesNoTutorial, SalvamentoEntreCenas
{
    [SerializeField] private SpriteRenderer iconeDoModulo;
    [SerializeField] private SpriteRenderer iconeDoDesastre;
    [SerializeField] private SpriteRenderer iconeDeMultiplicador;
    [SerializeField] private bool tutorial = true;
    private int resistenciaModulo = 0;
    private string NomeDesastre = "";
    private int tipoModulo = 0;

    private void Start()
    {
        BaseScript.Instance.AdicionarModulo(this);
    }
    public void Click(jogadorScript jogador)
    {
        if (iconeDoModulo.sprite != null)
        {
            ConstruirModulo(jogador.GetModuloConstruido().GetForca(), jogador.GetModuloConstruido().desastre, jogador.GetModuloConstruido().modulo);
            RetornarCameraEMudarEstadoJogador();
        }
        else
        {
            RemoverModulo();
            ConstruirModulo(jogador.GetModuloConstruido().GetForca(), jogador.GetModuloConstruido().desastre, jogador.GetModuloConstruido().modulo);
            RetornarCameraEMudarEstadoJogador();
        }
    }
    public void ConstruirModulo(int forca, string desastre, int modulo)
    {
        if (forca != 0 && modulo != 0 && desastre != "")
        {
            SetValorResistencia(forca);
            SetNomeDesastre(desastre);
            SetModulo(modulo);
            //aplica o sprite do modulo dependendo do modulo no crafting
            if (modulo == 2)
            {
                SetSpriteDoModulo(DesastresList.Instance.SelecionaSpriteModulo(modulo));
                BossAlho.Instance.BossDerotado();
                return;
            }
            SetSpriteDoModulo(DesastresList.Instance.SelecionaSpriteModulo(modulo));
            //cria o icone do desastre dependedo do desastre natural do crafting
            SetSpriteDoDesastre(DesastresList.Instance.SelecionaSpriteDesastre(NomeDesastre));
            //cria o icone de multiplicador dependedo do nivel do crafting
            SetSpriteDoMultiplicador(DesastresList.Instance.SelecionaSpriteMultiplicador(resistenciaModulo));
        }
    }
    public void RemoverModulo()
    {
        SetSpriteDoModulo(null);
        SetSpriteDoDesastre(null);
        SetSpriteDoMultiplicador(null);
        SetValorResistencia(0);
        SetNomeDesastre("");
    }
    public void SetSpriteDoModulo(Sprite Sprite)
    {
        iconeDoModulo.sprite = Sprite;
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
    public string GetNomeDesastre()
    {
        return NomeDesastre.ToUpper();
    }
    public void SetValorResistencia(int valor)
    {
        resistenciaModulo = valor;
    }
    public void SetNomeDesastre(string nome)
    {
        NomeDesastre = nome;
    }
    public void SetModulo(int i)
    {
        tipoModulo = i;
    }
    public int GetModulo()
    {
        return tipoModulo;
    }
    public void Tutorial()
    {
        if (tutorial)
        {
            DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            BaseScript.Instance.DesligarTutorialDosModulos();
            TutorialSetUp.Instance.IniciarDialogo();
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
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosModulos(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosModulos(this, 1);
        ConstruirModulo(resistenciaModulo, NomeDesastre, tipoModulo);
    }
    private void RetornarCameraEMudarEstadoJogador()
    {
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform);
        jogadorScript.Instance.MudarEstadoJogador(0);
        Tutorial();
    }
}
