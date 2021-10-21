using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class craftingSlot : MonoBehaviour
{
    public ReceitaDeCrafting receita;
    [SerializeField] private GameObject recursoNecessarioUIPrefab;
    [SerializeField] private GameObject recursosGrid;
    [SerializeField] private Image iconeDoDesastre;
    private List<Text> qntdNecessariaParaCadarecursoText = new List<Text>();
    private List<int> novosValores = new List<int>();
    private int divisor = 3;
    private int forca = 1;
    private void Start()
    {
        iconeDoDesastre.sprite = DesastresList.Instance.SelecionaSpriteDesastre(receita.desastre);
        for(int i = 0;i < receita.itensNecessarios.Count; i++)//adiciona a quantidade e a imagem para cada recurso na receita
        {
            GameObject obj = Instantiate(recursoNecessarioUIPrefab, recursosGrid.transform);
            float largura = obj.GetComponent<RectTransform>().rect.width;
            float altura = obj.GetComponent<RectTransform>().rect.height;
            obj.transform.localPosition = new Vector3((i % divisor) * largura, -(i / divisor) * altura, 0);
            obj.GetComponentInChildren<Text>().text = receita.quantidadeDosRecursos[i].ToString("000");
            novosValores.Add(receita.quantidadeDosRecursos[i]);
            qntdNecessariaParaCadarecursoText.Add(obj.GetComponentInChildren<Text>());
            obj.GetComponentInChildren<Image>().sprite = receita.itensNecessarios[i].icone;
        }
    }
    public void TrocaForcamodulo(int f)
    {
        forca = f;
        switch (f)
        {
            case 1:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = receita.quantidadeDosRecursos[i].ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i];
                }
                break;
            case 2:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = (receita.quantidadeDosRecursos[i] + receita.incrementoEntreForcas).ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i] + receita.incrementoEntreForcas;
                }
                break;
            case 3:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = (receita.quantidadeDosRecursos[i] + 2 * receita.incrementoEntreForcas).ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i] + 2 * receita.incrementoEntreForcas;
                }
                break;
        }
    }
    public ReceitaDeCrafting construcaoConfirmada()
    {
        ReceitaDeCrafting moduloConfig = ScriptableObject.CreateInstance<ReceitaDeCrafting>();
        moduloConfig.desastre = receita.desastre.ToUpper();
        moduloConfig.SetForca(forca);
        for (int i = 0; i < receita.quantidadeDosRecursos.Count; i++)
        {
            moduloConfig.quantidadeDosRecursos.Add(novosValores[i]);
        }
        return moduloConfig;
    }
    public int GetForca()
    {
        return forca;
    }
    //public List<int> GetNovaListaDeQntdRecursosNecessarios()
    //{
    //    return novosValores;
    //}
}
