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
    private Animator animator;
    private Queue<string> Frases = new Queue<string>();
    private int index = 0;
    private Dialogo dialogoAtual;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void IniciarDialogo(Dialogo dialogo)
    {
        Frases.Clear();
        jogadorScript.Instance.MudarEstadoJogador(3);
        dialogoAtual = dialogo;
        TrocaImagemDoNPC();
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
        if (Frases.Count == 0)
        {
            FimDialogo();
            return;
        }
        string textoDialogo = Frases.Dequeue();
        TrocarNomeNPC();
        TrocaImagemDoNPC();
        TrocaEstadoImagemDoNPC();
        TrocarFocoDaCamera();
        AcionarEventosDuranteDialogo();
        index++;
        StopAllCoroutines();
        StartCoroutine(this.EscreveDialogo(textoDialogo));
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
    }
    protected virtual void AoFinalizarDialogo()
    {
        if(DialogoFinalizado != null)
        {
            DialogoFinalizado(this, System.EventArgs.Empty);
        }
        if (dialogoAtual.FocarComCamera.Length > 0)
            DialogoFinalizado -= jogadorScript.Instance.AoFinalizarDialogo;
    }
    public void LimparListaDeAoFinalizarDialogo()
    {
        DialogoFinalizado = delegate { };
    }
    private void RetornarCameraAoJogadorNoFinalDoDialogo()
    {
        if (DialogoFinalizado != jogadorScript.Instance.AoFinalizarDialogo)
            DialogoFinalizado += jogadorScript.Instance.AoFinalizarDialogo;
    }
    public void TrocarAnimator(Animator anim)//todos precisam trocar com a float ESTADO
    {
        animator = anim;
    }
    private void TrocaImagemDoNPC()
    {
        if (dialogoAtual.IDdoNPC.Length != 0)
        {
            if (dialogoAtual.IDdoNPC.Length == dialogoAtual.Frases.Length)
                animatorImage.SetInteger("NPC", dialogoAtual.IDdoNPC[index]);
            else
                animatorImage.SetInteger("NPC", dialogoAtual.IDdoNPC[0]);
        }
        else
            Debug.LogError("Dialogo está sem um NPC NA IMAGEM");
    }
    private void TrocaEstadoImagemDoNPC()
    {
        if (dialogoAtual.EstadoImagemNPC.Length == dialogoAtual.Frases.Length)
            animatorImage.SetFloat("ESTADO", dialogoAtual.EstadoImagemNPC[index]);
        else
            animatorImage.SetFloat("ESTADO", 1f);
    }
    private void TrocarNomeNPC()
    {
        if (dialogoAtual.NomeNPC.Length != 0)
        {
            if (dialogoAtual.NomeNPC.Length == dialogoAtual.Frases.Length)
                NomeNPCText.text = dialogoAtual.NomeNPC[index];
        }
        else
            Debug.LogError("Dialogo está sem um NOME para o NPC");
    }
    private void AcionarEventosDuranteDialogo()
    {
        if (dialogoAtual.EventosDuranteDialogo.Length == dialogoAtual.Frases.Length && dialogoAtual.EventosDuranteDialogo[index] != null)
            dialogoAtual.EventosDuranteDialogo[index].Invoke();
    }
    private void TrocarFocoDaCamera()
    {
        if (dialogoAtual.FocarComCamera.Length == dialogoAtual.Frases.Length)
        {
            RetornarCameraAoJogadorNoFinalDoDialogo();
            if (dialogoAtual.FocarComCamera[index] != null)
                jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(dialogoAtual.FocarComCamera[index]);
        }
    }
}
