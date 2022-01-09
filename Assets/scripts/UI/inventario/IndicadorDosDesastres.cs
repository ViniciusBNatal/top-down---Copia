using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IndicadorDosDesastres : MonoBehaviour
{
    public static IndicadorDosDesastres Instance { get; private set; }
    private List<iconeDesastre> iconesDesenhados = new List<iconeDesastre>();
    //private List<Image> iconesDesenhados = new List<Image>();
    //private List<Image> multiplicadoresDesenhados = new List<Image>();
    [SerializeField] private GameObject iconesDesastrPrefab;
    [SerializeField] private Transform PosIcones;
    private const int divisor = 5;
    //[SerializeField] private Image iconesDesastrPrefab;
    //[SerializeField] private GameObject PosicaoIconesDesastre;
    //[SerializeField] private GameObject PosicaoIconesMultiplicador;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    /*private void Start()
    {
        PreenchePlaca();
    }*/
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
        float distanciaEntreIcones = 0;
        LimpaPlaca();
        for (int i = 0; i < desastreManager.Instance.GetQntdDesastresParaOcorrer(); i++)//preenche a tela de acordo com o que foi sorteado e a quantidade de desastres
        {
            GameObject icone = Instantiate(iconesDesastrPrefab, PosIcones.transform);
            iconesDesenhados.Add(icone.GetComponent<iconeDesastre>());
            iconesDesenhados[i].forca = desastreManager.Instance.forcasSorteados[i];
            iconesDesenhados[i].desastre = desastreManager.Instance.GetDesastreSorteado(i);
            iconesDesenhados[i].imagemDoDesastre.sprite = DesastresList.Instance.SelecionaSpriteDesastre(desastreManager.Instance.GetDesastreSorteado(i));
            iconesDesenhados[i].textoForca.text = desastreManager.Instance.forcasSorteados[i].ToString();
            /*icone.GetComponent<iconeDesastre>().imagemDoDesastre.sprite = DesastresList.Instance.SelecionaSpriteDesastre(desastreManager.Instance.GetDesastreSorteado(i));
            icone.GetComponent<iconeDesastre>().texto.text = desastreManager.Instance.forcasSorteados[i].ToString();*/
            float largura = icone.GetComponent<RectTransform>().rect.width;
            float altura = icone.GetComponent<RectTransform>().rect.height;
            if (i > 0)
                distanciaEntreIcones = .5f;
            icone.transform.localPosition = new Vector3((i * -largura) + distanciaEntreIcones, -(i / divisor) * altura, 0);
            /*//icone do multiplicador
            Image iconeDeMultiplicador = Instantiate(iconesDesastrPrefab, PosicaoIconesMultiplicador.transform.position, Quaternion.identity, PosicaoIconesMultiplicador.transform);
            Sprite iconeMult = DesastresList.Instance.SelecionaSpriteMultiplicador(desastreManager.Instance.GetForcaSorteada(i));
            //SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteMultiplicador(forcasSorteados[i]);
            float alturaiconeMultiplicador = iconeDeMultiplicador.rectTransform.rect.height;
            iconeDeMultiplicador.transform.localPosition = new Vector3(0f, -(alturaiconeMultiplicador * i + .1f), 0f);
            iconeDeMultiplicador.GetComponent<Image>().sprite = iconeMult;
            multiplicadoresDesenhados.Add(iconeDeMultiplicador);
            //icone do desastre
            Image iconeDeDesastre = Instantiate(iconesDesastrPrefab, PosicaoIconesDesastre.transform.position, Quaternion.identity, PosicaoIconesDesastre.transform);
            Sprite iconeDes = DesastresList.Instance.SelecionaSpriteDesastre(desastreManager.Instance.GetDesastreSorteado(i));
            //SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteDesastre(desastresSorteados[i]);
            float alturaiconeDesastre = iconeDeDesastre.rectTransform.rect.height;
            iconeDeDesastre.transform.localPosition = new Vector3(0f, -(alturaiconeDesastre * i + .1f), 0f);
            iconeDeDesastre.GetComponent<Image>().sprite = iconeDes;
            iconesDesenhados.Add(iconeDeDesastre);*/
        }
        AtualizarCheckDeModuloConstruido();
    }
    public void AtualizarCheckDeModuloConstruido()
    {
        if (BaseScript.Instance != null)
        {
            for (int i = 0; i < iconesDesenhados.Count; i++)
            {
                int forcaTotal = 0;
                int ModulosNaoCorrespondentes = 0;
                for (int a = 0; a < BaseScript.Instance.GetQntdModulos(); a++)
                {
                    if (BaseScript.Instance.GetModuloNaLista(a).GetNomeDesastre() == iconesDesenhados[i].desastre && BaseScript.Instance.GetModuloNaLista(a).GetModulo() == 1)
                    {
                        if (BaseScript.Instance.GetModuloNaLista(a).GetForca() >= iconesDesenhados[i].forca)
                        {
                            iconesDesenhados[i].caixaDeCheck.enabled = true;
                            break;
                        }
                        else
                        {
                            forcaTotal += BaseScript.Instance.GetModuloNaLista(a).GetForca();
                            if (forcaTotal >= iconesDesenhados[i].forca)
                            {
                                iconesDesenhados[i].caixaDeCheck.enabled = true;
                                break;
                            }
                            else
                                iconesDesenhados[i].caixaDeCheck.enabled = false;
                        }
                    }
                    else
                    {
                        ModulosNaoCorrespondentes++;
                        if (ModulosNaoCorrespondentes == BaseScript.Instance.GetQntdModulos())
                            iconesDesenhados[i].caixaDeCheck.enabled = false;
                    }
                }
            }
            VerificarSeDefesaEstaPronta();
        }
    }
    public void VerificarSeDefesaEstaPronta()
    {
        int defesasProntas = 0;
        for (int i = 0; i < iconesDesenhados.Count; i++)
        {
            if (iconesDesenhados[i].caixaDeCheck.enabled)
                defesasProntas++;
        }
        if (defesasProntas == iconesDesenhados.Count && BaseScript.duranteMelhoria)
            desastreManager.Instance.ConfigurarTimer(3f, desastreManager.Instance.GetTempoAcumuladoParaDesastre(), false);
    }
}
