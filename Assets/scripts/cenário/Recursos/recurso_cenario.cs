using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso_cenario : MonoBehaviour, CentroDeRecurso
{
    [Header("Componentes do recurso dropado")]
    [SerializeField] private Item item;
    [SerializeField] private GameObject recursoColetavelPreFab;
    [Header("Valores numéricos de centro de recursos")]
    [SerializeField] private Sprite iconeCentroDeRecursos;
    [SerializeField] private int quantasVezesPodeSerExtraida;
    [SerializeField] private int qntdDoRecursoDropado = 1;
    [SerializeField] private float forca;
    [SerializeField] private float tempoAteProximaColeta;
    private int vezesExtraida;
    [Header("Componentes de centro de spawn")]
    [SerializeField] private bool centroDeInimigos;
    [SerializeField] private Sprite iconeCentroDeInimigos;
    [SerializeField] private GameObject inimigoPrefab;
    [SerializeField] private int qntdMaximaDeInimigos;
    [SerializeField] private float intervaloEntreSpawns;
    [SerializeField] private int VidaMaxDoCentroDeSpawn;
    private int VidaAtualDoCentroDeSpawn;
    private int qntdInimigosAtuais = 0;
    private bool spawnandoInimigos = false;
    private SpriteRenderer SpriteDoObj;
    private bool aplicandoDano;

    private void Start()
    {
        SpriteDoObj = GetComponent<SpriteRenderer>();
        VidaAtualDoCentroDeSpawn = VidaMaxDoCentroDeSpawn;
        if (centroDeInimigos)
        {
            DefineSprite(iconeCentroDeInimigos);
            StartCoroutine(this.SpawnInimigos());
        }
        else
        {
            DefineSprite(iconeCentroDeRecursos);
        }
    }
    public void RecebeuHit()
    {
        if (centroDeInimigos)
        {
            AplicarDano();
        }
        else
        {
            CriaRecurso();
        }
    }
    IEnumerator RecursoCooldown()
    {
        yield return new WaitForSeconds(tempoAteProximaColeta);
        vezesExtraida = 0;
    }
    private void CriaRecurso()
    {
        if (vezesExtraida < quantasVezesPodeSerExtraida)
        {
            float localDeDropX = Random.Range(-1f, 1f);
            if (localDeDropX == 0)
                localDeDropX = 1;
            float localDeDropY = Random.Range(-1f, 1f);
            if (localDeDropY == 0)
                localDeDropY = 1;
            //GameObject recurso = Instantiate(recursoColetavelPreFab, new Vector3(transform.position.x + localDeDropX, transform.position.y + localDeDropY, 0), Quaternion.identity);
            GameObject recurso = Instantiate(recursoColetavelPreFab, transform.position, Quaternion.identity);
            recurso.GetComponent<recurso_coletavel>().item = item;
            recurso.GetComponent<recurso_coletavel>().qntd = qntdDoRecursoDropado;
            recurso.GetComponent<Rigidbody2D>().AddForce(new Vector2(localDeDropX, localDeDropY).normalized * forca);
            vezesExtraida++;
            if (vezesExtraida == quantasVezesPodeSerExtraida)
            {
                StartCoroutine(this.RecursoCooldown());
            }
        }
    }
    IEnumerator SpawnInimigos()
    {
        if (spawnandoInimigos == false)
        {
            spawnandoInimigos = true;
            while(qntdInimigosAtuais < qntdMaximaDeInimigos)
            {
                GameObject inimigo = Instantiate(inimigoPrefab, transform.position, Quaternion.identity);
                inimigo.GetComponent<inimigoScript>().SetCentroDeSpawn(this.gameObject.GetComponent<recurso_cenario>());
                qntdInimigosAtuais++;
                yield return new WaitForSeconds(intervaloEntreSpawns);
            }
            spawnandoInimigos = false;
        }
    }
    public void InimigoDerrotado()
    {
        qntdInimigosAtuais--;
        if (centroDeInimigos)
            StartCoroutine(this.SpawnInimigos());
    }
    private void AplicarDano()
    {
        if (!aplicandoDano)
        {
            aplicandoDano = true;
            VidaAtualDoCentroDeSpawn--;
            if (VidaAtualDoCentroDeSpawn <= VidaMaxDoCentroDeSpawn)
            {
                centroDeInimigos = false;
                StopAllCoroutines();
                DefineSprite(iconeCentroDeRecursos);
            }
            aplicandoDano = false;
        }
    }
    private void DefineSprite(Sprite sprite)
    {
        SpriteDoObj.sprite = sprite;
    }
}
