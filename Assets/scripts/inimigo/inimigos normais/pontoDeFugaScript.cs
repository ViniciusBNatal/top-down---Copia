using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pontoDeFugaScript : MonoBehaviour
{
    [HideInInspector] public List<inimigoScript> inimigosRelacionadosPorSpawn = new List<inimigoScript>();
    [SerializeField] private List<inimigoScript> inimigosFixos = new List<inimigoScript>();
    public Transform pontoDeTeleporte;
    // Start is called before the first frame update
    /*void Start()
    {
        Debug.Log(inimigosRelacionados.Count);
        //if (inimigosRelacionados.tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
        //    inimigosRelacionados.AdicionarPontoDeFuga(this.gameObject);
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (inimigosRelacionadosPorSpawn.Count > 0)
            {
                foreach(inimigoScript inimigo in inimigosRelacionadosPorSpawn)
                {
                    if (inimigo.tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
                        inimigo.Fuga(pontoDeTeleporte);
                }
            }
            if (inimigosFixos.Count > 0)
            {
                foreach (inimigoScript inimigo in inimigosFixos)
                {
                    if (inimigo.tiposDeMovimentacao == inimigoScript.TiposDeMovimentacao.movimentacaoEntrePontosFixa)
                        inimigo.Fuga(pontoDeTeleporte);
                }
            }
        }
    }
}
