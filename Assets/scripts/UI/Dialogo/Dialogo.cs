using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

[System.Serializable]
public class Dialogo
{
    public string NomeNPC;
    public int IDdoNPC;
    [TextArea(1, 4)]
    public string[] Frases;
    public float[] EstadoImagemNPC;
    public Transform[] FocarComCamera;
}
