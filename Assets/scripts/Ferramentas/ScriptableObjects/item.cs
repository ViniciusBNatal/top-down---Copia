using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Itens", menuName = "Itens/Item Coletável")]
public class Item : ScriptableObject
{
    public Sprite icone;
    public string ID;
}
