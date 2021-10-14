using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    [Header("Caracteristicas do Inimigo")]
    public bool movimentacaoFixa;
    public bool movimentacaoLivre;
    public bool disparo;
    public CircleCollider2D areaDetecao;
    public GameObject projetil;
    public Transform pontoDisparo;
    public Rigidbody2D rb;
    [Header("Valores numéricos")]
    public float vidaMaxima;
    public float velocidade;
    public float taxaDisparo;
    public float raioVisao;
    public float velocidadeProjetil;
    public float danoMelee;
    public float danoRanged;
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