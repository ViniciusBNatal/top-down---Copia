using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesaCraftingScript : MonoBehaviour
{

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
}
