using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class PostProcessScript : MonoBehaviour
{
    public static PostProcessScript Instance { get; private set; }
    private Volume volume;
    private ChromaticAberration CA;
    private Vignette vignette;
    private FilmGrain grain;
    private Color corVignette;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out grain);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out CA);
        corVignette = ((Color)vignette.color);
    }
    public void visualVirus(bool liga_desliga)
    {
        CA.active = liga_desliga;
        grain.active = liga_desliga;
    }
    public void visualErrupcaoTerrena(bool liga_desliga)
    {
        vignette.color = new ColorParameter(corVignette);
        vignette.active = liga_desliga;
    }
    public void visualPredio(bool liga_desliga)
    {
        vignette.color = new ColorParameter(Color.black);
        vignette.active = liga_desliga;
    }
}
