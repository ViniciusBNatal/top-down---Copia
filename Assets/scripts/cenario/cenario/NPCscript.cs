﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCscript : MonoBehaviour, SalvamentoEntreCenas
{
    [SerializeField] private List<DialogoTrigger> dialogos = new List<DialogoTrigger>();
    [SerializeField] private GameObject objetoDeMissao;
    private Item itemMissao = null;
    private bool missaoCumprida = false;
    private void Start()
    {
        if (objetoDeMissao != null && itemMissao == null)
        {
            objetoDeMissao.GetComponent<recurso_coletavel>().SetNPC(this.gameObject);
            itemMissao = objetoDeMissao.GetComponent<recurso_coletavel>().ReferenciaItem();
        }
    }
    public void Interacao()
    {
        if (itemMissao != null)
        {
            VerificarMissao();
        }
        else//se ele for apenas para conversar
        {
            dialogos[0].AtivarDialogo();
        }
    }
    private void VerificarMissao()
    {
        if (missaoCumprida)
        {
            dialogos[2].AtivarDialogo();//dialogo de missao ja foi cumprida
        }
        else
        {
            if (UIinventario.Instance.ProcurarChave(itemMissao))
            {
                missaoCumprida = true;
                DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
                dialogos[1].AtivarDialogo();//dialogo de missao cumprida
            }
            else
            {
                dialogos[0].AtivarDialogo();//dialogo de falar qual a missao
            }
        }
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        AoCompletarAMissao();
        DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
        SalvarEstado();
    }
    private void AoCompletarAMissao()
    {
        Debug.Log("missao Cumprida");
        transform.position += new Vector3(10f, 10f, 0f);
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
}