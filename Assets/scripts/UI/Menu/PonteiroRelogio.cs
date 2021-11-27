using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonteiroRelogio : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 PosicaoNoMundo = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 relativo = transform.position - PosicaoNoMundo;
        float angulo = Mathf.Atan2(relativo.y, relativo.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angulo+90);
    }
}
