using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueMisselParaTempo : MonoBehaviour
{
    [Header("Variáveis do Missel")]
    [SerializeField] private GameObject gobjPai;
    [SerializeField] private float duracaoExplosao;
    [Header("Componentes do recurso dropado")]
    [SerializeField] private float forca;
    [SerializeField] private List<item> itens = new List<item>();
    [SerializeField] private List<int> qntdDoRecursoDropado = new List<int>();
    [Header("Não Mexer")]
    [SerializeField] private SpriteRenderer areaDaExplosaoIndicador;
    [SerializeField] private GameObject recursoColetavelPreFab;
    private CircleCollider2D areaDaExplosao;
    private List<GameObject> recursosCriados = new List<GameObject>();
    private Animator animator;
    private bool jogadorAcertado = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        areaDaExplosao = GetComponent<CircleCollider2D>();
    }
    public void AoAtingirOChao()
    {
        areaDaExplosao.enabled = true;
        areaDaExplosaoIndicador.enabled = false;
        //animator.enabled = false;
        for (int i = 0; i < itens.Count; i++)
        {
            GameObject recurso = Instantiate(recursoColetavelPreFab, transform);
            recurso.transform.SetParent(null);//desasosia recursos do missel
            recurso.GetComponent<recurso_coletavel>().DefineItem(itens[i]);
            recurso.GetComponent<recurso_coletavel>().DefineQuantidadeItem(qntdDoRecursoDropado[i]);
            recursosCriados.Add(recurso);
        }
        animator.SetTrigger("EXPLODIR");
        StartCoroutine(this.destruirAposTempo());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jogadorAcertado = true;
            jogadorScript.Instance.MudarEstadoJogador(1);
            jogadorScript.Instance.GetComponent<Animator>().speed = 0f;
        }
    }
    IEnumerator destruirAposTempo()
    {
        yield return new WaitForSeconds(duracaoExplosao);
        if (jogadorAcertado)
        {
            jogadorScript.Instance.GetComponent<Animator>().speed = 1f;
            jogadorScript.Instance.MudarEstadoJogador(0);
        }
        for (int i = 0; i < recursosCriados.Count; i++)
        {
            if (recursosCriados[i] != null)
                recursosCriados[i].GetComponent<recurso_coletavel>().LancaRecurso(forca, 0f, 0f);
        }
        if (BossAlho.Instance != null)
            BossAlho.Instance.RemoverAtqDaLista(gobjPai.gameObject);
        Destroy(gobjPai);
    }
    public void FimAnimacao()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
