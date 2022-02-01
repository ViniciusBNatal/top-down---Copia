using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissoesManager : MonoBehaviour
{
    public static MissoesManager Instance { get; private set; }
    [Header("Não Mexer")]
    [SerializeField] private GameObject ConteudoMissaoPrefab;
    [SerializeField] private Transform scrollViewConteudo;
    [SerializeField] private Transform TrackerMissoes;
    [SerializeField] private GameObject textoObjetivoUI;
    [NonSerialized] public Dictionary<Missao, missaoPrefabScript> missoesAtivasMenu = new Dictionary<Missao, missaoPrefabScript>();
    [NonSerialized] public Dictionary<Missao, missaoPrefabScript> missoesAtivasTracker = new Dictionary<Missao, missaoPrefabScript>();
    [NonSerialized] public List<GameObject> posicaoMissoesNoTracker = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        //foreach (Missao m in missoesTutorial)
            //statusMissoesTutorial.Add(m.IDMissao, m);
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.KeypadEnter))
    //    {
    //        AdicionarMissao(statusMissoesTutorial, 0);
    //    }
    //}
    public void AdicionarMissao(Missao missaoScrObj)
    {
        if (!missoesAtivasMenu.ContainsKey(missaoScrObj) && !missoesAtivasTracker.ContainsKey(missaoScrObj))
        {
            textoObjetivoUI.SetActive(true);
            //cria a missão na aba de missões
            GameObject missao = Instantiate(ConteudoMissaoPrefab, scrollViewConteudo.position, Quaternion.identity, scrollViewConteudo);
            missaoPrefabScript missaoScript = missao.GetComponent<missaoPrefabScript>();
            missaoScript.missaoScrObj = missaoScrObj;
            missaoScript.EscreverMissao();
            //cria a missão no tracker de missões
            GameObject temp = Instantiate(missao, TrackerMissoes.position, Quaternion.identity, TrackerMissoes);
            temp.GetComponent<missaoPrefabScript>().textoDetalhesMissao.gameObject.SetActive(false);
            temp.GetComponent<missaoPrefabScript>().molduraMissao.enabled = false;
            float largura = temp.GetComponent<RectTransform>().rect.width;
            float altura = temp.GetComponent<missaoPrefabScript>().caixaIconeEResumoMissao.rect.height;
            temp.transform.localPosition = new Vector3(largura / 2f, -(missoesAtivasTracker.Count * altura + (missoesAtivasTracker.Count * 10f)), 0);
            //salva missão nas listas para remoção futura
            missoesAtivasTracker.Add(missaoScrObj, temp.GetComponent<missaoPrefabScript>());
            missoesAtivasMenu.Add(missaoScrObj, missaoScript);
            posicaoMissoesNoTracker.Add(temp);
        }            
        else
            Debug.LogWarning("Recebendo Missão " + missaoScrObj.name + " Repetida");
    }

    public void ConcluirMissao(Missao missao)
    {
        if (missoesAtivasMenu.ContainsKey(missao))
        {
            missoesAtivasTracker[missao].ConcluirMissao();
            missoesAtivasMenu[missao].ConcluirMissao();
        }
        else
            Debug.LogWarning("Missao " + missao.name + " Não foi recebida");
    }
    public void ReposicionarMissoesNoTracker()
    {
        if (posicaoMissoesNoTracker.Count > 0)
        {
            float largura = posicaoMissoesNoTracker[0].GetComponent<RectTransform>().rect.width;
            float altura = posicaoMissoesNoTracker[0].GetComponent<missaoPrefabScript>().caixaIconeEResumoMissao.rect.height;
            for (int i = 0; i < posicaoMissoesNoTracker.Count; i++)
            {
                posicaoMissoesNoTracker[i].transform.localPosition = new Vector3(largura / 2f, -(i * altura), 0);
            }
        }
    }
}

