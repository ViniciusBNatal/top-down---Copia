using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIinventario : MonoBehaviour, AcoesNoTutorial
{
    public static UIinventario Instance { get; private set; }
    private bool inventarioAberto = false;
    [SerializeField] private bool tutorial;
    [SerializeField] private List<UpgradeSlot> listaSlotUpgradesBase = new List<UpgradeSlot>();
    private List<Teleportador> listaTeleportadores = new List<Teleportador>();
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
        jogadorScript.Instance.MudarEstadoJogador(1);
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
        jogadorScript.Instance.MudarEstadoJogador(1);
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
        for (int tiposRecursosParaCrafting = 0; tiposRecursosParaCrafting < slot.receita.itensNecessarios.Count; tiposRecursosParaCrafting++)//passa para o próximo recurso na lista de crafting do objeto
        {
            for (int tipoRecursosInventario = 0; tipoRecursosInventario < listaSlotItem.Count; tipoRecursosInventario++)//procura se existe o recurso X na lista de crafting do objeto
            {
                if (listaSlotItem[tipoRecursosInventario].item.ID.ToUpper() == slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID.ToUpper() && listaSlotItem[tipoRecursosInventario].qntdRecurso >= slot.receita.quantidadeDosRecursos[tiposRecursosParaCrafting])
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
            BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(true);
            for (int tipoRecursoCrafting = 0; tipoRecursoCrafting < slot.receita.itensNecessarios.Count; tipoRecursoCrafting++)
            {
                listaSlotItem[recursosEncontrados[tipoRecursoCrafting]].atualizaQuantidade(-slot.receita.quantidadeDosRecursos[tipoRecursoCrafting]);
            }
            fechaMenuDeTempos();
            Tutorial();
            LiberarNovBtnDeTrocaDeTempo(slot, !tutorial);
        }
    }
    public void AoClicarEmMudarDeTempo(UpgradeSlot slot)
    {
        int index = 0;
        for (int i = 0; i < listaTeleportadores.Count; i++)
        {
            if (slot.fase == listaTeleportadores[i].GetFaseDoTeleportador())
            {
                index = i;
                break;
            }
        }
        jogadorScript.Instance.transform.position = listaTeleportadores[index].GetPosicao().position;
        Tutorial();
        fechaMenuDeTempos();
    }
    public void LiberarNovBtnDeTrocaDeTempo(UpgradeSlot slot, bool ativarDesastres)
    {
        slot.LiberarViagemNoTempo();
        TempoAtual++;
        if (TempoAtual < listaSlotUpgradesBase.Count)
            listaSlotUpgradesBase[TempoAtual].gameObject.SetActive(true); // liga o próximo botão da lista
        if (ativarDesastres)
        {
            BaseScript.Instance.Ativar_DesativarDuranteDefesaParaMelhorarBase(true);
            desastreManager.Instance.encerramentoDesastres();
            desastreManager.Instance.ConfigurarTimer(BaseScript.Instance.GetIntervaloDuranteOAprimoramentoDaBase(), 0f);
            desastreManager.Instance.StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
            desastreManager.Instance.StopAllCoroutines();
        }
    }
    public void AdicionaTeleportadorALista(Teleportador obj)
    {
        listaTeleportadores.Add(obj);
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
                StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
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
}
