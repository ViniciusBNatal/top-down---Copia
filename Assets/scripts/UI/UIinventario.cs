using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIinventario : MonoBehaviour, AcoesNoTutorial
{
    public static UIinventario Instance { get; private set; }
    private bool inventarioAberto = false;
    [SerializeField] private bool tutorial;
    [SerializeField] private List<UpgradeSlot> listaSlotUpgradesBase = new List<UpgradeSlot>();
    private List<ItemSlot> listaSlotItem = new List<ItemSlot>();
    [Header("Nao Mexer")]
    [SerializeField] private GameObject CaixaDeDialogos;
    [SerializeField] private GameObject inventarioParent;
    [SerializeField] private GameObject abaInventario;
    [SerializeField] private GameObject abaCriação;
    [SerializeField] private GameObject btnCriacao;
    [SerializeField] private GameObject slotItemPrefab;
    [SerializeField] private Transform posicaoDosIconesDeItens;
    [SerializeField] private GameObject abaSelecionarTempo;
    [SerializeField] private GameObject abaVitoriaDoJogo;
    private int TempoAtual = 0;
    public bool InventarioAberto => inventarioAberto;

    private void Awake()
    {
        Instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
        listaSlotItem.Add(script);
    }
    public void AtualizaInventarioUI(Item item, int quantidade)
    {
        bool JaNaLista = false;
        int posNaLista = 0;
        if (listaSlotItem.Count == 0) //lista vazia
        {
            CriaNovoSlotDeItem(item, quantidade);
            return;
        }
        for (int i = 0; i < listaSlotItem.Count; i++)//se já existe no inventário, muda a quantidade
        {
            if (listaSlotItem[i].item.ID.ToUpper() == item.ID.ToUpper())
            {
                JaNaLista = true;
                posNaLista = i;
                break;
            }
        }
        if (JaNaLista)
        {
            listaSlotItem[posNaLista].atualizaQuantidade(quantidade);
        }
        else
        {
            CriaNovoSlotDeItem(item, quantidade);
        }
    }
    public void AoClicarParaConstruirItem(craftingSlot slot)
    {
        List<int> recursosEncontrados = new List<int>();
        int possuiTodosOsRecursos = 0;
        for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.receita.itensNecessarios.Count; tiposRecursosParaCrafting++)//passa para o próximo recurso na lista de crafting do objeto
        {
            for (int tipoRecursosInventario = 0; tipoRecursosInventario < listaSlotItem.Count; tipoRecursosInventario++)//procura se existe o recurso X na lista de crafting do objeto
            {
                if (listaSlotItem[tipoRecursosInventario].item.ID.ToUpper() == slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID.ToUpper() && listaSlotItem[tipoRecursosInventario].qntdRecurso >= slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting])//slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting]
                {
                    int i = tipoRecursosInventario;
                    recursosEncontrados.Add(i);// guarda a posição do recurso na lista de itens
                    possuiTodosOsRecursos++;
                    break;
                }
            }
        }
        if (possuiTodosOsRecursos == slot.receita.itensNecessarios.Count)// caso tenha todos os itens e a quantidade necessária, consome eles para criar a receita
        {
            for (int tipoRecursoCrafting = 0; tipoRecursoCrafting < slot.receita.itensNecessarios.Count; tipoRecursoCrafting++)
            {
                listaSlotItem[recursosEncontrados[tipoRecursoCrafting]].atualizaQuantidade(-slot.construcaoConfirmada().quantidadeDosRecursos[tipoRecursoCrafting]);//-slot.GetNovaListaDeQntdRecursosNecessarios()[tipoRecursoCrafting]               
            }
            jogadorScript.Instance.SetModuloConstruido(slot.construcaoConfirmada());
            BaseScript.Instance.Ativar_DesativarVisualConstrucaoModulos(true);
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(BaseScript.Instance.transform);
            fechaInventario();
            jogadorScript.Instance.MudarEstadoJogador(2);
        }
        else
        {
            Debug.Log("recursos insuficientes");
        }
    }
    public void AoClicarparaMelhorar(UpgradeSlot slot)
    {
        List<int> recursosEncontrados = new List<int>();
        int possuiTodosOsRecursos = 0;
        for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.GetReceita().itensNecessarios.Count; tiposRecursosParaCrafting++)//passa para o próximo recurso na lista de crafting do objeto
        {
            for (int tipoRecursosInventario = 0; tipoRecursosInventario < listaSlotItem.Count; tipoRecursosInventario++)//procura se existe o recurso X na lista de crafting do objeto
            {
                if (listaSlotItem[tipoRecursosInventario].item.ID.ToUpper() == slot.GetReceita().itensNecessarios[tiposRecursosParaCrafting].ID.ToUpper() && listaSlotItem[tipoRecursosInventario].qntdRecurso >= slot.GetReceita().quantidadeDosRecursos[tiposRecursosParaCrafting])
                {
                    int i = tipoRecursosInventario;
                    recursosEncontrados.Add(i);// guarda a posição do recurso na lista de itens
                    possuiTodosOsRecursos++;
                    break;
                }
            }
        }
        if (possuiTodosOsRecursos == slot.GetReceita().itensNecessarios.Count)// caso tenha todos os itens e a quantidade necessária, consome eles para criar a receita
        {
            for (int tipoRecursoCrafting = 0; tipoRecursoCrafting < slot.GetReceita().itensNecessarios.Count; tipoRecursoCrafting++)
            {
                listaSlotItem[recursosEncontrados[tipoRecursoCrafting]].atualizaQuantidade(-slot.GetReceita().quantidadeDosRecursos[tipoRecursoCrafting]);
            }
            fechaMenuDeTempos();
            Tutorial();
            LiberarNovBtnDeTrocaDeTempo(slot, !tutorial);
        }
    }
    public void AoClicarEmMudarDeTempo(UpgradeSlot slot)
    {
        Tutorial();
        fechaMenuDeTempos();
        jogadorScript.Instance.IndicarInteracaoPossivel(null, false);
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
            BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(true);
            desastreManager.Instance.encerramentoDesastres();
            desastreManager.Instance.ConfigurarTimer(BaseScript.Instance.GetIntervaloDuranteOAprimoramentoDaBase(), 0f);
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
        if (tutorial)
        {
            if (TempoAtual == 0)//antes de definir o tempo atual, inica um dialogo
            {
                //DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                BaseScript.Instance.CancelarInscricaoEmDialogoFinalizado();
                BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(false);//pois sempre q aperto para melhorar, é uma defesa da base
                TutorialSetUp.Instance.IniciarDialogo();
            }
            else
            {
                //DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
                desastreManager.Instance.MudarTempoAcumuladoParaDesastre(0f);
                desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre());
                desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
                tutorial = false;
            }
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
            for (int i = 0; i < listaSlotItem.Count; i++)
            {
                if (listaSlotItem[i].item.ID == chave.ID)
                {
                    listaSlotItem[i].atualizaQuantidade(-1);
                    if (listaSlotItem[i].qntdRecurso <= 0)
                    {
                        Destroy(listaSlotItem[i].gameObject);
                        listaSlotItem.Remove(listaSlotItem[i]);
                    }
                    return true;
                }
            }
            return false;
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
    public void AbrirVitoria()
    {
        jogadorScript.Instance.MudarEstadoJogador(5);
        inventarioAberto = true;
        inventarioParent.SetActive(true);
        abaVitoriaDoJogo.SetActive(true);
    }
    public void FecharJogo()
    {
        Application.Quit();
    }
}
