using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnxameDeInsetos : MonoBehaviour
{
    [Header("Valores numéricos")]
    [SerializeField] private float velocidade;
    [SerializeField] private float dano;
    [SerializeField] private float intervaloEntreAtaques;
    private Transform alvo;
    private Vector2 direcao;
    private Rigidbody2D rb;
    private bool jogadorNoEnxame = false;
    public static EnxameDeInsetos Instance { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        alvo = jogadorScript.Instance.transform;
        StartCoroutine("movimentacao");
    }
    IEnumerator movimentacao()
    {
        while (true)
        {
            direcao = (alvo.position - transform.position).normalized;
            rb.velocity = direcao * velocidade;
            yield return new WaitForSeconds(.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jogadorNoEnxame = true;
            StartCoroutine("ferreJogador");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jogadorNoEnxame = false;
        }
    }
    IEnumerator ferreJogador()
    {
        while (jogadorNoEnxame)
        {
            jogadorScript.Instance.mudancaRelogio(dano, .15f);
            yield return new WaitForSeconds(intervaloEntreAtaques);
        }
    }
}