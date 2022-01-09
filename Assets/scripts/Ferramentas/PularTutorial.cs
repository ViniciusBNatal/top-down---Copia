using UnityEngine;
using UnityEngine.SceneManagement;

public class PularTutorial : MonoBehaviour
{
    [SerializeField] private Missao missaoInicial;
    void Start()
    {
        MissoesManager.Instance.AdicionarMissao(missaoInicial);
        UIinventario.Instance.LiberarNovBtnDeTrocaDeTempo(UIinventario.Instance.listaSlotUpgradesBase[0], false);
        desastreManager.Instance.SortearDesastresGeral();
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
        desastreManager.Instance.IniciarCorrotinaLogicaDesastres();
        SceneManager.LoadScene("BaseJogador");
    }
}
