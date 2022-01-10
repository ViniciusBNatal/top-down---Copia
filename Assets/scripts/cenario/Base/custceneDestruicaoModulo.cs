using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class custceneDestruicaoModulo : MonoBehaviour, TocarSom
{
    [HideInInspector] public static bool OcorreuCustcene { get; set; }
    public void FimCutscene()
    {
        if (!OcorreuCustcene && BaseScript.Instance.GetVidaAtual() > 0)
        {
            OcorreuCustcene = true;
            BaseScript.Instance.FimCutsceneSeConseguiuDefender();
            //IndicadorDosDesastres.Instance.LimpaPlaca();
            //desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
            //if (BossAlho.Instance == null)
            //{
            //    jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform, 0f);
            //    jogadorScript.Instance.MudarEstadoJogador(0);
            //}
            //desastreManager.Instance.Ativar_desativarInteracoesDaBase(true, true);
            //if (TutorialSetUp.Instance != null)
            //    BaseScript.Instance.Tutorial();
            //else
            //    BaseScript.Instance.RecomecarDesastres();
        }
    }
    public void TocarSom(SoundManager.Som som, Transform origemSom)
    {
        SoundManager.Instance.TocarSom(som, origemSom);
    }
    public void TocarSomPorAnimacao(SoundManager.Som som)
    {
        TocarSom(som, this.transform);
    }
}
