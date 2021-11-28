using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoDeFase : MonoBehaviour
{
    public static string faseParaCarregar;
    public void TrocaLevel()
    {
        SceneManager.LoadScene(faseParaCarregar);
    }
    public void DesligarGameObject()
    {
        if (jogadorScript.Instance.estadosJogador != jogadorScript.estados.EmDialogo)
            jogadorScript.Instance.MudarEstadoJogador(0);
        this.gameObject.SetActive(false);
    }
    public void SomTrocaDeCena()
    {
        //SoundManager.Instance.TocarSom(SoundManager.Som.ViagemNoTempo);
    }
}
