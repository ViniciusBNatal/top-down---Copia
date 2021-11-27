using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class custceneDestruicaoModulo : MonoBehaviour
{
    [HideInInspector] public static bool OcorrerEvento { get; set; }
    public void FimCutscene()
    {
        if (!OcorrerEvento && BaseScript.Instance.GetVidaAtual() > 0)
        {
            OcorrerEvento = true;
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
    public void SomModuloExplodindo()
    {
        //SoundManager.Instance.TocarSom(SoundManager.Som.ModuloExplodindo);
    }
}
