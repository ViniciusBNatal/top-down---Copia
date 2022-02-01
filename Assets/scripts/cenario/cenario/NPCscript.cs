using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCscript : MonoBehaviour, SalvamentoEntreCenas
{
    private int nDeDialogos = 0;
    [SerializeField] private GameObject objetoDeMissao;
    [SerializeField] private Missao missaoNPC;
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
    private item itemMissao = null;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 posInicial;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        posInicial = transform.position;
    }
    private void Start()
    {
        if (objetoDeMissao != null && itemMissao == null)
        {
            objetoDeMissao.GetComponent<recurso_coletavel>().SetNPC(this.gameObject);
            itemMissao = objetoDeMissao.GetComponent<recurso_coletavel>().ReferenciaItem();
        }
    }
    private void FixedUpdate()
    {
        //MovimentarNPC();
    }
    public void Interacao()
    {
        if (dialogos.Length > 1)
        {
            if (nDeDialogos == 0)
            {
                DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                DialogeManager.Instance.IniciarDialogo(dialogos[nDeDialogos]);
            }
            else
                VerificarMissao(true);
        }
        else
            DialogeManager.Instance.IniciarDialogo(dialogos[0]);
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
                DialogeManager.Instance.limparDelegate = true;
            }
        }
    }
    public void AtualizarStatusMissao(bool finalizada)
    {
        if (!finalizada)
            MissoesManager.Instance.AdicionarMissao(missaoNPC);
        else
            MissoesManager.Instance.ConcluirMissao(missaoNPC);
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        AoCompletarAMissao();
        //DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();       
    }
    private void AoCompletarAMissao()
    {
        switch (nDeDialogos)
        {
            case 0:
                nDeDialogos = 1;
                DialogeManager.Instance.limparDelegate = false;
                VerificarMissao(false);
                break;
            case 2:
                nDeDialogos = 3;
                DialogeManager.Instance.limparDelegate = true;
                EventosAoCompletarMissao.Invoke();
                //StartCoroutine(this.Movimentacao());
                SalvarEstado();
                break;
            case 3:
                EventosAoCompletarMissao.Invoke();
                //StartCoroutine(this.Movimentacao());
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
            GetComponent<SalvarEstadoDoObjeto>().AtivarCarregamentoDoObjeto();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosNPCs(this, 0);
        }
    }

    public void CarregarDados()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosNPCs(this, 1);
        if (nDeDialogos == 3)
            AoCompletarAMissao();
    }
    public item GetItemDaMissao()
    {
        return itemMissao;
    }
    public void SetItemDaMissao(item obj)
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
        StartCoroutine(this.Movimentacao());
    }
    private IEnumerator Movimentacao()
    {
        if (direcaoParaMoverX != 0 || direcaoParaMoverY != 0)
        {
            animator.SetFloat("HORZ", direcaoParaMoverX);
            animator.SetFloat("VERTC", direcaoParaMoverY);
            while (Mathf.Sqrt(Mathf.Pow(transform.position.x - posInicial.x, 2)) < distanciaMaxParaPerocrer && Mathf.Sqrt(Mathf.Pow(transform.position.y - posInicial.y, 2)) < distanciaMaxParaPerocrer)
            {
                rb.velocity = new Vector2(direcaoParaMoverX, direcaoParaMoverY) * velocidade;
                yield return new WaitForSeconds(.2f);
            }
            rb.velocity = Vector2.zero;
        }
    }
}
