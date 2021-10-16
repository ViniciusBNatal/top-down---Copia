using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manterEntreCenas : MonoBehaviour
{
    public static List<GameObject> objetosSalvos = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        bool jaNaLista = false;
        for (int i = 0; i < objetosSalvos.Count; i++)
        {
            if (objetosSalvos[i] == gameObject)
            {
                jaNaLista = true;
                break;
            }
        }
        if (!jaNaLista)
            objetosSalvos.Add(gameObject);
        for (int i = 0; i < objetosSalvos.Count; i++)
        {
            DontDestroyOnLoad(objetosSalvos[i]);
        }
    }
}
