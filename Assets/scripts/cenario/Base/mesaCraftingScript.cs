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
            if (TutorialSetUp.Instance != null)
            {
                if (TutorialSetUp.Instance.GetSequenciaDialogos() == 4)
                {
                    Desativar_AtivarInteracao(false);
                    jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
                    UIinventario.Instance.caixaGuiaDeConstruao.SetActive(true);
                }
            }
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
