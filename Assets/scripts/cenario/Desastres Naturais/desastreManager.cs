using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class desastreManager : MonoBehaviour, AcoesNoTutorial
{
    public static desastreManager Instance { get; private set; }
    [Header("CONFIGURAÇÃO DO MANAGER")]
    [SerializeField] private float intervaloEntreOsDesastres;
    [SerializeField] private int chanceDeDesastre;
    [SerializeField] private float TempoMinimoRestante;
    [SerializeField] private float ReducaoDeTempAoLiberarNovaFase;
    [SerializeField] private bool tutorial;
    private int minutos;
    private int segundos;
    [Header("TERREMOTO")]
    [SerializeField] private float intensidadeScreenShakeTerremoto;
    [SerializeField] private float taxaDeMudancaDosControles;
    [SerializeField] private float unidadesX;
    [SerializeField] private float unidadesY;
    [SerializeField] private float forcaTerremoto;
    [Header("ENXAME DE INSETOS")]
    [SerializeField] private GameObject enxamePrefab;
    [SerializeField] private float velocidadeEnxame;
    [SerializeField] private float danoEnxame;
    [SerializeField] private float intervaloEntreAtaques;
    private GameObject enxameInst;
    [Header("VIRUS")]
    [SerializeField] private float intervaloEntreSpawnsVirus;
    [SerializeField] private float tempParaSumirUI;
    [SerializeField] private GameObject paredeVirusPrefab;
    [SerializeField] private float distanciaXdoJogador_virusMin;
    //private float distanciaXdoJogador_virusMax = jogadorScript.Instance.mainCamera.orthographicSize * Screen.width / Screen.height - .5f;
    [SerializeField] private float distanciaYdoJogador_virusMin;
    //private float distanciaYdoJogador_virusMax = jogadorScript.Instance.mainCamera.orthographicSize - .5f;
    [Header("CHUVA ÁCIDA")]
    [SerializeField] private GameObject chuvaParticulaPrefab;
    [SerializeField] private float danoDoAcido;
    [SerializeField] private float intervaloEntreHitsChuvaAcida;
    private GameObject chuvaInstance;
    [Header("ERRUPÇÃO TERRENA")]
    [SerializeField] private GameObject errupcaoPrefab;
    [SerializeField] private float intervaloEntreSpawnsErrupcao;
    [SerializeField] private float distanciaXdoJogador_errupcaoMin;
    [SerializeField] private float distanciaYdoJogador_errupcaoMin;
    [Header("NÃO MEXER")]
    //[SerializeField] private PostProcessScript CMefeitos;
    [SerializeField] private TMP_Text timer;
    private bool desastreAcontecendo = false;
    private float tempoAcumulado = 0f;
    private float tempoRestante;
    private int qntdDeDesastresParaOcorrer;
    public string[] desastresSorteados = new string[5];
    public int[] forcasSorteados = new int[5];
    private List<GameObject> errupcoesEmCena = new List<GameObject>();
    private List<GameObject> virusEmCena = new List<GameObject>();
    private void Awake()
    {
        if(Instance == null)
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
            PararTodasCorotinas();
            qntdDeDesastresParaOcorrer = 2;
            desastresSorteados[0] = "TERREMOTO";
            desastresSorteados[1] = "ERRUPCAO TERRENA";
    //        //desastresSorteados[2] = "NUVEM DE INSETOS";
            forcasSorteados[0] = 2;
            forcasSorteados[1] = 2;
    //        //forcasSorteados[2] = 1;
            IndicadorDosDesastres.Instance.PreenchePlaca();
    //        //Virus();
    //        //ErrupcaoTerrena();
    //        //Terremoto();
    //        //StopAllCoroutines();
            ConfigurarTimer(3f, tempoAcumulado, true);
    //        SetUpParaNovoSorteioDeDesastres();
            StartCoroutine(this.LogicaDesastres(false));
    //        //Debug.Log(desastresSorteados[0] + "," + desastresSorteados[1] + "," + desastresSorteados[2] + "," + desastresSorteados[3] + "," + desastresSorteados[4]);
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
        //CMefeitos.visualVirus(true);
        PostProcessScript.Instance.visualVirus(true);
        float distanciaXdoJogador_virusMax = jogadorScript.Instance.mainCamera.orthographicSize * Screen.width / Screen.height - .5f;
        float distanciaYdoJogador_virusMax = jogadorScript.Instance.mainCamera.orthographicSize - .5f;
        StartCoroutine(this.criaObjetosDoDesastre(distanciaXdoJogador_virusMin, distanciaXdoJogador_virusMax, distanciaYdoJogador_virusMin, distanciaYdoJogador_virusMax, intervaloEntreSpawnsVirus, paredeVirusPrefab));
        StartCoroutine(this.delayVirus());
    }
    private void ChuvaAcida()
    {
        chuvaInstance = Instantiate(chuvaParticulaPrefab, jogadorScript.Instance.transform);
        StartCoroutine(this.HitChuvaAcida());
    }
    private void ErrupcaoTerrena()
    {
        //CMefeitos.visualErrupcaoTerrena(true);
        PostProcessScript.Instance.visualErrupcaoTerrena(true);
        float distanciaXdoJogador_errupcaoMax = jogadorScript.Instance.mainCamera.orthographicSize * Screen.width / Screen.height - .5f;
        float distanciaYdoJogador_errupcaoMax = jogadorScript.Instance.mainCamera.orthographicSize - .5f;
        StartCoroutine(this.criaObjetosDoDesastre(distanciaXdoJogador_errupcaoMin, distanciaXdoJogador_errupcaoMax, distanciaYdoJogador_errupcaoMin, distanciaYdoJogador_errupcaoMax, intervaloEntreSpawnsErrupcao, errupcaoPrefab));
    }
    //
    public void ConfigurarTimer(float tempRestante, float tempoAcumuladoduranteDesastres, bool animar)
    {
        tempoRestante = tempRestante;
        minutos = Mathf.FloorToInt((tempRestante - tempoAcumuladoduranteDesastres) / 60);
        if (minutos < 0)
            minutos = 0;
        segundos = Mathf.FloorToInt((tempRestante - minutos * 60) - tempoAcumuladoduranteDesastres);
        if (segundos < 0)
            segundos = 0;
        timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
        if (animar)
            timer.GetComponent<Animator>().SetTrigger("AVISO");
    }
    public void IniciarCorrotinaLogicaDesastres(bool sortearDesastres)
    {
        StartCoroutine(Instance.LogicaDesastres(sortearDesastres));
    }
    public void PararTodasCorotinas()
    {
        StopAllCoroutines();
    }
    private IEnumerator LogicaDesastres(bool sortearDesastres)
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
        IndicadorDosDesastres.Instance.PreenchePlaca();
        while (!desastreAcontecendo)
        {
            yield return new WaitForSeconds(1f);
            if (jogadorScript.Instance.GetEstadoAtualJogador() != jogadorScript.estados.EmDialogo)
            {
                timer.text = minutos.ToString("00") + ":" + segundos.ToString("00");
                if (segundos == 0 && minutos == 0)
                {
                    IniciarDesastres();
                }
                else if (segundos == 0 && minutos > 0)
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
                TempoCurto();
            }
        }
    }
    private void TempoCurto()
    {
        if (tempoRestante <= 10f)
            AvisoDePerigoTimer();
    }
    public void AvisoDePerigoTimer()
    {
        timer.GetComponent<Animator>().SetTrigger("PERIGO");
    }
    private void SortearGeral(int chanceDesastre)
    {
        int desastresParaocorrer = 0;
        int desastresPossiveis = 0;
        for (int i = 0; i < DesastresList.Instance.ativar.Count; i++)//verifica quais desastres podem acontecer
        {
            if (DesastresList.Instance.ativar[i] == true)
                desastresPossiveis++;
        }
        if (BaseScript.Instance.GetDuranteDefesaParaMelhorarBase())//se estiver durante uma defesa sempre acontecerá todos os desastres possíveis
            DefinirQntdDeDesastresParaOcorrer(desastresPossiveis);
        else
        {
            for (int i = 0; i < desastresPossiveis; i++)
            {
                int r = Random.Range(1, 101);
                if (r <= chanceDesastre)
                    desastresParaocorrer++;
            }
            if (desastresParaocorrer == 0)
            {
                SetUpParaNovoSorteioDeDesastres();
                SortearGeral(chanceDesastre);
                return;
            }
            DefinirQntdDeDesastresParaOcorrer(desastresParaocorrer);
        }
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            int forcaEscolhida = Random.Range(1, 4);
            DefinirForcaParaDesastre(i, forcaEscolhida);
            SorteiaDesastre(desastresPossiveis);
        }
    }
    private void SorteiaDesastre(int desastrPossiveis)
    {
        int index = Random.Range(1, desastrPossiveis + 1);
        string desastreEscolhido = DesastresList.Instance.desastreNome[index - 1].ToUpper();
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            if (desastresSorteados[i].ToUpper() == desastreEscolhido)
            {
                SorteiaDesastre(desastrPossiveis);
                return;
            }
        }
        for (int i = 0; i < qntdDeDesastresParaOcorrer; i++)
        {
            if (desastresSorteados[i] == "")
            {
                DefinirDesastre(i, desastreEscolhido);
                break;
            }
        }
    }
    //logica dos desastres
    private void IniciarDesastres()
    {
        desastreAcontecendo = true;
        Ativar_desativarInteracoesDaBase(true);
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
        jogadorScript.Instance.GetComponent<Rigidbody2D>().AddForce(Vector2.one);//garante que se o jogador estiver parado dentro da maquina, irá verificar a defesa
    }
    public void encerramentoDesastres()
    {
        desastreAcontecendo = false;
        //terremoto
        CinemachineShake.Instance.ScreenShake(0f, false);
        jogadorScript.Instance.SetDirecaoDeMovimentacaoAleatoria(Vector2.zero);
        //nuvem de insetos
        if (enxamePrefab != null)
            Destroy(enxameInst);
        //virus
        //CMefeitos.visualVirus(false);
        PostProcessScript.Instance.visualVirus(false);
        UIinventario.Instance.GetComponent<Canvas>().enabled = true;
        if (virusEmCena.Count > 0)
        {
            for (int i = virusEmCena.Count; i > 0; i--)
            {
                Destroy(virusEmCena[i - 1]);
                //virusEmCena.RemoveAt(i - 1);
            }
            virusEmCena.Clear();
        }
        //Chuva Acida
        Destroy(chuvaInstance);
        //Errupcao Terrena
        //CMefeitos.visualErrupcaoTerrena(false);
        PostProcessScript.Instance.visualErrupcaoTerrena(false);
        if (errupcoesEmCena.Count > 0)
        {
            for (int i = errupcoesEmCena.Count; i > 0; i--)
            {
                Destroy(errupcoesEmCena[i - 1]);
                //errupcoesEmCena.RemoveAt(i - 1);
            }
            errupcoesEmCena.Clear();
        }
        Ativar_desativarInteracoesDaBase(false);
        IndicadorDosDesastres.Instance.LimpaPlaca();
        SetUpParaNovoSorteioDeDesastres();
        Tutorial();
    }
    private void SetUpParaNovoSorteioDeDesastres()
    {
        for (int i = 0; i < desastresSorteados.Length; i++)
        {
            DefinirDesastre(i, "");
            DefinirForcaParaDesastre(i, 0);
        }
        DefinirQntdDeDesastresParaOcorrer(0);
    }
    IEnumerator baguncaControleJogador()
    {
        while (desastreAcontecendo)
        {
            jogadorScript.Instance.BaguncaControles(unidadesX, unidadesY, forcaTerremoto);
            yield return new WaitForSeconds(taxaDeMudancaDosControles);
        }
    }
    IEnumerator criaObjetosDoDesastre(float distXMin, float distXMax, float disYMin, float disYMax, float tempo, GameObject prefab)
    {
        while (desastreAcontecendo)
        {
            int dirX = Random.Range(-1, 1);
            int diry = Random.Range(-1, 1);
            if (dirX == 0)
                dirX = 1;
            if (diry == 0)
                diry = 1;
            float X = Random.Range(distXMin, distXMax + 1);
            float Y = Random.Range(disYMin, disYMax + 1);
            GameObject obj = Instantiate(prefab, jogadorScript.Instance.transform.position + new Vector3(X * dirX, Y * diry, 0f), Quaternion.identity);
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
            jogadorScript.Instance.mudancaRelogio(danoDoAcido, .15f);
            yield return new WaitForSeconds(intervaloEntreHitsChuvaAcida);
        }
    }
    public void Tutorial()
    {
        if (tutorial)
        {
            //DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
            tutorial = false;
        }
    }
    public void AdionarVirusALista(GameObject obj) 
    {
        virusEmCena.Add(obj);
    }
    public void RemoverVirusDaLista(GameObject obj)
    {
        virusEmCena.Remove(obj);
    }
    public void AdionarErrupcaoALista(GameObject obj)
    {
        errupcoesEmCena.Add(obj);
    }
    public void RemoverErrupcaoDaLista(GameObject obj)
    {
        errupcoesEmCena.Remove(obj);
    }
    public void Ativar_desativarInteracoesDaBase(bool trancar_DestrancarInteracoes)
    {
        UIinventario.Instance.fechaInventario();
        UIinventario.Instance.fechaMenuDeTempos();
        if (trancar_DestrancarInteracoes)
        {
            if (mesaCraftingScript.Instance != null)
                mesaCraftingScript.Instance.Desativar_AtivarInteracao(false);
            if (BaseScript.Instance != null)
                BaseScript.Instance.Ativar_DesativarInteracao(false);
        }
        else
        {
            if (mesaCraftingScript.Instance != null)
                mesaCraftingScript.Instance.Desativar_AtivarInteracao(true);
            if (BaseScript.Instance != null)
                BaseScript.Instance.Ativar_DesativarInteracao(true);
        }
    }
    public int GetForcaSorteada(int i)
    {
        return forcasSorteados[i];
    }
    public void DefinirForcaParaDesastre(int i, int forca)
    {
        forcasSorteados[i] = forca;
    }
    public string GetDesastreSorteado(int i)
    {
        return desastresSorteados[i].ToUpper();
    }
    public void DefinirDesastre(int i, string desastre)
    {
        desastresSorteados[i] = desastre.ToUpper();
    }
    public int GetQntdDesastresParaOcorrer()
    {
        return qntdDeDesastresParaOcorrer;
    }
    public void DefinirQntdDeDesastresParaOcorrer(int qntd)
    {
        qntdDeDesastresParaOcorrer = qntd;
    }
    public void DiminuirTempoRestanteParaDesastre(float temp)
    {
        temp = Mathf.Clamp(temp, 0, tempoRestante);
        if (tempoRestante > TempoMinimoRestante)
        {
            if (tempoRestante - temp < TempoMinimoRestante)
            {
                tempoRestante = TempoMinimoRestante;
            }
            tempoRestante -= temp;
        }
    }
    public float GetTempoRestanteParaDesastre()
    {
        return tempoRestante;
    }
    public void MudarTempoAcumuladoParaDesastre(float temp)
    {
        tempoAcumulado = temp;
    }
    public float GetTempoAcumuladoParaDesastre()
    {
        return tempoAcumulado;
    }
    public bool VerificarSeUmDesastreEstaAcontecendo()
    {
        return desastreAcontecendo;
    }
    public float GetIntervaloDeTempoEntreOsDesastres()
    {
        if (BossAlho.Instance != null)
            return intervaloEntreOsDesastres - BossAlho.Instance.GetReducaoIntervaloDesastres();
        else
            return intervaloEntreOsDesastres - (UIinventario.Instance.GetTempoAtual() - 1) * ReducaoDeTempAoLiberarNovaFase;
    }
}