using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoUnico : MonoBehaviour, SalvamentoEntreCenas
{
    public Dialogo dialogo;
    private bool dialogoRealizado = false;
    public void AtivarDialogo()
    {
        if (!dialogoRealizado)
        {
            DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
            DialogeManager.Instance.IniciarDialogo(dialogo);
        }
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().AtivarCarregamentoDoObjeto();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosDialogosUnicos(this, 0);
        }
    }
    public void CarregarDados()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDosDialogosUnicos(this, 1);
        }
    }
    public bool GetDialogoORealizado()
    {
        return dialogoRealizado;
    }
    public void SetDialogoORealizado(bool b)
    {
        dialogoRealizado = b;
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        dialogoRealizado = true;
        SalvarEstado();
    }
}
