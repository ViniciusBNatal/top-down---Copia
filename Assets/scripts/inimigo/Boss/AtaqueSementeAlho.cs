using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueSementeAlho : MonoBehaviour
{
    [Header("Valores Numéricos")]
    [SerializeField] private float velocidade;
    [SerializeField] private float danoAreaToxica;
    [SerializeField] private float danoDeContato;
    [SerializeField] private float tamanhoMax;
    [SerializeField] private float aumentoDeTamanho;
    [SerializeField] private float duracaoAreaToxica;
    [SerializeField] private float intervaloEntreHitsNaAreaToxica;
    [SerializeField] private float duracaoStun;
    [SerializeField] private float forcaRepulsao;
    [Header("Não Mexer")]
    [SerializeField] private caixa_recursos criaRecursos;
    [SerializeField] private CircleCollider2D areaExplosao;
    [SerializeField] private CircleCollider2D hitboxObjeto;
    private Rigidbody2D rb;
    private bool movimentar = false;
    private bool gasIniciado = false;
    private SpriteRenderer sprite;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (movimentar)
        {
            Vector2 direcaoMovimento = (jogadorScript.Instance.transform.position - transform.position).normalized;
            animator.SetFloat("MOVHORZ", direcaoMovimento.x);
            animator.SetFloat("MOVVERTC", direcaoMovimento.y);
            rb.velocity = direcaoMovimento * velocidade;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !gasIniciado)
        {
            gasIniciado = true;
            StartCoroutine(this.DanoGas());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gasIniciado = false;
        }
    }
    public void IniciarCrescimento()
    {
        animator.SetBool("ROLAR", true);
        movimentar = true;
        hitboxObjeto.enabled = true;
        StartCoroutine(this.aumentaTamanho());
    }
    private void Explodir()
    {
        movimentar = false;
        rb.velocity = Vector2.zero;
        areaExplosao.enabled = true;
        hitboxObjeto.enabled = false;
        sprite.enabled = false;
        animator.enabled = false;
        criaRecursos.QuebrarCaixa();
        StartCoroutine(this.DuracaoGas());
    }
    IEnumerator aumentaTamanho()
    {
        while (transform.localScale.x <= tamanhoMax && transform.localScale.y <= tamanhoMax)
        {
            transform.localScale = transform.localScale + new Vector3(transform.localScale.x * aumentoDeTamanho, transform.localScale.y * aumentoDeTamanho, 1f);
            yield return new WaitForSeconds(.2f);
        }
        animator.SetTrigger("EXPLODIR");
        yield break;
    }
    IEnumerator DanoGas()
    {
        while (gasIniciado)
        {
            jogadorScript.Instance.mudancaRelogio(danoAreaToxica);
            yield return new WaitForSeconds(intervaloEntreHitsNaAreaToxica);            
        }
    }
    IEnumerator DuracaoGas()
    {
        yield return new WaitForSeconds(duracaoAreaToxica);
        Destroy(this.gameObject);
    }
    public float GetDuracaoStun()
    {
        return duracaoStun;
    }
    public float GetDano()
    {
        return danoDeContato;
    }
    public float GetForcaDeRepulsao()
    {
        return forcaRepulsao;
    }
}
