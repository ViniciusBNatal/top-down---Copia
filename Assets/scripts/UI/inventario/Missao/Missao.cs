using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Missao", menuName = "Missao")]
public class Missao : ScriptableObject
{
    //public IdentificacaoMissao IDmissao;
    [TextArea(1, 2)]
    public string TextoResumoMissao;
    [TextArea(1, 5)]
    public string TextoDetalheMissao;
    public Sprite iconeMissao;
}
