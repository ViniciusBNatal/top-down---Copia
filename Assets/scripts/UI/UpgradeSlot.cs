using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public ReceitaDeCrafting receita;
    [SerializeField] private GameObject IconeETextoDorecursoNecessarioPrefab;
    [SerializeField] private GameObject recursosGrid;
    public GameObject BtnConstruirUpgrade;
    public GameObject BtnTrocartempo;
    private int divisor = 3;
    private void Start()
    {
        for(int i = 0;i < receita.itensNecessarios.Count; i++)//adiciona a quantidade e a imagem para cada recurso na receita
        {
            GameObject obj = Instantiate(IconeETextoDorecursoNecessarioPrefab, recursosGrid.transform);
            float largura = obj.GetComponent<RectTransform>().rect.width;
            float altura = obj.GetComponent<RectTransform>().rect.height;
            obj.transform.localPosition = new Vector3((i % divisor) * largura, -(i / divisor) * altura, 0);
            obj.GetComponentInChildren<Text>().text = receita.quantidadeDosRecursos[i].ToString("000");
            obj.GetComponentInChildren<Image>().sprite = receita.itensNecessarios[i].icone;
        }
    }

}
