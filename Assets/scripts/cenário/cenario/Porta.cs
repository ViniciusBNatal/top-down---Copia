using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porta : MonoBehaviour
{
    private int botoesPrecionados = 0;
    [SerializeField] private int nDeBotoesNecessarios;
    public void AbrirPorta(int valorBotao)
    {
        botoesPrecionados += valorBotao;
        if (botoesPrecionados >= nDeBotoesNecessarios)
        {
            Destroy(this.gameObject);
        }
    }
}
