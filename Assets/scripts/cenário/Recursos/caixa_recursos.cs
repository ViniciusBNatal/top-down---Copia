using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caixa_recursos : MonoBehaviour, CentroDeRecurso
{
    [Header("Componentes do recurso dropado")]
    [SerializeField] private List<Item> itens = new List<Item>();
    [SerializeField] private List<int> qntdDoRecursoDropado = new List<int>();
    [SerializeField] private float forca;
    [Header ("Não Mexer")]
    [SerializeField] private GameObject recursoColetavelPreFab;

    public void DropaRecursos()
    {
        for (int i = 0; i < itens.Count; i++)
        {
            float localDeDropX = Random.Range(-1, 1);
            if (localDeDropX == 0)
                localDeDropX = 1;
            float localDeDropY = Random.Range(-1, 1);
            if (localDeDropY == 0)
                localDeDropY = 1;
            //GameObject recurso = Instantiate(recursoColetavelPreFab, new Vector3(transform.position.x + localDeDropX, transform.position.y + localDeDropY, 0), Quaternion.identity);
            GameObject recurso = Instantiate(recursoColetavelPreFab, transform);
            recurso.transform.SetParent(null);//desasosia recursos da caixa
            recurso.GetComponent<recurso_coletavel>().item = itens[i];
            recurso.GetComponent<recurso_coletavel>().qntd = qntdDoRecursoDropado[i];
            recurso.GetComponent<Rigidbody2D>().AddForce(new Vector2(localDeDropX, localDeDropY).normalized * forca);
        }
        Destroy(this.gameObject);
    }
}
