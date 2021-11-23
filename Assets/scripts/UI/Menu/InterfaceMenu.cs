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
    [HideInInspector] public bool pausado = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
    }
    private string CenaPorBuildIndex(int buildIndex)
    {
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(buildIndex);//pega o caminho da cena na pasta de arquivos
        string cenaParaAbrir = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        return cenaParaAbrir;
    }
    public void AbrirCena(int buildIndex)
    {
        SceneManager.LoadScene(CenaPorBuildIndex(buildIndex));
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
        AbrirCena(0);
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
    public void AbrirGameOver()
    {
        jogadorScript.Instance.MudarEstadoJogador(0);
        //AbrirMenu();
    }
    public void AbrirVitoria()
    {
        jogadorScript.Instance.MudarEstadoJogador(5);
        //craftingBossFinal.SetActive(false);
        //AbrirMenu();
    }
    private void ApagarDadosDoJogo()
    {
        if (TutorialSetUp.Instance == null)
        {
            GetComponent<SalvarEstadoDoObjeto>().LimparDados();
            GetComponent<SalvamentoDosCentrosDeRecursosManager>().LimparDados();
        }
    }
}
