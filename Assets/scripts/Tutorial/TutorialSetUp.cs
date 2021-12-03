using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSetUp : MonoBehaviour
{
    public static TutorialSetUp Instance { get; private set; }
    [SerializeField] private float intervaloDuranteTutorial;
    [SerializeField] private Transform pontoDeSpawnBonecoMovel;
    [SerializeField] private Transform pontoParaTerinamentoMelee;
    [SerializeField] private Transform pontoSpawnCaixaDeRecursos;
    //public Transform pontoDeCombateJogador;
    [SerializeField] private List<Dialogo> dialogosDoTutorial = new List<Dialogo>();
    private inimigoScript bonecoDeTreinamento;
    private int sequenciaDialogos = 0;
    public int hitsDeDisparosNoInimigo = 0;
    [HideInInspector] public int tirosAcertadosNoInimigo;
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
        IndicadorDosDesastres.Instance.PreenchePlaca();
        jogadorScript.Instance.MudarEstadoJogador(1);
    }
    public void AoTerminoDoDialogoInstaladoOModuloDeDefesa()
    {
        //Debug.Log("devo aparecer 1 vez");
        desastreManager.Instance.ConfigurarTimer(intervaloDuranteTutorial, 0f, true);
        desastreManager.Instance.IniciarCorrotinaLogicaDesastres(false);
        IndicadorDosDesastres.Instance.AtivarCheckDeModuloConstruido(1, "ERRUPCAO TERRENA", 1);
    }
    public void IniciarDialogo()
    {
        //jogadorScript.Instance.MudarEstadoJogador(3);
        DialogeManager.Instance.IniciarDialogo(dialogosDoTutorial[sequenciaDialogos]);
        sequenciaDialogos++;
    }
    public void AoTerminoDoDialogoTerminadoOPrimeiroDesastre()
    {
        BaseScript.Instance.Tutorial();// agr irá ativar a possibilidade de interagir com a maquina
    }
    public void AoAcertarDisparoNoInimigo()
    {
        jogadorScript.Instance.GetAnimacoesTutorial().GetComponent<Animator>().SetBool("DISP", false);
        bonecoDeTreinamento.tiposDeMovimentacao = inimigoScript.TiposDeMovimentacao.estatico;
        bonecoDeTreinamento.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        bonecoDeTreinamento.GetAnimScript().SetTipoBonecoDeTreino(false);
        bonecoDeTreinamento.transform.position = pontoParaTerinamentoMelee.position;
        IniciarDialogo();
    }
    public void AoEliminarOInimigoControlado()
    {
        jogadorScript.Instance.GetAnimacoesTutorial().GetComponent<Animator>().SetBool("MELEE", false);
        IniciarDialogo();
    }
    public void AoTerminoDoDialogoReparadaAMaquinaDoTempo()
    {
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), 0f, true);
        desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
    }
    public void CriarObjeto(GameObject obj)
    {
        if (obj.GetComponent<caixa_recursos>())
        {
            Instantiate(obj, pontoSpawnCaixaDeRecursos.position, Quaternion.identity);
        }
        else if (obj.GetComponent<inimigoScript>())
        {
            GameObject gobj = Instantiate(obj, pontoDeSpawnBonecoMovel.position, Quaternion.identity);
            bonecoDeTreinamento = gobj.GetComponent<inimigoScript>();
            bonecoDeTreinamento.GetAnimScript().SetTipoBonecoDeTreino(true);//todos boneco criados são móveis
        }
    }
    public void TeleportarJogador(Transform pontoDeTeleporte)
    {
        jogadorScript.Instance.transform.position = pontoDeTeleporte.position;
    }
    public int GetSequenciaDialogos()
    {
        return sequenciaDialogos;
    }
}
