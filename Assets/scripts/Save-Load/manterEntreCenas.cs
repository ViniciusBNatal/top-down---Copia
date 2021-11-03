using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class manterEntreCenas : MonoBehaviour
{
    //public static manterEntreCenas Instance;
    //private static List<string> RefDosObjetosPorNome = new List<string>();
    //private static int qntdDeObjsATransitar = 6;
    //private void Awake()
    //{
    //    Instance = this;
    //}
    private void Start()
    {
        //AdicionarALista(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    //public void AdicionarALista(GameObject gobj)
    //{
    //    int naoAchado = 0;
    //    for (int i = 0; i < RefDosObjetosPorNome.Count; i++)
    //    {
    //        if (RefDosObjetosPorNome[i] == gobj.name)
    //        {
    //            DontDestroyOnLoad(gobj);
    //            return;
    //        }
    //        else
    //            naoAchado++;
    //    }
    //    if (naoAchado == qntdDeObjsATransitar)
    //    {
    //        Destroy(gobj);
    //        return;
    //    }
    //    gobj.name = gobj.name + "0";
    //    RefDosObjetosPorNome.Add(gobj.name);
    //    DontDestroyOnLoad(gobj);
    //}
}
