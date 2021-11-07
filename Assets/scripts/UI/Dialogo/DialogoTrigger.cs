using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoTrigger : MonoBehaviour
{
    public Dialogo dialogo;

    public void AtivarDialogo()
    {
        //UIinventario.Instance.AtivaEDesativaCaixaDeDialogo(true);
        if (dialogo.animacoesDasImagens != null)
            DialogeManager.Instance.TrocarAnimator(dialogo.animacoesDasImagens);
        DialogeManager.Instance.IniciarDialogo(dialogo);
    }
}
