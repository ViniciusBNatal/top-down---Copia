using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndicadorDosDesastres : MonoBehaviour
{
    public static IndicadorDosDesastres Instance { get; private set; }
    private List<GameObject> iconesDesenhados = new List<GameObject>();
    //private List<Image> iconesDesenhados = new List<Image>();
    //private List<Image> multiplicadoresDesenhados = new List<Image>();
    [SerializeField] private GameObject iconesDesastrPrefab;
    [SerializeField] private Transform PosIcones;
    //[SerializeField] private Image iconesDesastrPrefab;
    //[SerializeField] private GameObject PosicaoIconesDesastre;
    //[SerializeField] private GameObject PosicaoIconesMultiplicador;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        PreenchePlaca();
    }
    public void LimpaPlaca()
    {
        if (iconesDesenhados.Count != 0)
        {
            for (int i = iconesDesenhados.Count; i > 0; i--)
            {
                Destroy(iconesDesenhados[i - 1].gameObject);
                iconesDesenhados.RemoveAt(i - 1);
            }
        }
        //if (iconesDesenhados != null)
        //{
        //    for (int i = iconesDesenhados.Count; i > 0; i--)
        //    {
        //        Destroy(iconesDesenhados[i - 1].gameObject);
        //        iconesDesenhados.RemoveAt(i - 1);
        //    }
        //}
        //if (multiplicadoresDesenhados != null)
        //{
        //    for (int i = multiplicadoresDesenhados.Count; i > 0; i--)
        //    {
        //        Destroy(multiplicadoresDesenhados[i - 1].gameObject);
        //        multiplicadoresDesenhados.RemoveAt(i - 1);
        //    }
        //}
    }
    public void PreenchePlaca()
    {
        //limpa os icones para os próximos desastres
        LimpaPlaca();
        for (int i = 0; i < desastreManager.Instance.GetQntdDesastresParaOcorrer(); i++)//preenche a tela de acordo com o que foi sorteado e a quantidade de desastres
        {
            GameObject icone = Instantiate(iconesDesastrPrefab, PosIcones.transform);
            icone.GetComponent<Image>().sprite = DesastresList.Instance.SelecionaSpriteDesastre(desastreManager.Instance.GetDesastreSorteado(i));
            icone.GetComponentInChildren<TMP_Text>().text = desastreManager.Instance.forcasSorteados[i].ToString();
            iconesDesenhados.Add(icone);
            ////icone do multiplicador
            //Image iconeDeMultiplicador = Instantiate(iconesDesastrPrefab, PosicaoIconesMultiplicador.transform.position, Quaternion.identity, PosicaoIconesMultiplicador.transform);
            //Sprite iconeMult = DesastresList.Instance.SelecionaSpriteMultiplicador(desastreManager.Instance.GetForcaSorteada(i));
            ////SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteMultiplicador(forcasSorteados[i]);
            //float alturaiconeMultiplicador = iconeDeMultiplicador.rectTransform.rect.height;
            //iconeDeMultiplicador.transform.localPosition = new Vector3(0f, -(alturaiconeMultiplicador * i + .1f), 0f);
            //iconeDeMultiplicador.GetComponent<Image>().sprite = iconeMult;
            //multiplicadoresDesenhados.Add(iconeDeMultiplicador);
            ////icone do desastre
            //Image iconeDeDesastre = Instantiate(iconesDesastrPrefab, PosicaoIconesDesastre.transform.position, Quaternion.identity, PosicaoIconesDesastre.transform);
            //Sprite iconeDes = DesastresList.Instance.SelecionaSpriteDesastre(desastreManager.Instance.GetDesastreSorteado(i));
            ////SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteDesastre(desastresSorteados[i]);
            //float alturaiconeDesastre = iconeDeDesastre.rectTransform.rect.height;
            //iconeDeDesastre.transform.localPosition = new Vector3(0f, -(alturaiconeDesastre * i + .1f), 0f);
            //iconeDeDesastre.GetComponent<Image>().sprite = iconeDes;
            //iconesDesenhados.Add(iconeDeDesastre);
        }
    }
}
