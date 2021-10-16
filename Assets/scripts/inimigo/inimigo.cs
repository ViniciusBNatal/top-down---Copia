using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    [Header("Caracteristicas do Inimigo")]
    [SerializeField] private bool imortal;
    public bool movimentacaoFixa;
    [SerializeField] private bool movimentacaoLivre;
    [SerializeField] private bool disparo;
    [SerializeField] private CircleCollider2D areaDetecao;
    [SerializeField] private GameObject projetil;
    [SerializeField] private Transform pontoDisparo;
    private Rigidbody2D rb;
    [Header("Valores numéricos")]
    [SerializeField] private float vidaMaxima;
    [SerializeField] private float velocidade;
    [SerializeField] private float taxaDisparo;
    [SerializeField] private float raioVisao;
    [SerializeField] private float velocidadeProjetil;
    public float danoMelee;
    [SerializeField] private float danoRanged;
    public float forcaRepulsao;
    public float tempoDeStunNoJogador = 1f;
    public Vector2 direcao;
    //variaveis privadas
    private Vector2 direcaoProjetil;
    private Transform alvo;
    private float vidaAtual;
    private bool atirando = false;
    // Start is called before the first frame update
    void Start()
    {
        areaDetecao.radius = raioVisao;
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
    }

    private void FixedUpdate()
    {
        if (movimentacaoFixa)
        {
            rb.velocity = (direcao * velocidade);
        }
        else if (alvo != null)
        {
          orientacaoSprite();
            if (movimentacaoLivre)
            {
                direcao = (alvo.position - transform.position).normalized;
                rb.velocity = (direcao * velocidade);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//jogador entrou da área de detecção
    {
        if(collision.gameObject.tag == "Player")
        {
            alvo = collision.transform;
            if (disparo)
            {
                if (!atirando)
                    StartCoroutine("Disparar");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)//jogador saiu da área de detecção
    {
        if (collision.gameObject.tag == "Player")
        {
            alvo = null;
            rb.velocity = Vector2.zero;
        }
    }
    IEnumerator Disparar()
    {
        while(alvo != null)
        {
            atirando = true;
            direcaoProjetil = (alvo.position - pontoDisparo.position).normalized;
            GameObject balains = Instantiate(projetil, pontoDisparo.position, Quaternion.identity);
            balains.GetComponent<Rigidbody2D>().velocity = velocidadeProjetil * direcaoProjetil;
            balains.GetComponent<balaHit>().dano = danoRanged;
            yield return new WaitForSeconds(taxaDisparo);
            atirando = false;
        }
    }
    private void orientacaoSprite()// roda o sprite de acordo com a posição do jogador
    {
        if (direcao.x < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direcao.x > 0)
            transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    public void mudancaVida(float valor)
    {
        if (!imortal)
        {
            vidaAtual += valor;
            if (vidaAtual > vidaMaxima)
            {
                vidaAtual = vidaMaxima;
            }
            else if (vidaAtual <= 0)
            {
                vidaAtual = 0f;
                Destroy(this.gameObject);
            }
        }
    }
}