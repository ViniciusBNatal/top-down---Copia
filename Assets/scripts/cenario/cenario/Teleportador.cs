using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportador : MonoBehaviour
{
    [SerializeField] private Transform pontoDeChegada;
    [SerializeField] private DialogoUnico dialogoInicial;
    const string NomeFaseDeRotornoDosteleportes = "BaseJogador";
    private void Start()
    {
        jogadorScript.Instance.transform.position = pontoDeChegada.position;
        DialogoInicial();
    }
    public void TeleportarPorInteracao()
    {
        SalvamentoDosCentrosDeRecursosManager.Instance.SalvarTempoDeSaida();
        SalvamentoDosCentrosDeRecursosManager.Instance.SalvarCentrosDeRecursoDaCenaAtual();
        jogadorScript.Instance.IndicarInteracaoPossivel(0f, false);
        TransicaoDeFase.faseParaCarregar = NomeFaseDeRotornoDosteleportes;
        UIinventario.Instance.transicaoLevelsAnimacao.SetActive(true);
        UIinventario.Instance.Ativar_DesativarTransicaoDeFase(true);
        //SceneManager.LoadScene(cenaPrincipal);
    }
    private void DialogoInicial()
    {
        if (dialogoInicial != null && !dialogoInicial.GetDialogoORealizado())
        {
            dialogoInicial.AtivarDialogo();
            DesastresList.Instance.LiberarNovosDesastres(UIinventario.Instance.GetTempoAtual());
        }
    }
}
