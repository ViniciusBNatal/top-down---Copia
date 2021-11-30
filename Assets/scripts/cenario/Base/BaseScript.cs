using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour, AcoesNoTutorial, SalvamentoEntreCenas
{
    public static BaseScript Instance { get; private set; }
    [Header("COMPONENTES DA BASE")]
    [SerializeField] private Transform posicaoDeChegadaPorTeleporte;
    [SerializeField] private int vidaMax;
    [SerializeField] private GameObject areaDeInteracao;
    [SerializeField] private float intervaloDuranteADefesa;
    [SerializeField] private int QntdDeDefesasNecessarias;
    [SerializeField] private GameObject BossPrefab;
    [SerializeField] private TMP_Text vidaAtualText;
    [SerializeField] private Image VidaImagem;
    [SerializeField] private GameObject animNovoTempo;
    [SerializeField] private AnimationClip animDestruicaoModulo;
    private bool duranteMelhoria = false;
    private List<SlotModulo> listaModulos = new List<SlotModulo>();
    private int vidaAtual;
    private int DefesasOcorridasDuranteMelhoriaBase = 0;
    private int defesasContraDisastre;
    [HideInInspector] public Animator animator { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
        vidaAtual = vidaMax;
        vidaAtualText.text = vidaAtual.ToString();
    }
    private void Start()
    {
        jogadorScript.Instance.transform.position = posicaoDeChegadaPorTeleporte.position;
        //jogadorScript.Instance.Tutorial();
    }
    public void VerificarModulos()
    {
        defesasContraDisastre = 0;
        for (int a = 0; a < desastreManager.Instance.GetQntdDesastresParaOcorrer(); a++)
        {
            int forcaTotal = 0;
            bool encontrouDefesaDeMesmoValor = false;
            List<SlotModulo> ModulosDeMesmoDesastre = new List<SlotModulo>();
            List<SlotModulo> modulosParaContaDeForca = new List<SlotModulo>();
            for (int i = 0; i < listaModulos.Count; i++)
            {
                if (desastreManager.Instance.GetDesastreSorteado(a) == listaModulos[i].GetNomeDesastre())
                {
                    ModulosDeMesmoDesastre.Add(listaModulos[i]);
                }
            }
            foreach (SlotModulo mod in ModulosDeMesmoDesastre)
            {
                if (mod.GetvalorResistencia() >= desastreManager.Instance.GetForcaSorteada(a))
                {
                    mod.RemoverModulo();
                    defesasContraDisastre++;
                    encontrouDefesaDeMesmoValor = true;
                    break;
                }
                /*if(mod.GetvalorResistencia() == desastreManager.Instance.GetForcaSorteada(a)) // se quiser q subtraia das forças
                {
                    mod.RemoverModulo();
                    defendido++;
                    encontrouDefesaDeMesmoValor = true;
                    break;
                }
                else if (mod.GetvalorResistencia() > desastreManager.Instance.GetForcaSorteada(a))
                {
                    mod.SetValorResistencia(mod.GetvalorResistencia() - desastreManager.Instance.GetForcaSorteada(a));
                    mod.ConstruirModulo(mod.GetvalorResistencia(), mod.GetNomeDesastre(), mod.GetModulo());
                    break;
                }*/
                else
                {
                    modulosParaContaDeForca.Add(mod);
                }
            }
            if (!encontrouDefesaDeMesmoValor)
            {
                List<SlotModulo> modulosNaConta = new List<SlotModulo>();
                foreach (SlotModulo mod in modulosParaContaDeForca)
                {
                    modulosNaConta.Add(mod);
                    forcaTotal += mod.GetvalorResistencia();
                    if (forcaTotal >= desastreManager.Instance.GetForcaSorteada(a))
                    {
                        foreach (SlotModulo m in modulosNaConta)
                        {
                            m.RemoverModulo();
                        }
                        defesasContraDisastre++;
                        break;
                    }
                    /*if (forcaTotal == desastreManager.Instance.GetForcaSorteada(a)) // se quiser q subtraia das forças
                    {
                        foreach (SlotModulo m in modulosNaConta)
                        {
                            m.RemoverModulo();
                        }
                        defendido++;
                        break;
                    }
                    else if (forcaTotal > desastreManager.Instance.GetForcaSorteada(a))
                    {
                        for (int i = 0; i < modulosNaConta.Count - 1; i++)
                        {
                            modulosNaConta[i].RemoverModulo();
                        }
                        int ultimoIndex = modulosNaConta.Count - 1;
                        modulosNaConta[ultimoIndex].SetValorResistencia(modulosNaConta[ultimoIndex].GetvalorResistencia() - desastreManager.Instance.GetForcaSorteada(a));
                        modulosNaConta[ultimoIndex].ConstruirModulo(modulosNaConta[ultimoIndex].GetvalorResistencia(), modulosNaConta[ultimoIndex].GetNomeDesastre(), modulosNaConta[ultimoIndex].GetModulo());
                        defendido++;
                        break;
                    }*/
                }
            }
        }
        if (desastreManager.Instance.GetQntdDesastresParaOcorrer() == defesasContraDisastre)
        {
            if(TutorialSetUp.Instance == null)
                animator.SetTrigger("DEFENDIDO");
        }
        else
        {
            if (TutorialSetUp.Instance == null)
                animator.SetTrigger("HIT");
            //vidaAtual -= desastreManager.Instance.GetQntdDesastresParaOcorrer() - defesasContraDisastre;
            //vidaAtualText.text = vidaAtual.ToString();
            //if (vidaAtual <= 0)
            //{
            //    GameOver();
            //    Debug.Log("Perdeu");
            //}
            //else
            //{
                //if (TutorialSetUp.Instance == null)
                //    animator.SetTrigger("HIT");
                //if (defesasContraDisastre == 0)// caso não defendeu nada cancela a animação de destruição dos modulos
                //{
                //    desastreManager.Instance.encerramentoDesastres();
                //    jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform, 0f);
                //    jogadorScript.Instance.MudarEstadoJogador(0);
                //    RecomecarDesastres();
                //}
            //}
            //if (!tutorial)
            //{
            //for (int i = 0; i < desastreManager.Instance.GetQntdDesastresParaOcorrer() - defendido; i++)
            //{
            //    vidaAtual--;
            //    vidaAtualText.text = vidaAtual.ToString();
            //    if (vidaAtual <= 0)
            //    {
            //        GameOver();
            //        //Debug.Log("Perdeu");
            //    }
            //    
            //}
            //Debug.Log(vidaAtual);
            //
            //}
        }
    }
    public void MudancaVida()
    {
        vidaAtual -= desastreManager.Instance.GetQntdDesastresParaOcorrer() - defesasContraDisastre;
        VidaImagem.fillAmount -= (1f / vidaMax) * (desastreManager.Instance.GetQntdDesastresParaOcorrer() - defesasContraDisastre);
        vidaAtualText.text = vidaAtual.ToString();
        if (vidaAtual <= 0)
        {
            GameOver();
        }
        else
        {
            FimCustceneSeNaoConseguiuDefender();
        }
    }
    public void AbreEFechaMenuDeTrocaDeTempo()
    {
       if (duranteMelhoria)
       {
           Debug.Log("espere a defesa acabar para interagir");
       }
       else
       {
            if (animNovoTempo.activeSelf)
            {
                Ativa_DesativaAnimacaoDeNovoTempoLiberado(false);
            }
            if (!jogadorScript.Instance.InterfaceJogador.InventarioAberto)
            {
                jogadorScript.Instance.InterfaceJogador.abreMenuDeTempos();
            }
            else
            {
                jogadorScript.Instance.InterfaceJogador.fechaMenuDeTempos();
            }
       }
    } 
    public void AdicionarModulo(SlotModulo modulo)
    {
        listaModulos.Add(modulo);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
            {
                custceneDestruicaoModulo.OcorrerEvento = false;
                if (BossAlho.Instance == null)
                {
                    CutsceneDestruicaoDosModulos();
                }
                else
                {
                    VerificarModulos();
                    //Ativar_DesativarInteracao(true);
                    desastreManager.Instance.encerramentoDesastres();
                    IndicadorDosDesastres.Instance.LimpaPlaca();
                    desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
                    RecomecarDesastres();
                }
                //EncerrarDesastresEVerificarDefesa();
                //if (tutorial)
                //{
                //    Tutorial();
                //    return;
                //}
                //RecomecarDesastres();
            }
        }
    }
    public void Tutorial()
    {
        //DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
        IndicadorDosDesastres.Instance.LimpaPlaca();
        desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
        TutorialSetUp.Instance.IniciarDialogo();
        custceneDestruicaoModulo.OcorrerEvento = false;
    }
    /*private void EncerrarDesastresEVerificarDefesa()
    {
        if (BossAlho.Instance == null)
        {
            CutsceneDestruicaoDosModulos();
        }
        else
        {
            VerificarModulos();
            //Ativar_DesativarInteracao(true);
            desastreManager.Instance.encerramentoDesastres();
            IndicadorDosDesastres.Instance.LimpaPlaca();
            desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
        }
    }*/
    public void CutsceneDestruicaoDosModulos()
    {
        desastreManager.Instance.encerramentoDesastres();
        if (BossAlho.Instance == null)
        {
            jogadorScript.Instance.MudarEstadoJogador(1);
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(this.transform, UIinventario.Instance.zoomOutAoConstruir);
        }
        VerificarModulos();
    }
    private void FimCustcene()
    {
        if (BossAlho.Instance == null)//se o boss n existe tem custcene 
        {
            jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform, 0f);
            jogadorScript.Instance.MudarEstadoJogador(0);
            desastreManager.Instance.Ativar_desativarInteracoesDaBase(true, true);
        }
        else
            desastreManager.Instance.Ativar_desativarInteracoesDaBase(false, true);
        if (TutorialSetUp.Instance != null)
            Tutorial();
        else
            RecomecarDesastres();
    }
    public void FimCutsceneSeConseguiuDefender()
    {
        FimCustcene();
    }
    private void FimCustceneSeNaoConseguiuDefender()
    {
        if (defesasContraDisastre == 0)
            FimCustcene();
    }
    public void RecomecarDesastres()
    {
        IndicadorDosDesastres.Instance.LimpaPlaca();
        desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
        //custceneDestruicaoModulo.finalizandoCutscene = false;
        if (duranteMelhoria)
        {
            DefesasOcorridasDuranteMelhoriaBase++;
            if (DefesasOcorridasDuranteMelhoriaBase == QntdDeDefesasNecessarias)
            {
                duranteMelhoria = false;
                DefesasOcorridasDuranteMelhoriaBase = 0;
                if (UIinventario.Instance.VerificarSeLiberouBossFinal())//verificação para o boss
                {
                    //pode ter um dialogo aqui
                    Instantiate(BossPrefab, transform.position + new Vector3(0f, 14f, 0f), Quaternion.identity);
                    return;
                }
                desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
                Ativa_DesativaAnimacaoDeNovoTempoLiberado(true);
                //DesastresList.Instance.LiberarNovosDesastres(UIinventario.Instance.GetTempoAtual());//ativa a possibilidade do evento desse tempo acontecer
            }
            else
                desastreManager.Instance.ConfigurarTimer(intervaloDuranteADefesa, desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
        }
        else
        {
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), desastreManager.Instance.GetTempoAcumuladoParaDesastre(), true);
        }
        desastreManager.Instance.MudarTempoAcumuladoParaDesastre(0f);
        desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
    }
    public void Ativa_DesativaAnimacaoDeNovoTempoLiberado(bool b)
    {
        animNovoTempo.SetActive(b);
    }
    public void Ativar_DesativarInteracao(bool b)
    {
        areaDeInteracao.SetActive(b);
    }
    protected virtual void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        TutorialSetUp.Instance.AoTerminoDoDialogoTerminadoOPrimeiroDesastre();
    }
    public void CancelarInscricaoEmDialogoFinalizado()
    {
        DialogeManager.Instance.DialogoFinalizado -= AoFinalizarDialogo;
    }
    public void Ativar_DesativarVisualConstrucaoModulos(bool b)
    {
        for (int i = 0; i < listaModulos.Count; i++)
        {
            listaModulos[i].VisualConstrucao(b);
        }
    }
    public void Ativar_DesativarDuranteDefesaParaMelhorarBase(bool b)
    {
        duranteMelhoria = b;
    }
    public Transform GetPosicaoParaTeleporte()
    {
        return posicaoDeChegadaPorTeleporte;
    }
    public bool GetDuranteDefesaParaMelhorarBase()
    {
        return duranteMelhoria;
    }
    public float GetIntervaloDuranteOAprimoramentoDaBase()
    {
        return intervaloDuranteADefesa;
    }
    public void SalvarEstadosDosModulos()
    {
        for (int i = 0; i < listaModulos.Count; i++)
        {
            listaModulos[i].SalvarEstado();
        }
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDaBase(this, 0);
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        GetComponent<SalvarEstadoDoObjeto>().Salvar_CarregarDadosDaBase(this, 1);
    }
    public void SetVidaAtual(int i)
    {
        vidaAtual = i;
        vidaAtualText.text = vidaAtual.ToString();
        VidaImagem.fillAmount -= (1f / vidaMax) * (vidaMax - vidaAtual);
    }
    public int GetVidaAtual()
    {
        return vidaAtual;
    }
    private void GameOver()
    {
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(transform, 0f);
        if (desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
            desastreManager.Instance.encerramentoDesastres();
        desastreManager.Instance.PararTodasCorotinas();
        animator.SetTrigger("DESTRUIDO");
    }
    public void SomPortalDestruido()
    {
        //SoundManager.Instance.TocarSom(SoundManager.Som.BaseExplodindo);
    }
    public void AbrirGameOver()
    {
        InterfaceMenu.Instance.AbrirGameOver();
        //UIinventario.Instance.abrirAbaDeGameOver();
    }
}
