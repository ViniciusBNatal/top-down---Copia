using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessScript : MonoBehaviour
{
    private PostProcessVolume volume;
    private ChromaticAberration CA;
    private Vignette vignette;
    private Grain grain;
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out CA);
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out grain);
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
