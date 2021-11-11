using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualPredio : MonoBehaviour
{
    [SerializeField] private LayerMask cenario;
    [SerializeField] List<LayerMask> LayerPredios;
    [SerializeField] int predio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < LayerPredios.Count; i++)
            {
                if (i != predio - 1)
                    collision.GetComponent<jogadorScript>().mainCamera.cullingMask -= LayerPredios[i];
            }
            collision.GetComponent<jogadorScript>().mainCamera.cullingMask -= cenario;
            //PostProcessScript.Instance.visualPredio(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            for (int i = 0; i < LayerPredios.Count; i++)
            {
                if (i != predio - 1)
                    collision.GetComponent<jogadorScript>().mainCamera.cullingMask += LayerPredios[i];
            }
            collision.GetComponent<jogadorScript>().mainCamera.cullingMask += cenario;
            //PostProcessScript.Instance.visualPredio(false);
        }
    }
}
