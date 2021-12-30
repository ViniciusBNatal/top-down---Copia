using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class craftingSlot : MonoBehaviour, TocarSom
{
    public ReceitaDeCrafting receita;
    [SerializeField] private Image[] botoesDeForca = new Image[3];
    [SerializeField] private GameObject recursoNecessarioUIPrefab;
    [SerializeField] private GameObject recursosGrid;
    [SerializeField] private Image iconeDoModulo;
    [SerializeField] private Image iconeDoDesastre;
    //[SerializeField] private Image iconeBotaoConstrucao;
    private List<TMP_Text> qntdNecessariaParaCadarecursoText = new List<TMP_Text>();
    private List<int> novosValores = new List<int>();
    private List<Animator> iconesDeRecursosNecessarios = new List<Animator>();
    private int divisor = 4;
    private int forca = 1;
    //private bool forcaSelecionada = false;
    private void Start()
    {
        if (receita.desastre == "")
        {
            if(receita.modulo != 1)
                iconeDoDesastre.sprite = DesastresList.Instance.SelecionaSpriteModulo(receita.modulo);
            else
                iconeDoDesastre.enabled = false;
        }
        else
        {
            iconeDoDesastre.sprite = DesastresList.Instance.SelecionaSpriteDesastre(receita.desastre);
            botoesDeForca[0].color = Color.green;
        }
        //iconeDoModulo.sprite = DesastresList.Instance.SelecionaSpriteModulo(receita.modulo);
        for(int i = 0;i < receita.itensNecessarios.Count; i++)//adiciona a quantidade e a imagem para cada recurso na receita
        {
            GameObject obj = Instantiate(recursoNecessarioUIPrefab, recursosGrid.transform);
            float largura = obj.GetComponent<RectTransform>().rect.width;
            float altura = obj.GetComponent<RectTransform>().rect.height;
            obj.transform.localPosition = new Vector3((i % divisor) * largura, -(i / divisor) * altura, 0);
            obj.GetComponentInChildren<TMP_Text>().text = receita.quantidadeDosRecursos[i].ToString("000");
            novosValores.Add(receita.quantidadeDosRecursos[i]);
            qntdNecessariaParaCadarecursoText.Add(obj.GetComponentInChildren<TMP_Text>());
            obj.GetComponentInChildren<Image>().sprite = receita.itensNecessarios[i].icone;
            iconesDeRecursosNecessarios.Add(obj.GetComponent<Animator>());
        }
    }
    public void TrocaForcamodulo(int f)
    {
        forca = f;
        //forcaSelecionada = true;
        switch (f)
        {
            case 1:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = receita.quantidadeDosRecursos[i].ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i];
                }
                break;
            case 2:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = (receita.quantidadeDosRecursos[i] + receita.incrementoEntreForcas).ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i] + receita.incrementoEntreForcas;
                }
                break;
            case 3:
                for (int i = 0; i < qntdNecessariaParaCadarecursoText.Count; i++)
                {
                    qntdNecessariaParaCadarecursoText[i].text = (receita.quantidadeDosRecursos[i] + 2 * receita.incrementoEntreForcas).ToString("000");
                    novosValores[i] = receita.quantidadeDosRecursos[i] + 2 * receita.incrementoEntreForcas;
                }
                break;
        }
        for (int i = 0; i < botoesDeForca.Length; i++)
        {
            if (i == f - 1)
                botoesDeForca[i].color = Color.green;
            else
                botoesDeForca[i].color = Color.white;
        }
    }
    public void FalhaNoCrafting(bool Insuficiente, int recurso)
    {
        switch (Insuficiente)
        {
            case true:
                iconesDeRecursosNecessarios[recurso].SetTrigger("INSUFICIENTE");
                break;
            case false:
                iconesDeRecursosNecessarios[recurso].SetTrigger("INEXISTENTE");
                break;
        }
        TocarSom(SoundManager.Som.RecursosInsuficientesBotao, null);
    }
    public ReceitaDeCrafting construcaoConfirmada()
    {
        ReceitaDeCrafting moduloConfig = ScriptableObject.CreateInstance<ReceitaDeCrafting>();
        moduloConfig.modulo = receita.modulo;
        moduloConfig.desastre = receita.desastre.ToUpper();
        moduloConfig.SetForca(forca);
        for (int i = 0; i < receita.quantidadeDosRecursos.Count; i++)
        {
            moduloConfig.quantidadeDosRecursos.Add(novosValores[i]);
        }
        return moduloConfig;
    }
    public int GetForca()
    {
        return forca;
    }
    public void TocarSom(SoundManager.Som som, Transform origemSom)
    {
        SoundManager.Instance.TocarSom(som, origemSom);
    }
}
