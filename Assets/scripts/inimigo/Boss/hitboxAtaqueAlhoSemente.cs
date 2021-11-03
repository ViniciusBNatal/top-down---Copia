using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxAtaqueAlhoSemente : MonoBehaviour
{
    [SerializeField] private AtaqueSementeAlho ataqueRef;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jogadorScript.Instance.mudancaRelogio(ataqueRef.GetDano());
            jogadorScript.Instance.Knockback(ataqueRef.GetDuracaoStun(), ataqueRef.GetForcaDeRepulsao(), this.transform);
        }
    }
}
