using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecionadorDeIconeDeInteracao : MonoBehaviour
{
    //public static SelecionadorDeIconeDeInteracao Instance { get; private set; }
    [SerializeField] private List<int> listaDosIconesDeInteracao = new List<int>();

    //private void Awake()
    //{
    //    Instance = this;
    //}
    public int SelecionaIconeDeInteracao(KeyCode tecla)
    {
        int sprite = 0;
        switch (tecla)
        {
            case (KeyCode.E):
                sprite = listaDosIconesDeInteracao[0];
                break;
            case (KeyCode.T):
                sprite = listaDosIconesDeInteracao[1];
                break;
        }
        return sprite;
    }
}
