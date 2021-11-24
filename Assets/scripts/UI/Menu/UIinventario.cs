﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIinventario : MonoBehaviour, AcoesNoTutorial
{
    public static UIinventario Instance { get; private set; }
    private bool inventarioAberto = false;
    [SerializeField] private List<UpgradeSlot> listaSlotUpgradesBase = new List<UpgradeSlot>();
    public float zoomOutAoConstruir;
    private Dictionary<string, ItemSlot> itens = new Dictionary<string, ItemSlot>();
    [Header("Nao Mexer")]
    [SerializeField] private GameObject CaixaDeDialogos;
    [SerializeField] private GameObject inventarioParent;
    [SerializeField] private GameObject abaInventario;
    [SerializeField] private GameObject abaCriação;
    [SerializeField] private GameObject btnCriacao;
    [SerializeField] private GameObject slotItemPrefab;
    [SerializeField] private Transform posicaoDosIconesDeItens;
    [SerializeField] private GameObject abaSelecionarTempo;
    public GameObject caixaGuiaDeConstruao;
    //[SerializeField] private GameObject abaVitoriaDoJogo;
    //[SerializeField] private GameObject abaDerrotaDoJogo;
    //[SerializeField] private GameObject abaPausa;
    //[SerializeField] private GameObject abaOpcoes;
    //[HideInInspector] public bool pausado = false;
    public GameObject craftingBossFinal;
    private int TempoAtual = 0;
    public bool InventarioAberto => inventarioAberto;

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && (jogadorScript.Instance.estadosJogador == jogadorScript.estados.EmAcao || jogadorScript.Instance.estadosJogador == jogadorScript.estados.EmUI) && /*!pausado*/!InterfaceMenu.Instance.pausado)
        {
            if (inventarioAberto)
            {
                fechaInventario();
                btnCriacao.SetActive(true);
            }
            else
            {
                btnCriacao.SetActive(false);
                abreInventario();
            }
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (!pausado)
        //        abrePausa();
        //    else
        //    {
        //        fechaPausa();
        //        fechaOpcoes();
        //    }
        //}
    }
    //public void abrePausa()
    //{
    //    pausado = true;
    //    inventarioAberto = true;
    //    abaPausa.SetActive(true);
    //    Time.timeScale = 0f;
    //}
    //public void fechaPausa()
    //{
    //    pausado = false;
    //    inventarioAberto = false;
    //    abaPausa.SetActive(false);
    //    Time.timeScale = 1f;
    //}
    //public void abreOpcoes()
    //{
    //    inventarioAberto = true;
    //    abaPausa.SetActive(false);
    //    abaOpcoes.SetActive(true);
    //}
    //public void fechaOpcoes()
    //{
    //    inventarioAberto = false;
    //    abaOpcoes.SetActive(false);
    //    abaPausa.SetActive(true);
    //}
    public void fecharTodoInventario()
    {
        inventarioAberto = false;
        inventarioParent.SetActive(false);
        abaSelecionarTempo.SetActive(false);
    }
    public void abreInventario()
    {
        jogadorScript.Instance.MudarEstadoJogador(5);
        inventarioAberto = true;
        inventarioParent.SetActive(true);
        AoClicarBtnInventario();
    }
    public void fechaInventario()
    {
        jogadorScript.Instance.MudarEstadoJogador(0);
        inventarioAberto = false;
        inventarioParent.SetActive(false);
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
    //public void abrirAbaDeGameOver()
    //{
    //    jogadorScript.Instance.MudarEstadoJogador(0);
    //    inventarioAberto = true;
    //    abaDerrotaDoJogo.SetActive(true);
    //}

    public void AoClicarBtnInventario()
    {
        abaInventario.SetActive(true);
        abaCriação.SetActive(false);
    }

    public void AoClicarBtnCriacao()
    {
        abaInventario.SetActive(false);
        abaCriação.SetActive(true);
    }

    private void CriaNovoSlotDeItem(Item item, int quantidade)
    {
        GameObject obj = Instantiate(slotItemPrefab, posicaoDosIconesDeItens.position, Quaternion.identity, posicaoDosIconesDeItens);
        ItemSlot script = obj.GetComponent<ItemSlot>();
        script.item = item;
        script.atualizaQuantidade(quantidade);
        itens.Add(item.ID, obj.GetComponent<ItemSlot>());
    }
    public void AtualizaInventarioUI(Item item, int quantidade)
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
            jogadorScript.Instance.SetModuloConstruido(slot.construcaoConfirmada());
            BaseScript.Instance.Ativar_DesativarVisualConstrucaoModulos(true);
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(BaseScript.Instance.transform, zoomOutAoConstruir);
            fechaInventario();
            jogadorScript.Instance.MudarEstadoJogador(2);
        }
    }
    public void AoClicarparaMelhorar(UpgradeSlot slot)
    {
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
            for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.GetReceita().itensNecessarios.Count; tiposRecursosParaCrafting++)
            {
                AtualizaInventarioUI(slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting], -slot.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting]);
                //itens[slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting].ID].atualizaQuantidade(-slot.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting]);
            }
            fechaMenuDeTempos();
            desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
            if (TutorialSetUp.Instance != null)
            {
                Tutorial();
                LiberarNovBtnDeTrocaDeTempo(slot, false);
            }
            else
                LiberarNovBtnDeTrocaDeTempo(slot, true);
        }
    }
    public void AoClicarEmMudarDeTempo(UpgradeSlot slot)
    {
        if (TutorialSetUp.Instance != null)
            Tutorial();
        fechaMenuDeTempos();
        // anim de trensição de mundo
        jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
        if (BaseScript.Instance != null)
        {
            BaseScript.Instance.SalvarEstado();
            BaseScript.Instance.SalvarEstadosDosModulos();
        }
        SceneManager.LoadScene(slot.FaseParaAbrir());
    }
    public void LiberarNovBtnDeTrocaDeTempo(UpgradeSlot slot, bool ativarDefesa)
    {
        slot.LiberarViagemNoTempo();
        TempoAtual++;
        if (TempoAtual < listaSlotUpgradesBase.Count)
            listaSlotUpgradesBase[TempoAtual].gameObject.SetActive(true); // liga o próximo botão da lista
        if (ativarDefesa)
        {
            desastreManager.Instance.encerramentoDesastres();
            BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(true);
            desastreManager.Instance.Ativar_desativarInteracoesDaBase(false, true);
            jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
            desastreManager.Instance.ConfigurarTimer(BaseScript.Instance.GetIntervaloDuranteOAprimoramentoDaBase(), 0f, true);
            desastreManager.Instance.PararTodasCorotinas();
            desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
        }
    }
    public UpgradeSlot GetBtnEspecifico(int i)
    {
        return listaSlotUpgradesBase[i];
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        TutorialSetUp.Instance.AoTerminoDoDialogoReparadaAMaquinaDoTempo();
    }
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
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
            desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
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
    public bool ProcurarChave(Item chave)
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
    private string NomeFasePorBuildIndex(int index)
    {
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(index);//pega o caminho da cena na pasta de arquivos
        string cenaParaAbrir = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        return cenaParaAbrir;
    }
    //public void AbrirVitoria()
    //{
    //    jogadorScript.Instance.MudarEstadoJogador(5);
    //    craftingBossFinal.SetActive(false);
    //    inventarioAberto = true;
    //    inventarioParent.SetActive(true);
    //    abaVitoriaDoJogo.SetActive(true);
    //}
}