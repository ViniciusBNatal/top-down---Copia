using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceMenu : MonoBehaviour
{
    const int fase = 1;
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    private void Update()
    {
        
    }
    public void AbrirCena()
    {
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(fase);//pega o caminho da cena na pasta de arquivos
        string cenaParaAbrir = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        SceneManager.LoadScene(cenaParaAbrir);
    }
    public void FecharJogo()
    {
        Application.Quit();
    }
    public void MenuOpcoes()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].name.ToUpper().Contains("OPCOES"))
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    public void VoltarMenu(string MenuParaRetornar)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].name.ToUpper().Contains(MenuParaRetornar.ToUpper()))
            {
                menus[i].SetActive(true);
            }
            else
            {
                menus[i].SetActive(false);
            }
        }
    }
    //public void InputDeRetorno()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        for (int i = 0; i < menus.Count; i++)
    //        {
    //            if (menus[i].activeInHierarchy == true)
    //            {
    //                menus[i].SetActive(true);
    //            }
    //            else
    //            {
    //                menus[i].SetActive(false);
    //            }
    //        }
    //    }
    //}
}
