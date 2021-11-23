using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportador : MonoBehaviour
{
    [SerializeField] private Transform pontoDeChegada;
    [SerializeField] private DialogoUnico dialogoInicial;
    const int BuildIndexDaFaseBaseJogador = 2;
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
        // anim de trensição de mundo
        string IndexFaseBase = SceneUtility.GetScenePathByBuildIndex(BuildIndexDaFaseBaseJogador);//pega o caminho da cena na pasta de arquivos
        string cenaPrincipal = IndexFaseBase.Substring(0, IndexFaseBase.Length - 6).Substring(IndexFaseBase.LastIndexOf('/') + 1);
        SceneManager.LoadScene(cenaPrincipal);
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
