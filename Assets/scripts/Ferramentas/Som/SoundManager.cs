using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private static Dictionary<Som, float> intervalosDosSons = new Dictionary<Som, float>();
    private static Dictionary<Som, float> ultimaVezTocado = new Dictionary<Som, float>();
    [SerializeField] private GameObject SomEfeitosGobjPrefab;
    [SerializeField] private GameObject SomMusicaGobjPrefab;
    private static GameObject SomGobj;
    private static AudioSource SomEfeitosSource;
    private static GameObject SomMusicaGobj;
    private static AudioSource SomMusicaSource;
    public SomConfig[] Sons;
    private static Dictionary<string, SomConfig> SonsDicionario = new Dictionary<string, SomConfig>();
    [SerializeField] private Som somTestes;
    public enum Som
    {
        MusicaMenu,
        MusicaTutorial,
        MusicaBaseJogador,
        MusicaFloresta,
        MusicaCidade,
        MusicaLixao,
        JogadorAtirando,
        JogadorAtqMelee,
        JogadorLevouHit,
        JogadorColetouItem,
        JogadorBateuEmRecurso,
        InimigoLevouHit,
        BaseExplodindo,
        BaseDefendida,
        BaseLevouHit,
        ModuloExplodindo,
        ConstrucaoModulo,
        ViagemNoTempo,
        DesastreVirus,
        DesastreTerremoto,
        DesastreErupcao,
        DesastreChuvaAcida,
        DesastreEnxame,
        AbelhaLevouHit,
        RoboSentiAtirando,
        RoboLixaoLevouHit,
        RoboLixaoAtaque,
        RatoLevouHit,
        MinhocaAtirando,
        MinhocaLevouHit,
        AlhoSendoPlantado,
        AreaDeGasAlho,
        MisselEmVoo,
        MisselExplodindo,
        BotaoConstruiuModulo,
        RecursosInsuficientesBotao,
        ContagemRegressiva,
    };
    public enum TipoSom
    {
        Musica,
        EfeitoSonoro
    };
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupAudios();
        }
        else
            Destroy(gameObject);
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //        TocarSom(somTestes, null);
    //    if (Input.GetKeyDown(KeyCode.P))
    //        PararEfeitosSonoros();
    //}
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Musica();
    }
    private void OnLevelWasLoaded(int level)
    {
        Musica();
    }
    private void SetupAudios()
    {
        foreach(SomConfig somcnf in Sons)
        {
            SonsDicionario.Add(somcnf.som.ToString(), somcnf);
            if (somcnf.intervaloEntreSons > 0)
            {
                intervalosDosSons.Add(somcnf.som, somcnf.intervaloEntreSons);
                ultimaVezTocado.Add(somcnf.som, 0f);
            }
        }
    }
    public void TocarSom(Som tipoDoSom, Transform origemSom)
    {
        if (PodeTocarSom(tipoDoSom))
        {
            if (SomGobj == null)
            {
                SomGobj = Instantiate(SomEfeitosGobjPrefab, this.transform);
                SomEfeitosSource = SomGobj.GetComponent<AudioSource>();
                SomGobj.transform.SetParent(this.transform);
            }
            if (origemSom != null)
            {
                if (origemSom.gameObject.GetComponentInChildren<AudioSource>())
                {
                    AudioSourceConfiguracao(PegarSom(tipoDoSom).som, origemSom.gameObject.GetComponentInChildren<AudioSource>());
                    origemSom.gameObject.GetComponentInChildren<AudioSource>().Play();
                    //origemSom.gameObject.GetComponentInChildren<AudioSource>().PlayOneShot(AudioSourceConfiguracao(PegarSom(tipoDoSom).som, SomEfeitosSource).ArquivosDESom[Random.Range(0, PegarSom(tipoDoSom).ArquivosDESom.Length)]);
                }
                else
                {
                    GameObject gobj = Instantiate(SomEfeitosGobjPrefab, origemSom.position, Quaternion.identity, origemSom);//new GameObject("Somgobj")
                    AudioSourceConfiguracao(PegarSom(tipoDoSom).som, gobj.GetComponent<AudioSource>());
                    gobj.GetComponent<AudioSource>().Play();
                    //gobj.GetComponent<AudioSource>().PlayOneShot(AudioSourceConfiguracao(PegarSom(tipoDoSom).som, SomEfeitosSource).ArquivosDESom[Random.Range(0, PegarSom(tipoDoSom).ArquivosDESom.Length)]);
                }
            }
            else
            {
                SomEfeitosSource.PlayOneShot(AudioSourceConfiguracao(PegarSom(tipoDoSom).som, SomEfeitosSource).ArquivosDESom[Random.Range(0, PegarSom(tipoDoSom).ArquivosDESom.Length)]);
            }
        }
    }
    private SomConfig AudioSourceConfiguracao(Som tipoDoSom, AudioSource ausc)
    {
        SomConfig somEscolhido = PegarSom(tipoDoSom);
        ausc.volume = 1;//somEscolhido.Volume;
        ausc.pitch = somEscolhido.Pitch;
        ausc.loop = somEscolhido.Loop;
        ausc.clip = somEscolhido.ArquivosDESom[Random.Range(0, somEscolhido.ArquivosDESom.Length)];
        return somEscolhido;
    }
    public SomConfig PegarSom(Som tipoSom)
    {
        if (SonsDicionario.ContainsKey(tipoSom.ToString()))
        {
            return SonsDicionario[tipoSom.ToString()];
        }
        else
        {
            Debug.LogError("Som" + tipoSom + "Não encontrado!");
            return null;
        }
    }
    private bool PodeTocarSom(Som tipoSom)
    {
        if (intervalosDosSons.ContainsKey(tipoSom))
        {
            if (SonsDicionario.ContainsKey(tipoSom.ToString()))
            {
                float intervalo = intervalosDosSons[tipoSom];
                float ultimaVez = ultimaVezTocado[tipoSom];
                if (ultimaVez + intervalo < Time.time)
                {
                    ultimaVezTocado[tipoSom] = Time.time;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                Debug.LogError("Som" + tipoSom + "Não encontrado!");
                return false;
            }
        }
        else
            return true;
    }
    private void Musica()
    {
        if (SomMusicaGobj == null)
        {
            SomMusicaGobj = Instantiate(SomMusicaGobjPrefab, this.transform);//new GameObject("Musicagobj")
            SomMusicaSource = SomMusicaGobj.GetComponent<AudioSource>();
            SomMusicaGobj.transform.SetParent(this.transform);
        }
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);//pega o caminho da cena na pasta de arquivos
        string cenaAtualNome = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        if (SonsDicionario.ContainsKey("Musica" + cenaAtualNome))
        {
            SonsDicionario[cenaAtualNome].Loop = true;
            //SonsDicionario[cenaAtualNome].tipoSom = TipoSom.Musica;
            SomMusicaSource.volume = SonsDicionario[cenaAtualNome].Volume;
            SomMusicaSource.pitch = SonsDicionario[cenaAtualNome].Pitch;
            SomMusicaSource.loop = SonsDicionario[cenaAtualNome].Loop;
            SomMusicaSource.clip = SonsDicionario[cenaAtualNome].ArquivosDESom[0];
            if (!SomMusicaSource.isPlaying)
                SomMusicaSource.Play();
        }
    }
    public void PararEfeitosSonoros()
    {
        SomEfeitosSource.Stop();
    }
    //private string NomeFasePorBuildIndex(int index)
    //{
    //    string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(index);//pega o caminho da cena na pasta de arquivos
    //    string cena = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
    //    return cena;
    //}
    [System.Serializable]
    public class SomConfig
    {
        public string nomeElemento;
        public Som som;
        //public TipoSom tipoSom;
        public AudioClip[] ArquivosDESom;
        [Range(0f, 1f)]
        public float Volume;
        [Range(0.01f, 3f)]
        public float Pitch;
        public bool Loop;
        public float intervaloEntreSons;
    }
}
