using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogo
{
    public string NomeNPC;
    [TextArea(1, 4)]
    public string[] Frases;
    public float[] EstadoImagemNPC;
    public Animator animacoesDasImagens;
    public Transform[] FocarComCamera;
}
