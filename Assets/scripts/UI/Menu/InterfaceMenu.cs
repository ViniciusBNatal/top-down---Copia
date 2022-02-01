using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceMenu : MonoBehaviour
{
    public static InterfaceMenu Instance { get; private set; }
    const int fase = 1;
    [SerializeField] private GameObject abaMenus;
    [SerializeField] private List<GameObject> menus = new List<GameObject>();
    [SerializeField] private SalvarEstadoDoObjeto salvarObjetosScript;
    [SerializeField] private SalvamentoDosCentrosDeRecursosManager salvarTempoDeSaidasScript;
    [HideInInspector] public bool pausado = false;
    [HideInInspector] public bool podePausar = true;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        //if (!UIinventario.Instance.InventarioAberto)
        //{

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (podePausar)
                {
                    if (jogadorScript.Instance != null)
                    {
                        if (!pausado)
                        {
                            Ativa_DesativaPausaJogo(true);
                            AbrirMenu(menus[0]);
                        }
                        else
                        {
                            Ativa_DesativaPausaJogo(false);
                        }
                    }
                    else
                    {
                        AbrirMenu(menus[0]);
                    }
                }
                else
                    podePausar = true;
            }
        //}
    }
    public void AbrirCena(string cena)
    {
        SceneManager.LoadScene(cena);
    }
    public void FecharJogo()
    {
        Application.Quit();
        Debug.Log("sair jogo");
    }
    public void Ativa_DesativaPausaJogo(bool b)
    {
        if (b)
        {
            abaMenus.SetActive(true);
            pausado = true;
            Time.timeScale = 0f;
        }
        else
        {
            abaMenus.SetActive(false);
            pausado = false;
            Time.timeScale = 1f;
        }
    }
    public void VoltarParaMenuPrincipal()
    {
        Time.timeScale = 1f;
        ApagarDadosDoJogo();
        AbrirCena("Menu");
    }
    public void AbrirMenu(GameObject MenuParaRetornar)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].activeInHierarchy)
            {
                menus[i].SetActive(false);
                break;
            }
        }
        MenuParaRetornar.SetActive(true);
    }
    public void AbrirGameOver()//sempre deve ser o penultimo menu na lista
    {
        jogadorScript.Instance.MudarEstadoJogador(0);
        abaMenus.SetActive(true);
        AbrirMenu(menus[menus.Count - 2]);
        SoundManager.Instance.PararEfeitosSonoros();
    }
    public void AbrirVitoria()//sempre deve ser o ultimo menu na lista
    {
        jogadorScript.Instance.MudarEstadoJogador(5);
        UIinventario.Instance.craftingBossFinal.SetActive(false);
        abaMenus.SetActive(true);
        AbrirMenu(menus[menus.Count - 1]);
    }
    private void ApagarDadosDoJogo()
    {
        if (TutorialSetUp.Instance == null)
        {
            salvarObjetosScript.LimparDados();
            salvarTempoDeSaidasScript.LimparDados();
        }
    }
}
