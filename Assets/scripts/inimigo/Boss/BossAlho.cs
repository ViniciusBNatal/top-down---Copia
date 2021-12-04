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
    [SerializeField] private Missao missaoMatarBoss;
    [SerializeField] private List<Dialogo> dialogos = new List<Dialogo>();
    private bool atacar = true;
    private Animator animator;
    private List<GameObject> ataquesEmCena = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        IndicadorDosDesastres.Instance.LimpaPlaca();
        desastreManager.Instance.SetUpParaNovoSorteioDeDesastres();
        desastreManager.Instance.Ativar_desativarInteracoesDaBase(false, true);
        DialogeManager.Instance.DialogoFinalizado += AoFinalizarDialogo;
        DialogeManager.Instance.IniciarDialogo(dialogos[0]);
        //dialogos.AtivarDialogo();
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
        ataquesEmCena.Add(Instantiate(misselPrefab, jogadorScript.Instance.transform.position, Quaternion.identity));
    }
    public void AtaqueDeSementeDeAlho()
    {
        ataquesEmCena.Add(Instantiate(sementeDeAlhoPrefab, pontoDeSpawnDaSemente.transform.position, Quaternion.identity));
    }
    private void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        UIinventario.Instance.craftingBossFinal.SetActive(true);
        MissoesManager.Instance.AdicionarMissao(missaoMatarBoss);
        desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres(), 0f, true);
        desastreManager.Instance.IniciarCorrotinaLogicaDesastres(true);
        StartCoroutine(this.PadraoDeAtaque());
    }
    public void BossDerotado()
    {
        atacar = false;
        MissoesManager.Instance.ConcluirMissao(missaoMatarBoss);
        desastreManager.Instance.PararTodasCorotinas();
        jogadorScript.Instance.MudarEstadoJogador(1);
        LimpaAtqsDaCena();
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(this.transform, 0f);
        StartCoroutine(this.tempo());
    }
    IEnumerator tempo()
    {
        yield return new WaitForSeconds(3f);
        jogadorScript.Instance.comportamentoCamera.MudaFocoCamera(jogadorScript.Instance.transform, 0f);
        InterfaceMenu.Instance.AbrirVitoria();
        //UIinventario.Instance.AbrirVitoria();
        //Application.Quit();
        Destroy(this.gameObject);
    }
    public float GetReducaoIntervaloDesastres()
    {
        return ReducaoIntervaloDesastres;
    }
    public void RemoverAtqDaLista(GameObject gobj)
    {
        ataquesEmCena.Remove(gobj);
    }
    private void LimpaAtqsDaCena()
    {
        for (int i = ataquesEmCena.Count - 1; i >= 0; i--)
        {
            Destroy(ataquesEmCena[i]);
        }
        ataquesEmCena.Clear();
    }
}
