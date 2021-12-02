using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotModulo : MonoBehaviour, /*Clicavel*/ AcoesNoTutorial, SalvamentoEntreCenas, ClickInter
{
    [SerializeField] private SpriteRenderer iconeDoModulo;
    [SerializeField] private SpriteRenderer iconeDoDesastre;
    [SerializeField] private SpriteRenderer iconeDeMultiplicador;
    [HideInInspector] public List<Material> materiais = new List<Material>();
    [SerializeField] private GameObject animacaoConstrucao;
    [SerializeField] private GameObject animacaoDestruicaoModulo;
    private SpriteRenderer iconeSlotDeModulo;
    private int resistenciaModulo = 0;
    private string NomeDesastre = "";
    private int tipoModulo = 0;
    private void Awake()
    {
        iconeSlotDeModulo = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        BaseScript.Instance.AdicionarModulo(this);
    }
    //public void Click(jogadorScript jogador)
    //{
    //    ConstruirModulo(jogador.GetModuloConstruido().GetForca(), jogador.GetModuloConstruido().desastre, jogador.GetModuloConstruido().modulo);
    //    RetornarCameraEMudarEstadoJogador();
    //}
    public void Acao(jogadorScript jogador)
    {
        ConstruirModulo(jogador.GetModuloConstruido().GetForca(), jogador.GetModuloConstruido().desastre, jogador.GetModuloConstruido().modulo);
        RetornarCameraEMudarEstadoJogador();
    }
    public void ConstruirModulo(int forca, string desastre, int modulo)
    {
        BaseScript.Instance.Ativar_DesativarVisualConstrucaoModulos(false);
        if (modulo == 2)
        {
            SetModulo(modulo);
            SetSpriteDoModulo(DesastresList.Instance.SelecionaSpriteModulo(modulo));
            if (BossAlho.Instance != null)
                BossAlho.Instance.BossDerotado();
            return;
        }
        else
        {
            if (forca != 0 && modulo != 0 && desastre != "")
            {
                SetValorResistencia(forca);
                SetNomeDesastre(desastre);
                SetModulo(modulo);
                //aplica o sprite do modulo dependendo do modulo no crafting
                SetSpriteDoModulo(DesastresList.Instance.SelecionaSpriteModulo(modulo));
                //cria o icone do desastre dependedo do desastre natural do crafting
                SetSpriteDoDesastre(DesastresList.Instance.SelecionaSpriteDesastre(NomeDesastre));
                //cria o icone de multiplicador dependedo do nivel do crafting
                SetSpriteDoMultiplicador(DesastresList.Instance.SelecionaSpriteMultiplicador(resistenciaModulo));
                IndicadorDosDesastres.Instance.AtivarCheckDeModuloConstruido(forca, desastre, modulo);
            }
        }
    }
    public void RemoverModulo()
    {
        animacaoDestruicaoModulo.GetComponent<Animator>().SetTrigger("EXPLODIR");
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
        //DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
        DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
        TutorialSetUp.Instance.IniciarDialogo();
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
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosModulos(this, 1);
            ConstruirModulo(resistenciaModulo, NomeDesastre, tipoModulo);
        }
    }
    private void RetornarCameraEMudarEstadoJogador()
    {
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform, 0f);
        jogadorScript.Instance.MudarEstadoJogador(0);
        if (TutorialSetUp.Instance != null)
            Tutorial();
    }
    public void VisualConstrucao(bool emConstrucao)
    {
        if (emConstrucao && !animacaoConstrucao.activeInHierarchy)
        {
            animacaoConstrucao.SetActive(true);
            if (iconeDoModulo.sprite == null)//visal de costrução
            {
                //iconeSlotDeModulo.material = materiais[1];
                animacaoConstrucao.GetComponent<Animator>().SetInteger("ESTADO", 0);
            }
            else//visual demolição
            {
                //iconeDoModulo.material = materiais[2];
                animacaoConstrucao.GetComponent<Animator>().SetInteger("ESTADO", 1);
            }
        }
        else
        {
            //iconeSlotDeModulo.material = materiais[0];
            //iconeDoModulo.material = materiais[0];
            animacaoConstrucao.SetActive(false);
            if (iconeSlotDeModulo.material != materiais[0])
                iconeSlotDeModulo.material = materiais[0];
        }
    }
}
