using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCscript : MonoBehaviour, SalvamentoEntreCenas
{
    private const int nDeDialogos = 3;
    [SerializeField] private GameObject objetoDeMissao;
    [Range(-1,1)]
    [SerializeField] private int direcaoParaMoverX;
    [Range(-1, 1)]
    [SerializeField] private int direcaoParaMoverY;
    [SerializeField] private float velocidade;
    [SerializeField] private float distanciaMaxParaPerocrer;
    [SerializeField] private UnityEvent EventosAoCompletarMissao;
    [SerializeField] private Dialogo[] dialogos = new Dialogo[nDeDialogos];
    private Item itemMissao = null;
    private bool missaoCumprida = false;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 posInicial;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        posInicial = transform.position;
        if (objetoDeMissao != null && itemMissao == null)
        {
            objetoDeMissao.GetComponent<recurso_coletavel>().SetNPC(this.gameObject);
            itemMissao = objetoDeMissao.GetComponent<recurso_coletavel>().ReferenciaItem();
        }
    }
    private void FixedUpdate()
    {
        MovimentarNPC();
    }
    public void Interacao()
    {
        if (itemMissao != null)
        {
            VerificarMissao();
        }
        else//se ele for apenas para conversar
        {
            DialogeManager.Instance.IniciarDialogo(dialogos[0]);
        }
    }
    private void VerificarMissao()
    {
        if (missaoCumprida)
        {
            DialogeManager.Instance.IniciarDialogo(dialogos[2]);//dialogo de missao ja foi cumprida
        }
        else
        {
            if (UIinventario.Instance.ProcurarChave(itemMissao))
            {
                DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                DialogeManager.Instance.IniciarDialogo(dialogos[1]);//dialogo de missao cumprida
            }
            else
            {
                DialogeManager.Instance.IniciarDialogo(dialogos[0]);//dialogo de falar qual a missao
            }
        }
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        AoCompletarAMissao();
        DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();       
    }
    private void AoCompletarAMissao()
    {
        missaoCumprida = true;
        EventosAoCompletarMissao.Invoke();
        SalvarEstado();
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosNPCs(this, 0);
        }
    }

    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosNPCs(this, 1);
        if (missaoCumprida)
            AoCompletarAMissao();
    }
    public bool GetMissaoCumprida()
    {
        return missaoCumprida;
    }
    public Item GetItemDaMissao()
    {
        return itemMissao;
    }
    public void SetMissaoCumprida(bool b)
    {
        missaoCumprida = b;
    }
    public void SetItemDaMissao(Item obj)
    {
        itemMissao = obj;
    }
    public void MovimentarNPC()
    {
        if (missaoCumprida)
        {
            animator.SetFloat("HORZ", direcaoParaMoverX);
            animator.SetFloat("VERTC", direcaoParaMoverY);
            if (Mathf.Abs(transform.position.x - posInicial.x) * direcaoParaMoverX < distanciaMaxParaPerocrer && Mathf.Abs(transform.position.y - posInicial.y) * direcaoParaMoverY < distanciaMaxParaPerocrer)
                rb.velocity = new Vector2(direcaoParaMoverX, direcaoParaMoverY) * velocidade;
            else
                rb.velocity = Vector2.zero;
        }
    }
}
