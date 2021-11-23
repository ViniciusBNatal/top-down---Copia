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
    public enum Som
    {
        Menu,
        Tutorial,
        BaseJogador,
        Floresta,
        Cidade,
        Lixao,
        JogadorAndando,
        JogadorAtirando,
        JogadorAtqMelee
    };
    public enum TipoSom
    {
        Global,
        Local
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
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Musica();
    }
    private void OnLevelWasLoaded(int level)
    {
        Musica();
    }
    public void SetupAudios()
    {
        foreach(SomConfig somcnf in Sons)
        {
            //foreach(AudioClip somClip in somcnf.ArquivosDESom)
            //{
            //    somClip.volume = somcnf.Volume;
            //    somClip.pitch = somcnf.Pitch;
            //    somClip.loop = somcnf.Loop;
            //    somcnf.ArquivosDESom.
            //}
            if (somcnf.intervaloEntreSons > 0)
            {
                intervalosDosSons.Add(somcnf.som, somcnf.intervaloEntreSons);
                ultimaVezTocado.Add(somcnf.som, 0f);
            }
        }
    }
    public void TocarSom(Som tipoDoSom)
    {
        if (PodeTocarSom(tipoDoSom))
        {
            if (SomGobj == null)
            {
                SomGobj = Instantiate(SomEfeitosGobjPrefab, this.transform);//new GameObject("Somgobj")
                SomEfeitosSource = SomGobj.GetComponent<AudioSource>();
                SomGobj.transform.SetParent(this.transform);
            }
            SomConfig somEscolhido = PegarSom(tipoDoSom);
            SomEfeitosSource.volume = somEscolhido.Volume;
            SomEfeitosSource.pitch = somEscolhido.Pitch;
            SomEfeitosSource.loop = somEscolhido.Loop;
            switch (somEscolhido.tipoSom)
            {
                case TipoSom.Global:
                    SomEfeitosSource.clip = somEscolhido.ArquivosDESom[(int)Random.Range(0, somEscolhido.ArquivosDESom.Length)]; 
                    SomEfeitosSource.Play();
                    break;
                case TipoSom.Local:
                    SomEfeitosSource.PlayOneShot(somEscolhido.ArquivosDESom[(int)Random.Range(0, somEscolhido.ArquivosDESom.Length)]);
                    break;
            }
        }
        //switch (tipoDoSom)
        //{
        //    default:
        //        SomSource.Play()
        //
        //        break;
        //}
    }
    public SomConfig PegarSom(Som tipoSom)
    {
        foreach (SomConfig somcnf in Sons)
        {
            if (somcnf.som == tipoSom)
                return somcnf;
        }
        Debug.LogError("Som" + tipoSom + "Não encontrado!");
        return null;
    }
    public bool PodeTocarSom(Som tipoSom)
    {
        foreach (SomConfig somcnf in Sons)
        {
            if (somcnf.som == tipoSom && intervalosDosSons.ContainsKey(somcnf.som))
            {
                float intervalo = intervalosDosSons[somcnf.som];
                float ultimaVez = ultimaVezTocado[somcnf.som];
                if (ultimaVez + intervalo < Time.time)
                {
                    ultimaVezTocado[somcnf.som] = Time.time;
                    return true;
                }
                else
                    return false;
            }
            else
                return true;
        }
        Debug.LogError("Som" + tipoSom + "Não encontrado!");
        return false;
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
        foreach(SomConfig som in Sons)
        {
            if (som.som.ToString().ToUpper() == cenaAtualNome.ToUpper())
            {
                som.Loop = true;
                som.tipoSom = TipoSom.Global;
                SomMusicaSource.clip = som.ArquivosDESom[0];
                if (!SomMusicaSource.isPlaying)
                    SomMusicaSource.Play();
            }
        }
    }
    private string NomeFasePorBuildIndex(int index)
    {
        string CaminhoCena = SceneUtility.GetScenePathByBuildIndex(index);//pega o caminho da cena na pasta de arquivos
        string cena = CaminhoCena.Substring(0, CaminhoCena.Length - 6).Substring(CaminhoCena.LastIndexOf('/') + 1);//retira o .unity e começa do ultimo /+1 char para pegar o nome
        return cena;
    }
    [System.Serializable]
    public class SomConfig
    {
        public Som som;
        public TipoSom tipoSom;
        public AudioClip[] ArquivosDESom;
        [Range(0f, 1f)]
        public float Volume;
        [Range(0.01f, 3f)]
        public float Pitch;
        public bool Loop;
        public float intervaloEntreSons;
    }
}
