using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSetUp : MonoBehaviour
{
    public static TutorialSetUp Instance { get; private set; }
    [SerializeField] private float intervaloDuranteTutorial;
    [SerializeField] private List<Dialogo> dialogosDoTutorial = new List<Dialogo>();
    private int sequenciaDialogos = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void SetupInicialJogador()
    {
        JogadorAnimScript.Instance.Levantar(true);
        desastreManager.Instance.DefinirQntdDeDesastresParaOcorrer(1);
        desastreManager.Instance.DefinirDesastre(0, "ERRUPCAO TERRENA");
        desastreManager.Instance.DefinirForcaParaDesastre(0, 1);
        desastreManager.Instance.PreenchePlaca();
        jogadorScript.Instance.MudarEstadoJogador(1);
    }
    public void AoTerminoDoDialogoFocarCameraNoJogador()
    {
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform);
    }
    public void AoTerminoDoDialogoInstaladoOModuloDeDefesa()
    {
        //Debug.Log("devo aparecer 1 vez");
        desastreManager.Instance.ConfigurarTimer(intervaloDuranteTutorial, 0f);
        StartCoroutine(desastreManager.Instance.LogicaDesastres(false));
    }
    public void IniciarDialogo()
    {
        jogadorScript.Instance.MudarEstadoJogador(3);
        DialogeManager.Instance.IniciarDialogo(dialogosDoTutorial[sequenciaDialogos]);
        sequenciaDialogos++;
    }
    public void AoTerminoDoDialogoTerminadoOPrimeiroDesastre()
    {
        BaseScript.Instance.Tutorial();// agr irá ativar a possibilidade de interagir com a maquina
        //desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloEntreOsDesastres, desastreManager.Instance.tempoAcumulado);
        //desastreManager.Instance.tempoAcumulado = 0f;
        //StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
    }
    public void AoTerminoDoDialogoReparadaAMaquinaDoTempo()
    {
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), 0f);
        StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
    }
}
