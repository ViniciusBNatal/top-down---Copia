using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox_inimigo : MonoBehaviour
{
    public GameObject inimigo;
    private float forcaRepulsao;
    private float duracaoStun;
    private void Start()
    {
        forcaRepulsao = inimigo.GetComponent<inimigo>().forcaRepulsao; // * inimigo.GetComponent<inimigo>().raioVisao;
        duracaoStun = inimigo.GetComponent<inimigo>().tempoDeStunNoJogador;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jogadorScript.Instance.mudancaRelogio(-inimigo.GetComponent<inimigo>().danoMelee);
            StartCoroutine(jogadorScript.Instance.Knockback(duracaoStun, forcaRepulsao, this.transform));
        }
    }
}
