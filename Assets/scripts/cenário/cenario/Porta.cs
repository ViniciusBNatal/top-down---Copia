using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    private int botoesPrecionados = 0;
    private BoxCollider2D colisao;
    private bool iniciouCorrotina = false;
    [SerializeField] private bool aberta;
    [SerializeField] private int nDeBotoesNecessarios;

    private void Start()
    {
        colisao = GetComponent<BoxCollider2D>();
        if (aberta)
            AbrePorta();
        else
            FechaPorta();
    }
    public void VerificarParaAbrirPorta(int valorBotao, float tempoAbrirPorta, float tempoFecharPorta)
    {
        botoesPrecionados += valorBotao;
        if (botoesPrecionados >= nDeBotoesNecessarios)
        {
            StartCoroutine(this.TempoPorta(tempoAbrirPorta, tempoFecharPorta));
        }
    }
    private IEnumerator TempoPorta(float tempAbrir, float tempFechar)
    {
        if (!iniciouCorrotina)
        {
            iniciouCorrotina = true;
            if (aberta)
            {
                yield return new WaitForSeconds(tempFechar);
                FechaPorta();
                iniciouCorrotina = false;
            }
            else
            {
                yield return new WaitForSeconds(tempAbrir);
                AbrePorta();
                iniciouCorrotina = false;
            }
        }
    }
    private void AbrePorta()
    {
        colisao.enabled = false;
        transform.Rotate(new Vector3(0f, 0f, 90f));
        aberta = true;
    }
    private void FechaPorta()
    {
        colisao.enabled = true;
        if (transform.rotation.z != 0f)
            transform.Rotate(new Vector3(0f, 0f,-90f));
        aberta = false;
    }
}
