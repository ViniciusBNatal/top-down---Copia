using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitbox_inimigo : MonoBehaviour
{
    public inimigoScript inimigo;
    private float forcaRepulsao;
    private float duracaoStun;
    private void Start()
    {
        forcaRepulsao = inimigo.forcaRepulsao; // * inimigo.GetComponent<inimigo>().raioVisao;
        duracaoStun = inimigo.tempoDeStunNoJogador;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inimigo.GetAnimScript().AtaqueMelee();
            jogadorScript.Instance.mudancaRelogio(inimigo.danoMelee);
            jogadorScript.Instance.Knockback(duracaoStun, forcaRepulsao, this.transform);
        }
    }
}
