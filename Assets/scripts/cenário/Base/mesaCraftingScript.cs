using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mesaCraftingScript : MonoBehaviour
{

    public void AbrirEFecharMenuDeCrafting()
    {
        if (!jogadorScript.Instance.inventario.InventarioAberto)
        {
            jogadorScript.Instance.inventario.abreInventario();
            jogadorScript.Instance.inventario.AoClicarBtnCriacao();
        }
        else
        {
            jogadorScript.Instance.inventario.fechaInventario();
            jogadorScript.Instance.inventario.AoClicarBtnInventario();
        }
    }
}
