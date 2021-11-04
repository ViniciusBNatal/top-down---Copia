using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAlho : MonoBehaviour
{
    public static BossAlho Instance { get; private set; }
    [SerializeField] private float intervaloEntreAtaques;
    [SerializeField] private float ReducaoIntervaloDesastres;
    [SerializeField] private GameObject misselPrefab;
    [SerializeField] private GameObject sementeDeAlhoPrefab;
    [SerializeField] private Transform pontoDeSpawnDaSemente;
    [SerializeField] private DialogoTrigger dialogos;
    private bool atacar = true;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
        dialogos.AtivarDialogo();
    }
    IEnumerator PadraoDeAtaque()
    {
        while (atacar)
        {
            int r = Random.Range(0, 2);
            switch (r)
            {
                case 0:
                    animator.SetTrigger("MISSEL");
                    break;
                case 1:
                    animator.SetTrigger("SEMENTE");
                    break;
            }
            yield return new WaitForSeconds(intervaloEntreAtaques);
        }
    }
    public void AtaqueDeMissel()
    {
        Instantiate(misselPrefab, jogadorScript.Instance.transform.position, Quaternion.identity);
    }
    public void AtaqueDeSementeDeAlho()
    {
        Instantiate(sementeDeAlhoPrefab, pontoDeSpawnDaSemente.transform.position, Quaternion.identity);
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres() - ReducaoIntervaloDesastres, 0f);
        desastreManager.Instance.StartCoroutine(desastreManager.Instance.LogicaDesastres(true));
        StartCoroutine(this.PadraoDeAtaque());
        DialogeManager.Instance.LimparListaDeAoFinalizarDialogo();
    }
    public void BossDerotado()
    {
        atacar = false;
        jogadorScript.Instance.MudarEstadoJogador(1);
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(transform);
        StartCoroutine(this.tempo());
    }
    IEnumerator tempo()
    {
        yield return new WaitForSeconds(3f);
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform);
        Destroy(this.gameObject);
    }
    public float GetReducaoIntervaloDesastres()
    {
        return ReducaoIntervaloDesastres;
    }
}
