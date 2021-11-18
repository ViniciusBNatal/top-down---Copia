using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecionadorDeIconeDeInteracao : MonoBehaviour
{
    //public static SelecionadorDeIconeDeInteracao Instance { get; private set; }
    [SerializeField] private List<Sprite> listaDosIconesDeInteracao = new List<Sprite>();

    //private void Awake()
    //{
    //    Instance = this;
    //}
    public Sprite SelecionaIconeDeInteracao(KeyCode tecla)
    {
        Sprite sprite = null;
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
