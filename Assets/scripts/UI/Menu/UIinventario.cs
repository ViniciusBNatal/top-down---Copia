using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIinventario : MonoBehaviour, AcoesNoTutorial, TocarSom
{
    public static UIinventario Instance { get; private set; }
    private bool inventarioAberto = false;
    [SerializeField] private List<UpgradeSlot> listaSlotUpgradesBase = new List<UpgradeSlot>();
    public float zoomOutAoConstruir;
    private Dictionary<string, ItemSlot> itens = new Dictionary<string, ItemSlot>();
    [Header("Nao Mexer")]
    public Missao[] missoesDeLiberarNovoTempo = new Missao[3];
    [SerializeField] private GameObject CaixaDeDialogos;
    [SerializeField] private GameObject inventarioParent;
    [SerializeField] private GameObject abaInventario;
    [SerializeField] private GameObject abaCriação;
    [SerializeField] private GameObject btnCriacao;
    [SerializeField] private GameObject btnMissoes;
    [SerializeField] private GameObject slotItemPrefab;
    [SerializeField] private Transform posicaoDosIconesDeItens;
    [SerializeField] private GameObject abaSelecionarTempo;
    [SerializeField] private GameObject abaMissoes;
    [SerializeField] private GameObject abaConfimacaoMelhoriaPortal;
    public GameObject transicaoLevelsAnimacao;
    public GameObject caixaGuiaDeConstruao;
    public GameObject craftingBossFinal;
    private int TempoAtual = 0;
    private UpgradeSlot slotReceitaMelhoriaselecionada;
    public bool InventarioAberto => inventarioAberto;

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || (Input.GetKeyDown(KeyCode.Escape) && inventarioAberto)) && (jogadorScript.Instance.estadosJogador == jogadorScript.estados.EmAcao || jogadorScript.Instance.estadosJogador == jogadorScript.estados.EmUI) && !InterfaceMenu.Instance.pausado)
        {
            if (inventarioAberto)
            {
                if (TutorialSetUp.Instance == null)
                {
                    fechaInventario();
                }
                else
                {
                    if (TutorialSetUp.Instance.GetSequenciaDialogos() != 4)
                        fechaInventario();
                }
                //btnCriacao.SetActive(true);
            }
            else
            {
                if (TutorialSetUp.Instance == null)
                {
                    abreInventario(false, true);
                }
                else
                {
                    if (TutorialSetUp.Instance.GetSequenciaDialogos() != 4)
                        abreInventario(false, true);
                }
                //btnCriacao.SetActive(false);
                //abreInventario(false, true);
            }
        }
    }
    public void abreConfirmacaoMelhoriaBase()
    {
        inventarioAberto = true;
        abaConfimacaoMelhoriaPortal.SetActive(true);
    }
    public void fechaConfirmacaoMelhoriaBase()
    {
        inventarioAberto = false;
        abaConfimacaoMelhoriaPortal.SetActive(false);
    }
    public void fecharTodoInventario()
    {
        inventarioAberto = false;
        inventarioParent.SetActive(false);
        abaSelecionarTempo.SetActive(false);
    }
    public void abreInventario(bool ativarCriacao, bool ativarMissoes)
    {
        InterfaceMenu.Instance.podePausar = false;
        jogadorScript.Instance.MudarEstadoJogador(5);
        inventarioAberto = true;
        inventarioParent.SetActive(true);
        btnMissoes.SetActive(ativarMissoes);
        btnCriacao.SetActive(ativarCriacao);
        AoClicarBtnInventario();
    }
    public void fechaInventario()
    {
        //if (Time.timeScale == 0f)//Pausar
        //    Time.timeScale = 1f;
        jogadorScript.Instance.MudarEstadoJogador(0);
        inventarioParent.SetActive(false);
        inventarioAberto = false;
        HabilitarPausa();
    }
    IEnumerator HabilitarPausa()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        InterfaceMenu.Instance.podePausar = true;
    }
    public void abreMenuDeTempos()
    {
        jogadorScript.Instance.MudarEstadoJogador(5);
        inventarioAberto = true;
        abaSelecionarTempo.SetActive(true);
    }
    public void fechaMenuDeTempos()
    {
        jogadorScript.Instance.MudarEstadoJogador(0);
        inventarioAberto = false;
        abaSelecionarTempo.SetActive(false);
    }
    public void AoClicarBtnInventario()
    {
        //if (Time.timeScale == 0f)//Pausar
        //    Time.timeScale = 1f;
        abaInventario.SetActive(true);
        abaCriação.SetActive(false);
        abaMissoes.SetActive(false);
    }

    public void AoClicarBtnCriacao()
    {
        abaInventario.SetActive(false);
        abaCriação.SetActive(true);
    }
    public void AoClicarBtnMissoes()
    {
        abaInventario.SetActive(false);
        abaMissoes.SetActive(true);
        //Time.timeScale = 0f;//Pausar
    }
    public void AoClicarBtnConstruirMelhoriaBase()
    {
        fechaConfirmacaoMelhoriaBase();
        AprimorarBase();
    }

    private void CriaNovoSlotDeItem(item item, int quantidade)
    {
        GameObject obj = Instantiate(slotItemPrefab, posicaoDosIconesDeItens.position, Quaternion.identity, posicaoDosIconesDeItens);
        ItemSlot script = obj.GetComponent<ItemSlot>();
        script.item = item;
        script.atualizaQuantidade(quantidade);
        itens.Add(item.ID, obj.GetComponent<ItemSlot>());
    }
    public void AtualizaInventarioUI(item item, int quantidade)
    {
        if (itens.ContainsKey(item.ID))
        {
            if (itens[item.ID].GetQntdRecurso() + quantidade == 0)
            {
                itens[item.ID].atualizaQuantidade(quantidade);
                itens[item.ID].gameObject.SetActive(false);
                //Destroy(itens[item.ID].gameObject);
                //itens.Remove(item.ID);
            }
            else
            {
                itens[item.ID].atualizaQuantidade(quantidade);
                itens[item.ID].gameObject.SetActive(true);
            }
        }
        else
        {
            CriaNovoSlotDeItem(item, quantidade);
        }
        //bool JaNaLista = false;
        //int posNaLista = 0;
        //if (listaSlotItem.Count == 0) //lista vazia
        //{
        //    CriaNovoSlotDeItem(item, quantidade);
        //    return;
        //}
        //for (int i = 0; i < listaSlotItem.Count; i++)//se já existe no inventário, muda a quantidade
        //{
        //    if (listaSlotItem[i].item.ID.ToUpper() == item.ID.ToUpper())
        //    {
        //        JaNaLista = true;
        //        posNaLista = i;
        //        break;
        //    }
        //}
        //if (JaNaLista)
        //{
        //    listaSlotItem[posNaLista].atualizaQuantidade(quantidade);
        //}
        //else
        //{
        //    CriaNovoSlotDeItem(item, quantidade);
        //}
    }
    public void AoClicarParaConstruirItem(craftingSlot slot)
    {
        //if (slot.GetForcaFoiSelecionada())
        //{
            int possuiTodosOsRecursos = 0;
            for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.receita.itensNecessarios.Count; tiposRecursosParaCrafting++)//passa para o próximo recurso na lista de crafting do objeto
            {
                if (itens.ContainsKey(slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID))
                {
                    if (itens[slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID].qntdRecurso >= slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting])
                        possuiTodosOsRecursos++;
                    else
                        slot.FalhaNoCrafting(true, tiposRecursosParaCrafting);
                }
                else
                {
                    slot.FalhaNoCrafting(false, tiposRecursosParaCrafting);
                }
            }
            if (possuiTodosOsRecursos == slot.receita.itensNecessarios.Count)// caso tenha todos os itens e a quantidade necessária, consome eles para criar a receita
            {
                for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.receita.itensNecessarios.Count; tiposRecursosParaCrafting++)
                {
                    AtualizaInventarioUI(slot.receita.itensNecessarios[tiposRecursosParaCrafting], -slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting]);
                    //itens[slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID].atualizaQuantidade(-slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting]);
                }
            TocarSom(SoundManager.Som.BotaoConstruiuModulo, null);
                jogadorScript.Instance.SetModuloConstruido(slot.construcaoConfirmada());
                BaseScript.Instance.Ativar_DesativarVisualConstrucaoModulos(true);
                jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(BaseScript.Instance.transform, zoomOutAoConstruir);
                fechaInventario();
                jogadorScript.Instance.MudarEstadoJogador(2);
                if (TutorialSetUp.Instance != null)
                    caixaGuiaDeConstruao.SetActive(false);
            }
        //}
    }
    public void AoClicarparaMelhorar(UpgradeSlot slot)
    {
        slotReceitaMelhoriaselecionada = null;
        List<int> recursosEncontrados = new List<int>();
        int possuiTodosOsRecursos = 0;
        for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.GetReceita().itensNecessarios.Count; tiposRecursosParaCrafting++)//passa para o próximo recurso na lista de crafting do objeto
        {
            if (itens.ContainsKey(slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting].ID))
            {
                if (itens[slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting].ID].qntdRecurso >= slot.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting])
                    possuiTodosOsRecursos++;
                else
                    slot.FalhaNoCrafting(true, tiposRecursosParaCrafting);
            }
            else
            {
                slot.FalhaNoCrafting(false, tiposRecursosParaCrafting);
            }
        }
        if (possuiTodosOsRecursos == slot.GetReceita().itensNecessarios.Count)// caso tenha todos os itens e a quantidade necessária, consome eles para criar a receita
        {
            slotReceitaMelhoriaselecionada = slot;
            TocarSom(SoundManager.Som.BotaoConstruiuModulo, null);
            if (TutorialSetUp.Instance == null)
                abreConfirmacaoMelhoriaBase();
            else
                AprimorarBase();
        }
    }
    public void AprimorarBase()
    {
        for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slotReceitaMelhoriaselecionada.GetReceita().itensNecessarios.Count; tiposRecursosParaCrafting++)
        {
            AtualizaInventarioUI(slotReceitaMelhoriaselecionada.GetReceita().itensNecessarios[tiposRecursosParaCrafting], -slotReceitaMelhoriaselecionada.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting]);
            //itens[slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting].ID].atualizaQuantidade(-slot.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting]);
        }
        fechaMenuDeTempos();
        fechaConfirmacaoMelhoriaBase();
        //desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
        if (TutorialSetUp.Instance != null)
        {
            Tutorial();
            LiberarNovBtnDeTrocaDeTempo(slotReceitaMelhoriaselecionada, false);
        }
        else
            LiberarNovBtnDeTrocaDeTempo(slotReceitaMelhoriaselecionada, true);
    }
    public void AoClicarEmMudarDeTempo(UpgradeSlot slot)
    {
        if (TutorialSetUp.Instance != null)
            Tutorial();
        fechaMenuDeTempos();
        TransicaoDeFase.faseParaCarregar = slot.GetFaseParaAbrir();
        jogadorScript.Instance.IndicarInteracaoPossivel(0, false);
        if (BaseScript.Instance != null)
        {
            BaseScript.Instance.SalvarEstado();
            BaseScript.Instance.SalvarEstadosDosModulos();
        }
        transicaoLevelsAnimacao.SetActive(true);
        Ativar_DesativarTransicaoDeFase(true);
        //jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
        //if (BaseScript.Instance != null)
        //{
        //    BaseScript.Instance.SalvarEstado();
        //    BaseScript.Instance.SalvarEstadosDosModulos();
        //}
        //SceneManager.LoadScene(slot.FaseParaAbrir());
    }
    public void Ativar_DesativarTransicaoDeFase(bool b)
    {
        if (b)
        {
            jogadorScript.Instance.MudarEstadoJogador(1);
            transicaoLevelsAnimacao.GetComponent<Animator>().SetTrigger("INICIAR");
        }
        else
        {
            transicaoLevelsAnimacao.GetComponent<Animator>().SetTrigger("FINALIZAR");
        }
    }
    public void LiberarNovBtnDeTrocaDeTempo(UpgradeSlot slot, bool ativarDefesa)
    {
        slot.LiberarViagemNoTempo();
        TempoAtual++;
        if (TempoAtual < listaSlotUpgradesBase.Count)
            listaSlotUpgradesBase[TempoAtual].gameObject.SetActive(true); // liga o próximo botão da lista
        if (ativarDefesa)//quando começar a defesa não troca os desastres mais, mantem os que já estavam
        {
            desastreManager.Instance.encerramentoDesastres();
            BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(true);
            desastreManager.Instance.Ativar_desativarInteracoesDaBase(false, true);
            jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
            MissoesManager.Instance.ConcluirMissao(missoesDeLiberarNovoTempo[GetTempoAtual() - 2]);// -2 pois quando vai liberar o 1 tempo fora do tutorial o valo de tempoatual é 2
            desastreManager.Instance.ConfigurarTimer(BaseScript.Instance.GetIntervaloDuranteOAprimoramentoDaBase(), 0f, true);
            IndicadorDosDesastres.Instance.VerificarSeDefesaEstaPronta();
            //desastreManager.Instance.SortearDesastresGeral();
            //desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
            //desastreManager.Instance.PararTodasCorotinas();
            //desastreManager.Instance.IniciarCorrotinaLogicaDesastres(false);
            //desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
        }
    }
    public UpgradeSlot GetBtnEspecifico(int i)
    {
        return listaSlotUpgradesBase[i];
    }
    /*private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        TutorialSetUp.Instance.AoTerminoDoDialogoReparadaAMaquinaDoTempo();
    }*/
    public void Tutorial()
    {
        if (TempoAtual == 0)//antes de definir o tempo atual, inica um dialogo
        {
            //DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            //BaseScript.Instance.CancelarInscricaoEmDialogoFinalizado();
            //BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(false);//pois sempre q aperto para melhorar, é uma defesa da base
            BaseScript.Instance.animator.enabled = true;
            BaseScript.Instance.Ativa_DesativaAnimacaoDeNovoTempoLiberado(true);
            TutorialSetUp.Instance.IniciarDialogo();
        }
        else
        {
            //DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
            desastreManager.Instance.MudarTempoAcumuladoParaDesastre(0f);
            desastreManager.Instance.SortearDesastresGeral();
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
            desastreManager.Instance.IniciarCorrotinaLogicaDesastres(/*true*/);
        }
    }
    public bool VerificarSeLiberouBossFinal()
    {
        if (TempoAtual >= listaSlotUpgradesBase.Count)
            return true;
        else
            return false;
    }
    public int GetTempoAtual()
    {
        return TempoAtual;
    }
    public bool ProcurarChave(item chave)
    {
        if (chave != null)
        {
            if (itens.ContainsKey(chave.ID))
            {
                itens[chave.ID].atualizaQuantidade(-1);
                if (itens[chave.ID].qntdRecurso <= 0)
                {
                    Destroy(itens[chave.ID].gameObject);
                    itens.Remove(chave.ID);
                }
                return true;
            }
            else
                return false;
            //for (int i = 0; i < listaSlotItem.Count; i++)
            //{
            //    if (listaSlotItem[i].item.ID == chave.ID)
            //    {
            //        listaSlotItem[i].atualizaQuantidade(-1);
            //        if (listaSlotItem[i].qntdRecurso <= 0)
            //        {
            //            Destroy(listaSlotItem[i].gameObject);
            //            listaSlotItem.Remove(listaSlotItem[i]);
            //        }
            //        return true;
            //    }
            //}
            //return false;
        }
        else
            return true;
    }
    public void TocarSom(SoundManager.Som som, Transform origemSom)
    {
        SoundManager.Instance.TocarSom(som, origemSom);
    }
}
