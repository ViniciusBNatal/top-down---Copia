using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour, SalvamentoEntreCenas
{
    //private int botoesPrecionados = 0;
    private BoxCollider2D colisao;
    private bool iniciouCorrotina = false;
    [SerializeField] private bool aberta;
    [SerializeField] private float tempoParaAbrir;
    [SerializeField] private float tempoParaFechar;
    [SerializeField] private GameObject chaveNecessaria;
    private Item chave = null;
    //[SerializeField] private int nDeBotoesNecessarios;

    private void Start()
    {
        colisao = GetComponent<BoxCollider2D>();
        if (chaveNecessaria != null)
            chave = chaveNecessaria.GetComponent<recurso_coletavel>().ReferenciaItem();
        //if (aberta)
        //    AbrePorta();
        //else
        //    FechaPorta();
    }
    //public void VerificarParaAbrirPorta(int valorBotao, float tempoAbrirPorta, float tempoFecharPorta)
    //{
    //    botoesPrecionados += valorBotao;
    //    if (botoesPrecionados >= nDeBotoesNecessarios)
    //    {
    //        StartCoroutine(this.TempoPorta(tempoAbrirPorta, tempoFecharPorta));
    //    }
    //}
    public void TentarAbrirPorta()
    {
        if (!aberta)
        {
            if (UIinventario.Instance.ProcurarChave(chave))
            {
                StartCoroutine(this.TempoPorta());
            }            
        }
        else
        {
            StartCoroutine(this.TempoPorta());
        }
    }
    private IEnumerator TempoPorta()
    {
        if (!iniciouCorrotina)
        {
            iniciouCorrotina = true;
            if (aberta)
            {
                yield return new WaitForSeconds(tempoParaFechar);
                FechaPorta();
                iniciouCorrotina = false;
            }
            else
            {
                yield return new WaitForSeconds(tempoParaAbrir);
                AbrePorta();
                iniciouCorrotina = false;
            }
            SalvarEstado();
        }
    }
    private void AbrePorta()
    {
        colisao.enabled = false;
        transform.Rotate(new Vector3(0f, 0f, 90f));
        chave = null;
        aberta = true;
    }
    private void FechaPorta()
    {
        colisao.enabled = true;
        if (transform.rotation.z != 0f)
            transform.Rotate(new Vector3(0f, 0f,-90f));
        aberta = false;
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDasPortas(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDasPortas(this, 1);
        colisao = GetComponent<BoxCollider2D>();
        if (aberta)
        {
            chaveNecessaria = null;
            chave = null;
            AbrePorta();
        }
        else
            FechaPorta();
        //AbrePorta();
    }
    public bool GetAberto_Fechado()
    {
        return aberta;
    }
    public void SetAberto_Fechado(bool b)
    {
        aberta = b;
    }
}
