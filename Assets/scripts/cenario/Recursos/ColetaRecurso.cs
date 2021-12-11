using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColetaRecurso : MonoBehaviour
{
    [SerializeField] private recurso_coletavel itemColetavel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            itemColetavel.ColetaItem();
        }
    }
}
