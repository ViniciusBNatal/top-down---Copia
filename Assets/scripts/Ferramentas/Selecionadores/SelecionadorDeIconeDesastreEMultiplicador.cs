using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecionadorDeIconeDesastreEMultiplicador : MonoBehaviour
{
    public static SelecionadorDeIconeDesastreEMultiplicador Instance { get; private set; }
    public List<Sprite> iconesMultiplicador = new List<Sprite>();
    public List<Sprite> iconesDesastre = new List<Sprite>();

    private void Awake()
    {
        Instance = this;
    }
    public Sprite SelecionarSpriteMultiplicador(int resistencia)
    {
        return iconesMultiplicador[resistencia - 1];
    }
    public Sprite SelecionarSpriteDesastre(int desastre)
    {
        return iconesDesastre[desastre - 1];
    }
}
