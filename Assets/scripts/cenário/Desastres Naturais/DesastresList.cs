using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesastresList : MonoBehaviour
{
    public static DesastresList Instance { get; private set; }
    public List<string> desastreNome = new List<string>();
    public List<bool> ativar = new List<bool>();
    public List<Sprite> iconesModulos = new List<Sprite>();
    public List<Sprite> iconesMultiplicador = new List<Sprite>();
    public List<Sprite> iconesDesastre = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
    }
    public Sprite SelecionaSpriteDesastre(string desastre)
    {
        for (int i = 0; i < iconesDesastre.Count; i++)
        {
            if (desastreNome[i].ToUpper() == desastre.ToUpper())
                return iconesDesastre[i];
        }
        return null;
    }
    public Sprite SelecionaSpriteMultiplicador(int forca)
    {
        return iconesMultiplicador[forca - 1];
    }
    public Sprite SelecionaSpriteModulo(int tipo)
    {
        return iconesModulos[tipo - 1];
    }
    public void LiberarNovosDesastres(int i)
    {
        ativar[i] = true;
    }
}
