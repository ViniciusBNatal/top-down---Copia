using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Receita de Crafting", menuName = "Itens/Receita de Crafting")]
public class itemCrafting : ScriptableObject
{
    [Header("Componentes Do Slot")]
    public Sprite iconeDeCrafting;
    public int resistencia;
    public int desastre;
    //var que tem uma lista contendo os tipos de desastres, dropbox

    [Header("Itens Da Receita")]
    public List<Item> itensNecessarios = new List<Item>(); 
    public List<int> quantidadeDoRecurso = new List<int>();
}
