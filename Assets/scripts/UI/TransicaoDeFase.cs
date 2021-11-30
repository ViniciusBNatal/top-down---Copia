using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransicaoDeFase : MonoBehaviour
{
    public static string faseParaCarregar;
    private Image sprite;
    private void Awake()
    {
        sprite = GetComponent<Image>();
    }
    public void TrocaLevel()
    {
        SceneManager.LoadScene(faseParaCarregar);
        if (faseParaCarregar == "BaseJogador" && desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
            sprite.enabled = false;
    }
    public void DesligarGameObject()
    {
        if (jogadorScript.Instance.estadosJogador != jogadorScript.estados.EmDialogo)
            jogadorScript.Instance.MudarEstadoJogador(0);
        this.gameObject.SetActive(false);
    }
    public void SomTrocaDeCena()
    {
        sprite.enabled = true;
        //SoundManager.Instance.TocarSom(SoundManager.Som.ViagemNoTempo);
    }
}
