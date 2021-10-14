using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Item item;
    private Sprite icone;
    public Text qntdItemText;
    public int qntdRecurso;

    public void atualizaQuantidade(int quantidade)
    {
        qntdRecurso += quantidade;
        qntdItemText.text = qntdRecurso.ToString("000");
    }
    public void destroiSlot()
    {
        Destroy(gameObject);
    }
}