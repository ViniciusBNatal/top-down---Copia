using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIinventario : MonoBehaviour
{
    public static UIinventario Instance { get; private set; }
    private bool inventarioAberto = false;
    private List<ItemSlot> listaSlotItem = new List<ItemSlot>();
    [SerializeField] private List<UpgradeSlot> listaSlotUpgradesBase = new List<UpgradeSlot>();
    [SerializeField] private GameObject jogador;
    [Header ("Nao Mexer")]
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
        if (listaSlotItem.Count == 0)
        {
            CriaNovoSlotDeItem(item, quantidade);
            return;
        }
        for (int i = 0; i < listaSlotItem.Count; i++)
        {
            if (listaSlotItem[i].item.ID.ToUpper() == item.ID.ToUpper())
            {
                listaSlotItem[i].atualizaQuantidade(quantidade);
                break;
            }
            else
            {
                CriaNovoSlotDeItem(item, quantidade);
            }
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
                if (listaSlotItem[tipoRecursosInventario].item.ID.ToUpper() == slot.receita.itensNecessarios[tiposRecursosParaCrafting].ID.ToUpper() && listaSlotItem[tipoRecursosInventario].qntdRecurso >= slot.GetNovaListaDeQntdRecursosNecessarios()[tiposRecursosParaCrafting])//slot.construcaoConfirmada().quantidadeDosRecursos[tiposRecursosParaCrafting]
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
                listaSlotItem[recursosEncontrados[tipoRecursoCrafting]].atualizaQuantidade(-slot.GetNovaListaDeQntdRecursosNecessarios()[tipoRecursoCrafting]);//-slot.construcaoConfirmada().quantidadeDosRecursos[tipoRecursoCrafting]
            }
            jogadorScript.Instance.moduloCriado = slot.construcaoConfirmada();
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(2);
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
            BaseScript.Instance.duranteMelhoria = true;
            for (int tipoRecursoCrafting = 0; tipoRecursoCrafting < slot.receita.itensNecessarios.Count; tipoRecursoCrafting++)
            {
                listaSlotItem[recursosEncontrados[tipoRecursoCrafting]].atualizaQuantidade(-slot.receita.quantidadeDosRecursos[tipoRecursoCrafting]);
            }
            //BaseScript.Instance.função q aumenta a qntd de defesas de acordo com algo
            slot.BtnConstruirUpgrade.GetComponent<Image>().sprite = slot.receita.iconeDeAprimoramentoDabase; // coloca a nova imagem no botão
            slot.BtnConstruirUpgrade.GetComponent<Button>().enabled = false; // desliga a função de botão do botão de criar o upgrade
            slot.BtnTrocartempo.SetActive(true);
            TempoAtual++;
            listaSlotUpgradesBase[TempoAtual].gameObject.SetActive(true); // liga o próximo botão da lista
            DesastresList.Instance.LiberarNovosDesastres(TempoAtual + 2);//ativa a possibilidade do evento desse tempo acontecer, +2 por já começar com 2 desastres
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloDuranteADefesa, desastreManager.Instance.tempoAcumulado);
            desastreManager.Instance.StartCoroutine(desastreManager.Instance.LogicaDesastres());
            fechaMenuDeTempos();
        }
    }
    public void AoClicarEmMudarDeTempo(UpgradeSlot slot)
    {
        Debug.Log("abre fase");
        //SceneManager.LoadScene(slot.CenaParaCarregar.ToUpper());
    }
}
