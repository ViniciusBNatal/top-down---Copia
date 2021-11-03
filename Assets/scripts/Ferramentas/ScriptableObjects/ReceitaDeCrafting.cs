using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Receita de Crafting", menuName = "Itens/Receita de Crafting")]
public class ReceitaDeCrafting : ScriptableObject
{
    [Header("Componentes Do Slot De Melhorar a Base")]
    public Sprite iconeDeAprimoramentoDabase;
    [Header("Componentes Do Slot De Criar Modulos")]
    public int modulo;
    public string desastre;
    public int incrementoEntreForcas;
    private int forca;
    [Header("Receita")]
    public List<Item> itensNecessarios = new List<Item>();
    public List<int> quantidadeDosRecursos = new List<int>();

    public void SetForca(int f)
    {
        forca = f;
    }
    public int GetForca()
    {
        return forca;
    }
}
