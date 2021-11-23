using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogeManager : MonoBehaviour
{
    public static DialogeManager Instance { get; private set; }
    public delegate void DialogeManagerEventHandler(object origem, System.EventArgs args);
    public event DialogeManagerEventHandler DialogoFinalizado;
    //[SerializeField] private Text NomeNPCText;
    //[SerializeField] private Text DialogoText;
    [SerializeField] private TMP_Text NomeNPCText;
    [SerializeField] private TMP_Text DialogoText;
    [SerializeField] private float velocidadeDasLetras;
    [SerializeField] private Animator animatorImage;
    [SerializeField] private GameObject setaContinuarDialogo;
    private Animator animator;
    private Queue<string> Frases = new Queue<string>();
    private int index = 0;
    private Dialogo dialogoAtual;
    private Coroutine escrevendo = null;
    private string textoDialogo;
    [HideInInspector]
    public bool limparDelegate = true;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
    }
    public void IniciarDialogo(Dialogo dialogo)
    {
        Frases.Clear();
        jogadorScript.Instance.MudarEstadoJogador(3);
        dialogoAtual = dialogo;
        TrocaNPC();
        animator.SetBool("aberto", true);
        TrocarNomeNPC();
        foreach(string frase in dialogo.Frases)
        {
            Frases.Enqueue(frase);
        }
        MostraProximoDialogo();
    } 
    public void MostraProximoDialogo()
    {
        if (escrevendo == null)
        {
            if (Frases.Count == 0)
            {
                FimDialogo();
                return;
            }
            setaContinuarDialogo.SetActive(false);
            textoDialogo = Frases.Dequeue();
            TrocarNomeNPC();
            TrocaNPC();
            TrocaEstadoImagemDoNPC();
            TrocarFocoDaCamera();
            AcionarEventosDuranteDialogo();
            index++;
            escrevendo = StartCoroutine(this.EscreveDialogo(textoDialogo));
        }
        else
        {
            StopCoroutine(escrevendo);
            DialogoText.text = "";
            DialogoText.text = textoDialogo;
            setaContinuarDialogo.SetActive(true);
            escrevendo = null;
        }
    }
    public void FimDialogo()
    {
        animator.SetBool("aberto", false);
        index = 0;
        jogadorScript.Instance.MudarEstadoJogador(0);
        AoFinalizarDialogo();
        LimparListaDeAoFinalizarDialogo();
    }
    IEnumerator EscreveDialogo(string frase)
    {
        DialogoText.text = "";
        foreach(char letra in frase.ToCharArray())
        {
            DialogoText.text += letra;
            yield return new WaitForSeconds(velocidadeDasLetras);
        }
        setaContinuarDialogo.SetActive(true);
        escrevendo = null;
    }
    protected virtual void AoFinalizarDialogo()
    {
        if(DialogoFinalizado != null)
        {
            DialogoFinalizado(this, System.EventArgs.Empty);
        }
        if (dialogoAtual.FocarComCamera.Length > 0)
            DialogoFinalizado -= CinemachineBehaviour.Instance.AoFinalizarDialogo;
    }
    public void LimparListaDeAoFinalizarDialogo()
    {
        if (limparDelegate)
            DialogoFinalizado = delegate { };
    }
    private void RetornarCameraAoJogadorNoFinalDoDialogo()
    {
        if (DialogoFinalizado != CinemachineBehaviour.Instance.AoFinalizarDialogo)
            DialogoFinalizado += CinemachineBehaviour.Instance.AoFinalizarDialogo;
        //if (DialogoFinalizado != jogadorScript.Instance.AoFinalizarDialogo)
        //    DialogoFinalizado += jogadorScript.Instance.AoFinalizarDialogo;
    }
    private void TrocaNPC()
    {
        if (dialogoAtual.IDdoNPC.Length != 0)
        {
            if (index <= dialogoAtual.IDdoNPC.Length - 1)
                animatorImage.SetInteger("NPC", dialogoAtual.IDdoNPC[index]);
            else
                animatorImage.SetInteger("NPC", dialogoAtual.IDdoNPC[0]);
        }
        else
            Debug.LogError("Dialogo está sem um NPC NA IMAGEM");
    }
    private void TrocaEstadoImagemDoNPC()
    {
        if (dialogoAtual.EstadoImagemNPC.Length != 0)
        {
            if (index <= dialogoAtual.EstadoImagemNPC.Length - 1)
                animatorImage.SetFloat("ESTADO", dialogoAtual.EstadoImagemNPC[index]);
        }
        else
            animatorImage.SetFloat("ESTADO", 1f);
    }
    private void TrocarNomeNPC()
    {
        if (dialogoAtual.NomeNPC.Length != 0)
        {
            if (index <= dialogoAtual.NomeNPC.Length - 1)
                NomeNPCText.text = dialogoAtual.NomeNPC[index];
        }
        else
            Debug.LogError("Dialogo está sem um NOME para o NPC");
    }
    private void AcionarEventosDuranteDialogo()
    {
        if (index <= dialogoAtual.EventosDuranteDialogo.Length - 1)
            dialogoAtual.EventosDuranteDialogo[index].Invoke();
    }
    private void TrocarFocoDaCamera()
    {
        if (index <= dialogoAtual.FocarComCamera.Length - 1)
        {
            RetornarCameraAoJogadorNoFinalDoDialogo();
            if (dialogoAtual.FocarComCamera[index] != null)
                jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(dialogoAtual.FocarComCamera[index], 0f);
        }
    }
    public bool GetEstadoDialogo()
    {
        return animator.GetBool("aberto");
    }
}
