using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class desastreManager : MonoBehaviour
{
    public static desastreManager Instance { get; private set; }
    [Header("Configuração do Manager")]
    public float intervaloEntreOsDesastres = 90;
    public float intervaloDuranteTutorial;
    [SerializeField] private int chanceDeDesastre;
    public float intervaloDuranteADefesa;
    public int QntdDeDefesasNecessarias;
    private List<Image> iconesDesenhados = new List<Image>();
    private List<Image> multiplicadoresDesenhados = new List<Image>();
    private int minutos;
    private int segundos;
    [Header("Terremoto")]
    [SerializeField] private float intensidadeScreenShakeTerremoto;
    [SerializeField] private float taxaDeMudancaDosControles;
    [SerializeField] private float unidadesX;
    [SerializeField] private float unidadesY;
    [SerializeField] private float forcaTerremoto;
    [Header("Enxame De Insetos")]
    private GameObject enxameInst;
    [SerializeField] private GameObject enxamePrefab;
    [SerializeField] private float velocidadeEnxame;
    [SerializeField] private float danoEnxame;
    [SerializeField] private float intervaloEntreAtaques;
    [Header("Virus")]
    [SerializeField] private float distanciaXdoJogador_virus;
    [SerializeField] private float distanciaYdoJogador_virus;
    [SerializeField] private float intervaloEntreSpawnsVirus;
    [SerializeField] private float tempParaSumirUI;
    [SerializeField] private GameObject paredeVirusPrefab;
    [Header("Chuva Ácida")]
    [SerializeField] private GameObject chuvaParticulaPrefab;
    [SerializeField] private float danoDoAcido;
    [SerializeField] private float intervaloEntreHitsChuvaAcida;
    private GameObject chuvaInstance;
    [Header("Errupção Terrena")]
    [SerializeField] private GameObject errupcaoPrefab;
    [SerializeField] private float intervaloEntreSpawnsErrupcao;
    [SerializeField] private float distanciaXdoJogador_errupcao;
    [SerializeField] private float distanciaYdoJogador_errupcao;
    [Header ("Não Mexer")]
    public bool desastreAcontecendo = false;
    public float tempoAcumulado = 0f;
    public float tempoRestante;
    public int qntdDeDesastresParaOcorrer;
    public string[] desastresSorteados = new string[5];
    public int[] forcasSorteados = new int[5];
    public List<GameObject> errupcoesEmCena = new List<GameObject>();
    public List<GameObject> virusEmCena = new List<GameObject>();
    [SerializeField] private PostProcessScript CMefeitos;
    [SerializeField] private Image iconesDesastrPrefab;
    [SerializeField] private GameObject PosicaoIconesDesastre;
    [SerializeField] private GameObject PosicaoIconesMultiplicador;
    [SerializeField] private Text timer;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        tempoRestante = intervaloEntreOsDesastres;
        //ConfigurarTimer(intervaloEntreOsDesastres, tempoAcumulado);
        //StartCoroutine(this.LogicaDesastres());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ConfigurarTimer(intervaloEntreOsDesastres, tempoAcumulado);
            StartCoroutine(this.LogicaDesastres(true));
            //Debug.Log(desastresSorteados[0] + "," + desastresSorteados[1] + "," + desastresSorteados[2] + "," + desastresSorteados[3] + "," + desastresSorteados[4]);
        }
    }
    //desastres
    private void Terremoto()
    {
        CinemachineShake.Instance.ScreenShake(intensidadeScreenShakeTerremoto, true);
        StartCoroutine("baguncaControleJogador");
    }
    private void NuvemDeInsetos()
    {
        int esq_dir = Random.Range(-1, 2);
        int cima_baixo = Random.Range(-1, 2);
        if (cima_baixo == 0 && esq_dir == 0)
            esq_dir = 1;
        float posX = jogadorScript.Instance.mainCamera.orthographicSize * Screen.width / Screen.height;
        float posY = jogadorScript.Instance.mainCamera.orthographicSize;
        enxameInst = Instantiate(enxamePrefab, new Vector3(jogadorScript.Instance.transform.position.x + (posX + 1) * esq_dir, jogadorScript.Instance.transform.position.y + (posY + 1) * cima_baixo, 0f), Quaternion.identity);
    }
    private void Virus()
    {
        CMefeitos.visualVirus(true);
        StartCoroutine(this.criaObjetosDoDesastre(distanciaXdoJogador_virus, distanciaYdoJogador_virus, intervaloEntreSpawnsVirus, paredeVirusPrefab));
        StartCoroutine(this.delayVirus());
    }
    private void ChuvaAcida()
    {
        chuvaInstance = Instantiate(chuvaParticulaPrefab, jogadorScript.Instance.transform);
        StartCoroutine(this.HitChuvaAcida());
    }
    private void ErrupcaoTerrena()
    {
        CMefeitos.visualErrupcaoTerrena(true);
        StartCoroutine(this.criaObjetosDoDesastre(distanciaXdoJogador_errupcao, distanciaYdoJogador_errupcao, intervaloEntreSpawnsErrupcao, errupcaoPrefab));
    }
    //sorteadores
    public void ConfigurarTimer(float tempRestante, float tempoAcumuladoduranteDesastres)
    {
        tempoRestante = tempRestante;
        minutos = Mathf.FloorToInt((tempRestante - tempoAcumuladoduranteDesastres) / 60);
        if (minutos < 0)
            minutos = 0;
        segundos = Mathf.FloorToInt((tempRestante - minutos * 60) - tempoAcumuladoduranteDesastres);
        if (segundos < 0)
            segundos = 0;
        timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }
    public IEnumerator LogicaDesastres(bool sortearDesastres)
    {
        if (sortearDesastres)
        {
            SortearGeral(chanceDeDesastre);
        }
        //qntdDeDesastresParaOcorrer = 1;
        //desastresSorteados[0] = "terremoto";
        //forcasSorteados[0] = 1;
        //desastresSorteados[1] = 2;
        //forcasSorteados[1] = 2;
        //desastresSorteados[2] = 3;
        //forcasSorteados[2] = 3;
        PreenchePlaca();
        while (desastreAcontecendo == false)
        {
            timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
            yield return new WaitForSeconds(1);
            if (minutos == 0 && segundos == 0)
                IniciarDesastres();
            else if(segundos == 0 && minutos > 0)
            {
                minutos--;
                segundos = 59;
                tempoRestante--;
            }
            else
            {
                segundos--;
                tempoRestante--;
            }
        }
    }
    //IEnumerators
    private void SortearGeral(int chanceDEDesastre)
    {
        int qntd = 0;
        for (int i = 0; i < DesastresList.Instance.ativar.Count; i++)
        {
            if (DesastresList.Instance.ativar[i])
                qntd++;
        }
        qntdDeDesastresParaOcorrer = qntd;
        for (int i = 0; i < qntd; i++)
        {
            int forcaEscolhida = Random.Range(1, 4);
            forcasSorteados[i] = forcaEscolhida;
            SorteiaDesastre();
        }
    }
    private void SorteiaDesastre()
    {
        bool EventoRepetido = false;
        int index = Random.Range(1, DesastresList.Instance.ativar.Count + 1);
        string desastreEscolhido = DesastresList.Instance.desastreNome[index - 1].ToUpper();
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            if (desastresSorteados[i].ToUpper() == desastreEscolhido)
            {
                EventoRepetido = true;
                break;
            }
        }
        if (!DesastresList.Instance.ativar[index - 1] || EventoRepetido)
        {
            SorteiaDesastre();
            return;
        }
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            if (desastresSorteados[i] == "")
            {
                desastresSorteados[i] = desastreEscolhido;
                break;
            }
        }
    }
    //visual
    private void LimpaPlaca()
    {
        if (iconesDesenhados != null)
        {
            for (int i = iconesDesenhados.Count; i > 0; i--)
            {
                Destroy(iconesDesenhados[i - 1].gameObject);
                iconesDesenhados.RemoveAt(i - 1);
            }
        }
        if (multiplicadoresDesenhados != null)
        {
            for (int i = multiplicadoresDesenhados.Count; i > 0; i--)
            {
                Destroy(multiplicadoresDesenhados[i - 1].gameObject);
                multiplicadoresDesenhados.RemoveAt(i - 1);
            }
        }
    }
    private void PreenchePlaca()
    {
        //limpa os icones para os próximos desastres
        LimpaPlaca();
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)//preenche a tela de acordo com o que foi sorteado e a quantidade de desastres
        {
            //icone do multiplicador
            Image iconeDeMultiplicador = Instantiate(iconesDesastrPrefab, PosicaoIconesMultiplicador.transform.position, Quaternion.identity, PosicaoIconesMultiplicador.transform);
            Sprite iconeMult = DesastresList.Instance.SelecionaSpriteMultiplicador(forcasSorteados[i]);
                //SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteMultiplicador(forcasSorteados[i]);
            float alturaiconeMultiplicador = iconeDeMultiplicador.rectTransform.rect.height;
            iconeDeMultiplicador.transform.localPosition = new Vector3(0f, -(alturaiconeMultiplicador * i + .1f), 0f);
            iconeDeMultiplicador.GetComponent<Image>().sprite = iconeMult;
            multiplicadoresDesenhados.Add(iconeDeMultiplicador);
            //icone do desastre
            Image iconeDeDesastre = Instantiate(iconesDesastrPrefab, PosicaoIconesDesastre.transform.position, Quaternion.identity, PosicaoIconesDesastre.transform);
            Sprite iconeDes = DesastresList.Instance.SelecionaSpriteDesastre(desastresSorteados[i]);
                //SelecionadorDeIconeDesastreEMultiplicador.Instance.SelecionarSpriteDesastre(desastresSorteados[i]);
            float alturaiconeDesastre = iconeDeDesastre.rectTransform.rect.height;
            iconeDeDesastre.transform.localPosition = new Vector3(0f, -(alturaiconeDesastre * i + .1f), 0f);
            iconeDeDesastre.GetComponent<Image>().sprite = iconeDes;
            iconesDesenhados.Add(iconeDeDesastre);
        }
    }
    //logica dos desastres
    private void IniciarDesastres()
    {
        desastreAcontecendo = true;
        //Debug.Log(desastresSorteados[0] + "," + desastresSorteados[1] + "," + desastresSorteados[2] + "," + desastresSorteados[3] + "," + desastresSorteados[4]);
        //Terremoto();
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            switch (desastresSorteados[i].ToUpper())
            {
                case "TERREMOTO":
                    Terremoto();
                    break;
                case "ERRUPCAO TERRENA":
                    ErrupcaoTerrena();
                    break;
                case "NUVEM DE INSETOS":
                    NuvemDeInsetos();
                    break;
                case "VIRUS":
                    Virus();
                    break;
                case "CHUVA ACIDA":
                    ChuvaAcida();
                    break;
            }
        }
    }
    public void encerramentoDesastres()
    {
        //terremoto
        CinemachineShake.Instance.ScreenShake(0f, false);
        jogadorScript.Instance.baguncarControles = Vector2.zero;
        //nuvem de insetos
        if (enxamePrefab != null)
            Destroy(enxameInst);
        //virus
        CMefeitos.visualVirus(false);
        UIinventario.Instance.GetComponent<Canvas>().enabled = true;
        for (int i = virusEmCena.Count; i > 0; i--)
        {
            Destroy(virusEmCena[i - 1]);
            virusEmCena.RemoveAt(i - 1);
        }
        //Chuva Acida
        Destroy(chuvaInstance);
        //Errupcao Terrena
        CMefeitos.visualErrupcaoTerrena(false);
        for (int i = errupcoesEmCena.Count; i > 0; i--)
        {
            Destroy(errupcoesEmCena[i - 1]);
            errupcoesEmCena.RemoveAt(i - 1);
        }
    }
    public void LimpaArraysDeSorteio()
    {
        for (int i = 0; i < desastresSorteados.Length; i++)
        {
            forcasSorteados[i] = 0;
            desastresSorteados[i] = "";
        }
    }
    IEnumerator baguncaControleJogador()
    {
        while (desastreAcontecendo)
        {
            jogadorScript.Instance.BaguncaControles(unidadesX, unidadesY, forcaTerremoto);
            yield return new WaitForSeconds(taxaDeMudancaDosControles);
        }
    }
    IEnumerator criaObjetosDoDesastre(float distX, float distY, float tempo, GameObject prefab)
    {
        while (desastreAcontecendo)
        {
            float X = Random.Range(-distX, distX + 1);
            float Y = Random.Range(-distY, distY + 1);
            GameObject obj = Instantiate(prefab, jogadorScript.Instance.transform.position + new Vector3(X, Y, 0f), Quaternion.identity);
            yield return new WaitForSeconds(intervaloEntreSpawnsErrupcao);
        }
    }
    IEnumerator delayVirus()
    {
        yield return new WaitForSeconds(tempParaSumirUI);
        if (desastreAcontecendo)
            UIinventario.Instance.GetComponent<Canvas>().enabled = false;
    }
    IEnumerator HitChuvaAcida()
    {
        while (desastreAcontecendo)
        {
            jogadorScript.Instance.mudancaRelogio(danoDoAcido);
            yield return new WaitForSeconds(intervaloEntreHitsChuvaAcida);
        }
    }
}