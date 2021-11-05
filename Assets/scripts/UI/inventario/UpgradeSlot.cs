using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Confugurações do Slot de Upgrade")]
    [SerializeField] private ReceitaDeCrafting receita;
    [SerializeField] private int IndexFaseNaBuild;
    //public int fase;
    [Header("Não Mexer")]
    [SerializeField] private GameObject IconeETextoDorecursoNecessarioPrefab;
    [SerializeField] private GameObject recursosGrid;
    [SerializeField] private GameObject BtnConstruirUpgrade;
    [SerializeField] private GameObject BtnTrocartempo;
    private int divisor = 3;
    private void Start()
    {
        if (receita != null)
        {
            for(int i = 0;i < receita.itensNecessarios.Count; i++)//adiciona a quantidade e a imagem para cada recurso na receita
            {
                GameObject obj = Instantiate(IconeETextoDorecursoNecessarioPrefab, recursosGrid.transform);
                float largura = obj.GetComponent<RectTransform>().rect.width;
                float altura = obj.GetComponent<RectTransform>().rect.height;
                obj.transform.localPosition = new Vector3((i % divisor) * largura, -(i / divisor) * altura, 0);
                obj.GetComponentInChildren<TMP_Text>().text = receita.quantidadeDosRecursos[i].ToString("000");
                obj.GetComponentInChildren<Image>().sprite = receita.itensNecessarios[i].icone;
            }
        }
    }
    public void LiberarViagemNoTempo()
    {
        BtnConstruirUpgrade.GetComponent<Image>().sprite = receita.iconeDeAprimoramentoDabase; // coloca a nova imagem no botão
        BtnConstruirUpgrade.GetComponent<Button>().enabled = false; // desliga a oção de pressionar o botão de criar o upgrade
        BtnTrocartempo.SetActive(true);
        Destroy(recursosGrid.gameObject);
    }
    public string FaseParaAbrir()
    {
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(IndexFaseNaBuild);//pega o caminho da cena na pasta de arquivos
        string cenaParaAbrir = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        return cenaParaAbrir;
    }
    public ReceitaDeCrafting GetReceita()
    {
        return receita;
    }
}
