using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caixa_recursos : MonoBehaviour, CentroDeRecurso, SalvamentoEntreCenas
{
    [Header("Componentes do recurso dropado")]
    [SerializeField] private float forca;
    [SerializeField] private List<Item> itens = new List<Item>();
    [SerializeField] private List<int> qntdDoRecursoDropado = new List<int>();
    [Header ("Não Mexer")]
    [SerializeField] private GameObject recursoColetavelPreFab;
    public void RecebeuHit()
    {
        QuebrarCaixa();
        SalvarEstado();
        Destroy(this.gameObject);
    }
    public void QuebrarCaixa()
    {
        for (int i = 0; i < itens.Count; i++)
        {
            CriaRecurso(i);
        }
    }
    public void CriaRecurso(int i)
    {
        GameObject recurso = Instantiate(recursoColetavelPreFab, transform);
        recurso.transform.SetParent(null);//desasosia recursos da caixa
        recurso.GetComponent<recurso_coletavel>().DefineItem(itens[i]);
        recurso.GetComponent<recurso_coletavel>().DefineQuantidadeItem(qntdDoRecursoDropado[i]);
        recurso.GetComponent<recurso_coletavel>().LancaRecurso(forca);
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
    }
    public void AcaoSeEstadoJaModificado()
    {
        Destroy(this.gameObject);
    }
}
