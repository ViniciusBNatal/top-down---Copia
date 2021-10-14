using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class barraDeVida : MonoBehaviour
{
    public Image barra;

    public void AtualizaBarraDeVida(float valor)
    {
        barra.fillAmount = valor;
    }
}

