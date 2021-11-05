using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class PostProcessScript : MonoBehaviour
{
    private Volume volume;
    private ChromaticAberration CA;
    private Vignette vignette;
    private FilmGrain grain;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out grain);
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out CA);
    }
    public void visualVirus(bool liga_desliga)
    {
        CA.active = liga_desliga;
        grain.active = liga_desliga;
    }
    public void visualErrupcaoTerrena(bool liga_desliga)
    {
        vignette.active = liga_desliga;
    }
}
