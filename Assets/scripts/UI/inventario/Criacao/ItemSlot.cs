using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    public Item item;
    [SerializeField] private TMP_Text qntdItemText;
    //[SerializeField] private Text qntdItemText;
    public int qntdRecurso;

    private void Start()
    {
        GetComponent<Image>().sprite = item.icone;
    }
    public void atualizaQuantidade(int quantidade)
    {
        qntdRecurso += quantidade;
        qntdItemText.text = qntdRecurso.ToString("000");
    }
    public int GetQntdRecurso()
    {
        return qntdRecurso;
    }
}