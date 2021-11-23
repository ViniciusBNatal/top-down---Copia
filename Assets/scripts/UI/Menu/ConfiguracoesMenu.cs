using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfiguracoesMenu : MonoBehaviour
{
    public static ConfiguracoesMenu Instance { get; private set; }
    public AudioMixer audioMixer;
    [SerializeField] private Slider somGeral;
    [SerializeField] private Slider somMusica;
    [SerializeField] private Slider somEfeitos;
    private void Awake()
    {
        Instance = this;
    }
    private void OnLevelWasLoaded(int level)
    {
        float f;
        audioMixer.GetFloat("VolumeGeral", out f);
        somGeral.value = f;
        audioMixer.GetFloat("VolumeMusica", out f);
        somMusica.value = f;
        audioMixer.GetFloat("VolumeEfeitos", out f);
        somEfeitos.value = f;
    }
    public void SetVolumeGeral(float volume)
    {
        audioMixer.SetFloat("VolumeGeral", volume);
    }
    public void SetVolumeEfeitos(float volume)
    {
        audioMixer.SetFloat("VolumeEfeitos", volume);
    }
    public void SetVolumeMusica(float volume)
    {
        audioMixer.SetFloat("VolumeMusica", volume);
    }
}
