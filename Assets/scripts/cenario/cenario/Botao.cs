using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Botao : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private UnityEvent evento;
    [Header("Compnentes para abrir porta")]
    [SerializeField] private Porta portaRelacionada;
    [SerializeField] private int valorBotao;
    [SerializeField] private bool usoUnico;
    private bool precionado = false;
    [Header("Compnentes para acao com inimigos")]
    [SerializeField] private bool Criar;
    [SerializeField] private GameObject inimigo;
    [SerializeField] private Transform pontoDeSpawnInimigo;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("USOUNICO", usoUnico);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("PRECIONADO", true);
            evento.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!usoUnico)
                animator.SetBool("PRECIONADO", false);
        }
    }
    public void TentarAbrirPorta()
    {
        if (usoUnico)
        {
            if (!precionado)
            {
                precionado = true;
                portaRelacionada.PortaPorBotao(valorBotao);
            }
        }
        else
        {
            portaRelacionada.PortaPorBotao(valorBotao);
        }
    }
    public void acaoComInimigos()
    {
        if (Criar)
        {
            GameObject obj = Instantiate(inimigo, pontoDeSpawnInimigo);
        }
        else
        {
            inimigo.GetComponent<inimigoScript>().enabled = true;
        }
    }
}
