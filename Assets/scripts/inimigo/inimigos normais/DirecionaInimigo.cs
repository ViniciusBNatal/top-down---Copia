using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirecionaInimigo : MonoBehaviour
{
    [SerializeField] private List<Vector2> NovaDirecao = new List<Vector2>();
    private int direcaoIndex;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "inimigo" && collision.GetComponent<hitbox_inimigo>().inimigo.GetComponent<inimigoScript>().GetMovimentacaoFixa())
        {
            //direcaoIndex = Random.Range(0, NovaDirecao.Count + 1);
            float dirX = NovaDirecao[direcaoIndex].x / Mathf.Abs(NovaDirecao[direcaoIndex].x);
            float dirY = NovaDirecao[direcaoIndex].y / Mathf.Abs(NovaDirecao[direcaoIndex].y);
            if (NovaDirecao[direcaoIndex].y == 0)
                dirY = 0;
            if (NovaDirecao[direcaoIndex].x == 0)
                dirX = 0;
            collision.GetComponent<hitbox_inimigo>().inimigo.GetComponent<inimigoScript>().direcaoDeMovimentacao = new Vector2(dirX, dirY);
            direcaoIndex++;
            if (direcaoIndex >= NovaDirecao.Count)
                direcaoIndex = 0;
        }
    }
}
