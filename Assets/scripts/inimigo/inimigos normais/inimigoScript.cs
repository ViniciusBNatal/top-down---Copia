using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoScript : MonoBehaviour
{
    [Header("Caracteristicas do Inimigo")]
    [SerializeField] private bool imortal;
    public TiposDeMovimentacao tiposDeMovimentacao;
    [SerializeField] private bool disparo;
    //[SerializeField] private CircleCollider2D areaDetecao;
    [SerializeField] private GameObject projetil;
    [SerializeField] private Transform pontoDisparo;
    [SerializeField] private List<Transform> pontosDeFuga = new List<Transform>();
    private Rigidbody2D rb;
    private bool movimentacaoFixa = false;
    [Header("Valores numéricos")]
    [SerializeField] private float vidaMaxima;
    [SerializeField] private float velocidade;
    [SerializeField] private float taxaDisparo;
    //[SerializeField] private float raioVisao;
    [SerializeField] private float velocidadeProjetil;
    public float danoMelee;
    [SerializeField] private float danoRanged;
    public float forcaRepulsao;
    public float tempoDeStunNoJogador = 1f;
    [SerializeField] private Vector2 distanciaMinimaParaFugir;
    public Vector2 direcaoDeMovimentacao;
    //variaveis privadas
    [Header("Não Mexer")]
    private inimigoAnimScript inimigoAnimScript;
    private Vector2 direcaoProjetil;
    private Transform alvo;
    private float vidaAtual;
    private bool atirando = false;
    public enum TiposDeMovimentacao
    {
        estatico,
        movimentacaoLivre,
        movimentacaoFixa,
        movimentacaoEntrePontosAleatorios,
        movimentacaoEntrePontosFixa
    }
    private int proximoPontoDeFuga = 0;
    private CentroDeRecursoInfinito CentroDeSpawn = null;
    // Start is called before the first frame update
    void Start()
    {
        //areaDetecao.radius = raioVisao;
        rb = GetComponent<Rigidbody2D>();
        inimigoAnimScript = GetComponent<inimigoAnimScript>();
        vidaAtual = vidaMaxima;
        if (tiposDeMovimentacao == TiposDeMovimentacao.movimentacaoFixa)
            movimentacaoFixa = true;
        if (CentroDeSpawn != null && !CentroDeSpawn.GetCentroDeInimigos())
        {
            CentroDeSpawn.InimigoDerrotado();
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        //orientacaoSprite();
        switch (tiposDeMovimentacao)
        {
            case TiposDeMovimentacao.estatico:
                break;
            case TiposDeMovimentacao.movimentacaoFixa:
                rb.velocity = (direcaoDeMovimentacao * velocidade);
                break;
            case TiposDeMovimentacao.movimentacaoLivre:
                if (alvo != null)
                {
                    direcaoDeMovimentacao = (alvo.position - transform.position).normalized;
                    rb.velocity = (direcaoDeMovimentacao * velocidade);
                }
                break;
            case TiposDeMovimentacao.movimentacaoEntrePontosFixa:

                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && distanciaMinimaParaFugir != Vector2.zero)
        {
            Vector2 distanciaDoAlvo = alvo.position - transform.position;
            if (Mathf.Abs(distanciaDoAlvo.x) <= distanciaMinimaParaFugir.x && Mathf.Abs(distanciaDoAlvo.y) <= distanciaMinimaParaFugir.y)
            {
                Teleportar(tiposDeMovimentacao);
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
    public void Teleportar(TiposDeMovimentacao tipoDeMovimentacao)// 1 movimentação fixa, 2 movimentação aleatória
    {
        switch (tipoDeMovimentacao)
        {
            case TiposDeMovimentacao.movimentacaoEntrePontosFixa:
                if (proximoPontoDeFuga <= pontosDeFuga.Count - 1)
                {
                    transform.position = pontosDeFuga[proximoPontoDeFuga].position;
                    proximoPontoDeFuga++;
                }
                break;
            case TiposDeMovimentacao.movimentacaoEntrePontosAleatorios:
                int r = Random.Range(0, pontosDeFuga.Count);
                transform.position = pontosDeFuga[r].position;
                break;
        }
    }
    IEnumerator Disparar()
    {
        while(alvo != null)
        {
            atirando = true;
            direcaoProjetil = (alvo.position - pontoDisparo.position).normalized;
            inimigoAnimScript.AtaqueRanged(direcaoProjetil);
            GameObject balains = Instantiate(projetil, pontoDisparo.position, Quaternion.identity);
            balains.transform.Rotate(new Vector3(0f, 0f, Mathf.Atan2(direcaoProjetil.y, direcaoProjetil.x) * Mathf.Rad2Deg));
            balains.GetComponent<Rigidbody2D>().velocity = velocidadeProjetil * direcaoProjetil;
            balains.GetComponent<balaHit>().SetDano(danoRanged);
            yield return new WaitForSeconds(taxaDisparo);
            atirando = false;
        }
    }
    private void orientacaoSprite()// roda o sprite de acordo com a posição do jogador
    {
        if (direcaoDeMovimentacao.x < 0)
            transform.localScale = new Vector3(1f, 1f, 1f);
        else if (direcaoDeMovimentacao.x > 0)
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
                if (CentroDeSpawn != null)
                {
                    CentroDeSpawn.InimigoDerrotado();
                }
                vidaAtual = 0f;
                Destroy(this.gameObject);
            }
        }
    }
    public bool GetMovimentacaoFixa()
    {
        return movimentacaoFixa;
    }
    public inimigoAnimScript GetAnimScript()
    {
        return inimigoAnimScript;
    }
    public void SetCentroDeSpawn(CentroDeRecursoInfinito cDeSpawn)
    {
        CentroDeSpawn = cDeSpawn;
    }
}