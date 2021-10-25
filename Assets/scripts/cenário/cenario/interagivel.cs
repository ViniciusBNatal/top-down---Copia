using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class interagivel : MonoBehaviour
{
    private bool emAlcance;
    [SerializeField] private KeyCode btnInteragir;
    [SerializeField] private UnityEvent acao;

    // Update is called once per frame
    void Update()
    {
        if (emAlcance)
        {
            if (Input.GetKeyDown(btnInteragir) && (jogadorScript.Instance.EstadoAtualJogador() == jogadorScript.Instance.CompararComEstado(0) || jogadorScript.Instance.EstadoAtualJogador() == jogadorScript.Instance.CompararComEstado(1)))
            {
                acao.Invoke();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            emAlcance = true;
            collision.GetComponent<jogadorScript>().IndicarInteracaoPossivel(SelecionadorDeIconeDeInteracao.Instance.SelecionaIconeDeInteracao(btnInteragir), emAlcance);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            emAlcance = false;
            collision.GetComponent<jogadorScript>().IndicarInteracaoPossivel(SelecionadorDeIconeDeInteracao.Instance.SelecionaIconeDeInteracao(btnInteragir), emAlcance);

        }
    }
}
