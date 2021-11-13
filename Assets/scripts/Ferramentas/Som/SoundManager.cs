using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private static Dictionary<TipoSom, float> intervalosDosSons = new Dictionary<TipoSom, float>();
    private static Dictionary<TipoSom, float> ultimaVezTocado = new Dictionary<TipoSom, float>();
    private static GameObject SomGobj;
    private static AudioSource SomSource;
    public SomConfig[] Sons;
    public enum TipoSom
    {
        MusicaFase1,
        MusicaFase2,
        JogadorAndando,
        JogadorAtirando,
        JogadorAtqMelee
    };
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetupAudios();
        }
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
                intervalosDosSons.Add(somcnf.tipoSom, somcnf.intervaloEntreSons);
                ultimaVezTocado.Add(somcnf.tipoSom, 0f);
            }
        }
    }
    public void TocarSom(TipoSom tipoDoSom)
    {
        if (PodeTocarSom(tipoDoSom))
        {
            if (SomGobj == null)
            {
                SomGobj = new GameObject("Somgobj");
                SomSource = SomGobj.AddComponent<AudioSource>();
            }
            SomConfig somEscolhido = PegarSom(tipoDoSom);
            SomSource.volume = somEscolhido.Volume;
            SomSource.pitch = somEscolhido.Pitch;
            SomSource.loop = somEscolhido.Loop;
            SomSource.PlayOneShot(somEscolhido.ArquivosDESom[(int)Random.Range(0, somEscolhido.ArquivosDESom.Length)]);
        }
        //switch (tipoDoSom)
        //{
        //    default:
        //        SomSource.Play()
        //
        //        break;
        //}
    }
    public SomConfig PegarSom(TipoSom tipoSom)
    {
        foreach (SomConfig somcnf in Sons)
        {
            if (somcnf.tipoSom == tipoSom)
                return somcnf;
        }
        Debug.LogError("Som" + tipoSom + "Não encontrado!");
        return null;
    }
    public bool PodeTocarSom(TipoSom tipoSom)
    {
        foreach (SomConfig somcnf in Sons)
        {
            if (somcnf.tipoSom == tipoSom && intervalosDosSons.ContainsKey(somcnf.tipoSom))
            {
                float intervalo = intervalosDosSons[somcnf.tipoSom];
                float ultimaVez = ultimaVezTocado[somcnf.tipoSom];
                if (ultimaVez + intervalo < Time.time)
                {
                    ultimaVezTocado[somcnf.tipoSom] = Time.time;
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

    [System.Serializable]
    public class SomConfig
    {
        public TipoSom tipoSom;
        public AudioClip[] ArquivosDESom;
        public float Volume;
        public float Pitch;
        public bool Loop;
        public float intervaloEntreSons;
    }
}
