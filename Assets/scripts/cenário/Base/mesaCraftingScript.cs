using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesaCraftingScript : MonoBehaviour
{
    public static mesaCraftingScript Instance { get; private set; }
    [SerializeField] private GameObject areaDeInteracao;
    private void Awake()
    {
        Instance = this;
    }
    public void AbrirEFecharMenuDeCrafting()
    {
        if (!jogadorScript.Instance.InterfaceJogador.InventarioAberto)
        {
            jogadorScript.Instance.InterfaceJogador.abreInventario();
            jogadorScript.Instance.InterfaceJogador.AoClicarBtnCriacao();
        }
        else
        {
            jogadorScript.Instance.InterfaceJogador.fechaInventario();
            jogadorScript.Instance.InterfaceJogador.AoClicarBtnInventario();
        }
    }
    public void Desativar_AtivarInteracao(bool b)
    {
        areaDeInteracao.SetActive(b);
    }
}
