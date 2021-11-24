using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogo
{
    public string[] NomeNPC;
    //public int[] IDdoNPC;
    public Sprite[] ImagemNPC;
    [TextArea(1, 4)]
    public string[] Frases;
    public Transform[] FocarComCamera;
    public UnityEvent[] EventosDuranteDialogo;
}
