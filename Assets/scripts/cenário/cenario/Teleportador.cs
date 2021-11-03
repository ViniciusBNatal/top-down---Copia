using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleportador : MonoBehaviour
{
    [SerializeField] private Transform pontoDeChegada;
    const int BuildIndexDaFaseBaseJogador = 2;
    private void Start()
    {
        jogadorScript.Instance.transform.position = pontoDeChegada.position;
    }
    public void TeleportarPorInteracao()
    {
        CooldownDosRecursosManager.Instance.SalvarTempoDeSaida();
        jogadorScript.Instance.IndicarInteracaoPossivel(null, false);
        string IndexFaseBase = SceneUtility.GetScenePathByBuildIndex(BuildIndexDaFaseBaseJogador);//pega o caminho da cena na pasta de arquivos
        string cenaPrincipal = IndexFaseBase.Substring(0, IndexFaseBase.Length - 6).Substring(IndexFaseBase.LastIndexOf('/') + 1);
        SceneManager.LoadScene(cenaPrincipal);
    }
}
