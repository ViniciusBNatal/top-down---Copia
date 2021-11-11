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
    [SerializeField] private float aumentoDeVelocidade;
    [SerializeField] private float duracaoAreaToxica;
    [SerializeField] private float intervaloEntreHitsNaAreaToxica;
    [SerializeField] private float duracaoStun;
    [SerializeField] private float forcaRepulsao;
    [Header("Não Mexer")]
    [SerializeField] private caixa_recursos criaRecursos;
    [SerializeField] private GameObject areaExplosao;
    [SerializeField] private CircleCollider2D hitboxObjeto;
    private Rigidbody2D rb;
    private bool movimentar = false;
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
    public void IniciarCrescimento()
    {
        animator.SetBool("ROLAR", true);
        movimentar = true;
        hitboxObjeto.enabled = true;
        StartCoroutine(this.aumentaTamanhoEVelocidade());
    }
    private void Explodir()
    {
        movimentar = false;
        rb.velocity = Vector2.zero;
        areaExplosao.SetActive(true);
        hitboxObjeto.enabled = false;
        sprite.enabled = false;
        animator.enabled = false;
        criaRecursos.QuebrarCaixa();
        StartCoroutine(this.DuracaoGas());
    }
    IEnumerator aumentaTamanhoEVelocidade()
    {
        while (transform.localScale.x <= tamanhoMax && transform.localScale.y <= tamanhoMax)
        {
            transform.localScale = transform.localScale + new Vector3(transform.localScale.x * aumentoDeTamanho, transform.localScale.y * aumentoDeTamanho, 1f);
            velocidade += aumentoDeVelocidade;
            yield return new WaitForSeconds(.2f);
        }
        animator.SetTrigger("EXPLODIR");
        yield break;
    }
    IEnumerator DuracaoGas()
    {
        yield return new WaitForSeconds(duracaoAreaToxica);
        BossAlho.Instance.RemoverAtqDaLista(this.gameObject);
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
    public float GetDanoAreaToxica()
    {
        return danoAreaToxica;
    }
    public float GetIntervaloHitsAraToxica()
    {
        return intervaloEntreHitsNaAreaToxica;
    }
}
