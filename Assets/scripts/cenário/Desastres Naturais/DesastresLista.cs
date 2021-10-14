using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Desastre Natural Lista", menuName = "Desastre/Desastre Natural Lista")]
public class DesastresList : ScriptableObject
{
    public List<string> desastreNome = new List<string>();
}
