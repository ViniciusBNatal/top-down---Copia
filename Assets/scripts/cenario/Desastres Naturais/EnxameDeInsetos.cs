using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnxameDeInsetos : MonoBehaviour, TocarSom
{
    [Header("Valores numéricos")]
    [SerializeField] private float velocidade;
    [SerializeField] private float dano;
    [SerializeField] private float desaceleracao;
    /*[SerializeField]*/ private float intervaloEntreAtaques;
    [SerializeField] private float forcaEmpurrao;
    [SerializeField] private float duracaoStun;
    private Transform alvo;
    private Vector2 direcao;
    private Rigidbody2D rb;
    private bool jogadorNoEnxame = false;
    public static EnxameDeInsetos Instance { get; private set; }
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        alvo = jogadorScript.Instance.transform;
        TocarSom(SoundManager.Som.DesastreEnxame, this.transform);
        StartCoroutine("movimentacao");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jogadorScript.Instance.SetDesaceleracao(0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jogadorScript.Instance.SetDesaceleracao(desaceleracao);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        jogadorScript.Instance.Knockback(duracaoStun, forcaEmpurrao, this.transform);
    //    }
    //}
    IEnumerator movimentacao()
    {
        while (true)
        {
            direcao = (alvo.position - transform.position).normalized;
            rb.velocity = direcao * velocidade;
            yield return new WaitForSeconds(.2f);
        }
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        jogadorNoEnxame = true;
    //        StartCoroutine("ferreJogador");
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        jogadorNoEnxame = false;
    //    }
    //}
    IEnumerator ferreJogador()
    {
        while (jogadorNoEnxame)
        {
            jogadorScript.Instance.mudancaRelogio(dano, .15f);
            yield return new WaitForSeconds(intervaloEntreAtaques);
        }
    }
    public void TocarSom(SoundManager.Som som, Transform origemSom)
    {
        SoundManager.Instance.TocarSom(som, origemSom);
    }
}