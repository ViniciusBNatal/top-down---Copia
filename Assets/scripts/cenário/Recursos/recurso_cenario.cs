using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso_cenario : MonoBehaviour, CentroDeRecurso
{
    [Header("Componentes do recurso dropado")]
    [SerializeField] private Item item;
    [SerializeField] private GameObject recursoColetavelPreFab;
    [Header("Valores numéricos")]
    [SerializeField] private int qntdDoRecursoDropado = 1;
    [SerializeField] private float forca;
    public void DropaRecursos()
    {
        float localDeDropX = Random.Range(-1f, 1f);
        if (localDeDropX == 0)
            localDeDropX = 1;
        float localDeDropY = Random.Range(-1f, 1f);
        if (localDeDropY == 0)
            localDeDropY = 1;
        //GameObject recurso = Instantiate(recursoColetavelPreFab, new Vector3(transform.position.x + localDeDropX, transform.position.y + localDeDropY, 0), Quaternion.identity);
        GameObject recurso = Instantiate(recursoColetavelPreFab, transform.position, Quaternion.identity);
        recurso.GetComponent<recurso_coletavel>().item = item;
        recurso.GetComponent<recurso_coletavel>().qntd = qntdDoRecursoDropado;
        recurso.GetComponent<Rigidbody2D>().AddForce(new Vector2(localDeDropX, localDeDropY).normalized * forca);
    }
}
