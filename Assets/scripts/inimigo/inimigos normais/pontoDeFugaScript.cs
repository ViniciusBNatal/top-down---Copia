using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pontoDeFugaScript : MonoBehaviour
{
    [SerializeField] private inimigoScript inimigoRelacionado;

    // Start is called before the first frame update
    void Start()
    {
        if (inimigoRelacionado.tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
            inimigoRelacionado.AdicionarPontoDeFuga(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && inimigoRelacionado.tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
        {
            inimigoRelacionado.Fuga(this.gameObject);
        }
    }
}
