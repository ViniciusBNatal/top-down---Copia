using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCscript : MonoBehaviour, SalvamentoEntreCenas
{
    private int nDeDialogos = 0;
    [SerializeField] private GameObject objetoDeMissao;
    [Range(-1,1)]
    [SerializeField] private int direcaoParaMoverX;
    [Range(-1, 1)]
    [SerializeField] private int direcaoParaMoverY;
    [SerializeField] private float velocidade;
    [SerializeField] private float distanciaMaxParaPerocrer;
    [SerializeField] private GameObject objetoParaCriar;
    [SerializeField] private Transform pontoDeSpawnObjeto;
    [SerializeField] private Dialogo[] dialogos;
    [SerializeField] private UnityEvent EventosAoCompletarMissao;
    private Item itemMissao = null;
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
        if (nDeDialogos == 0)
        {
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            DialogeManager.Instance.IniciarDialogo(dialogos[nDeDialogos]);
        }
        else
            VerificarMissao(true);
    }
    private void VerificarMissao(bool encadearDialogos)
    {
        if (nDeDialogos == 3)
        {
            DialogeManager.Instance.IniciarDialogo(dialogos[nDeDialogos]);//dialogo de missao já cumprida
        }
        else
        {
            if (UIinventario.Instance.ProcurarChave(itemMissao))
            {
                nDeDialogos = 2;
                DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                DialogeManager.Instance.IniciarDialogo(dialogos[nDeDialogos]);//dialogo de missao cumprida
            }
            else
            {
                if (encadearDialogos)
                {
                    nDeDialogos = 1;
                    DialogeManager.Instance.IniciarDialogo(dialogos[nDeDialogos]);//dialogo entre missao recebida e missao não cumprida
                }
                else
                    SalvarEstado();//salva q ja pegou missao e n cumpriu
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
        switch (nDeDialogos)
        {
            case 0:
                nDeDialogos = 1;
                VerificarMissao(false);
                break;
            case 2:
                nDeDialogos = 3;
                EventosAoCompletarMissao.Invoke();
                SalvarEstado();
                break;
            default:
                break;
        }
    }
    public void CriarObjeto()
    {
        Instantiate(objetoParaCriar, pontoDeSpawnObjeto.position, Quaternion.identity);
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
        if (nDeDialogos == 3)
            AoCompletarAMissao();
    }
    public Item GetItemDaMissao()
    {
        return itemMissao;
    }
    public void SetItemDaMissao(Item obj)
    {
        itemMissao = obj;
    }
    public int GetNDeDialogos()
    {
        return nDeDialogos;
    }
    public void SetNDeDialogo(int i)
    {
        nDeDialogos = i;
    }
    public void MovimentarNPC()
    {
        if (nDeDialogos == 3 && (direcaoParaMoverX != 0 || direcaoParaMoverY != 0))
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
