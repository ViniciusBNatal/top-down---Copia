using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
    [SerializeField] private Text NomeNPCText;
    [SerializeField] private Text DialogoText;
    [SerializeField] private float velocidadeDasLetras;
    [SerializeField] private Animator animatorImage;
    private Animator animator;
    public static DialogeManager Instance { get; private set; }
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
            index = 0;
            return;
        }
        string textoDialogo = Frases.Dequeue();
        animatorImage.SetFloat("ESTADO", dialogoAtual.EstadoImagemNPC[index]);
        index++;
        StopAllCoroutines();
        StartCoroutine(this.EscreveDialogo(textoDialogo));
    }
    private void FimDialogo()
    {
        animator.SetBool("aberto", false);
        jogadorScript.Instance.MudarEstadoJogador(0);
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
}
