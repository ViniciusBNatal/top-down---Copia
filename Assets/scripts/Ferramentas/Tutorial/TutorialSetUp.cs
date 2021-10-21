using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSetUp : MonoBehaviour
{
    public static TutorialSetUp Instance { get; private set; }
    [SerializeField] private List<Dialogo> dialogosDoTutorial = new List<Dialogo>();
    private int sequenciaDialogos = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void SetupInicialJogador()
    {
        desastreManager.Instance.qntdDeDesastresParaOcorrer = 1;
        desastreManager.Instance.desastresSorteados[0] = "TERREMOTO";
        desastreManager.Instance.forcasSorteados[0] = 1;
        desastreManager.Instance.PreenchePlaca();
        jogadorScript.Instance.MudarEstadoJogador(1);
        //JogadorAnimScript.Instance.Levantar(true);
    }
    public void AoTerminoDoDialogoFocarCameraNoJogador()
    {
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform);
    }
    public void AoTerminoDoDialogoInstaladoOModuloDeDefesa()
    {
        //Debug.Log("devo aparecer 1 vez");
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloDuranteTutorial, 0f);
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
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.intervaloEntreOsDesastres, 0f);
        StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
    }
}
