using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
    public static DialogeManager Instance { get; private set; }
    public delegate void DialogeManagerEventHandler(object origem, System.EventArgs args);
    public event DialogeManagerEventHandler DialogoFinalizado;
    [SerializeField] private Text NomeNPCText;
    [SerializeField] private Text DialogoText;
    [SerializeField] private float velocidadeDasLetras;
    [SerializeField] private Animator animatorImage;
    private Animator animator;
    private Queue<string> Frases = new Queue<string>();
    private int index = 0;
    private Dialogo dialogoAtual;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void IniciarDialogo(Dialogo dialogo)
    {
        Frases.Clear();
        animator.SetBool("aberto", true);
        NomeNPCText.text = dialogo.NomeNPC;
        dialogoAtual = dialogo;
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
        if (dialogoAtual.EstadoImagemNPC.Length == dialogoAtual.Frases.Length)
            animatorImage.SetFloat("ESTADO", dialogoAtual.EstadoImagemNPC[index]);
        else
            animatorImage.SetFloat("ESTADO", 1f);
        if (dialogoAtual.FocarComCamera.Length == dialogoAtual.Frases.Length)
        {
            RetornarCameraAoJogadorNoFinalDoDialogo();
            if (dialogoAtual.FocarComCamera[index] != null)
                jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(dialogoAtual.FocarComCamera[index]);
        }
        index++;
        StopAllCoroutines();
        StartCoroutine(this.EscreveDialogo(textoDialogo));
    }
    private void FimDialogo()
    {
        animator.SetBool("aberto", false);
        index = 0;
        jogadorScript.Instance.MudarEstadoJogador(0);
        AoFinalizarDialogo();
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
}
