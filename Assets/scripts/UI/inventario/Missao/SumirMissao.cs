using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumirMissao : MonoBehaviour
{
    [SerializeField] private missaoPrefabScript missaoPrefabScript;
    public void IniciarAnimDesaparecer()
    {
        missaoPrefabScript.animDetalheMissao.enabled = true;
        missaoPrefabScript.animResumoMissao.enabled = true;
        missaoPrefabScript.animCaixaMissao.SetTrigger("SUMIR");
    }
}
