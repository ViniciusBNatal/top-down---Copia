using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasSementeAlho : MonoBehaviour
{
    [SerializeField] AtaqueSementeAlho atqRef;
    private bool gasIniciado = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !gasIniciado)
        {
            gasIniciado = true;
            StartCoroutine(this.DanoGas());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gasIniciado = false;
        }
    }
    IEnumerator DanoGas()
    {
        while (gasIniciado)
        {
            jogadorScript.Instance.mudancaRelogio(atqRef.GetDanoAreaToxica(), .25f);
            yield return new WaitForSeconds(atqRef.GetIntervaloHitsAraToxica());
        }
    }
}
