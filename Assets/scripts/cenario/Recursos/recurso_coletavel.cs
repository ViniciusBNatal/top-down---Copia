using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso_coletavel : MonoBehaviour, SalvamentoEntreCenas, TocarSom
{
    [SerializeField] private item item;
    [SerializeField] private int qntd;
    [SerializeField] private float tempoParaLiberarColeta;
    [SerializeField] private float ForcaMagnetismo;
    [SerializeField] private string ConteudoTextoFlutuante;
    [Range(0.05f, 1f)]
    [SerializeField] private float velocidadeAnimTextoFlutuante;
    [SerializeField] private BoxCollider2D areaMagnetismo;
    [SerializeField] private BoxCollider2D areaFisica;
    [SerializeField] private BoxCollider2D areaDetectaColeta;
    [SerializeField] private GameObject AnimacaoTextoColetaPrefab;
    private GameObject NPCRelacionado = null;
    private Rigidbody2D rb;
    private SpriteRenderer icone;
    private GameObject jogador = null;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        icone = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        icone.sprite = item.icone;
        if (areaMagnetismo.enabled == false)
            StartCoroutine(this.ligarColeta());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && jogador == null)
        {
            jogador = collision.gameObject;
            StartCoroutine(this.MagnetismoItem());
        }
    }
    IEnumerator MagnetismoItem()
    {
        areaFisica.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        while (jogador != null)
        {
            Vector2 direcao = jogador.transform.position - transform.position;
            rb.velocity = direcao.normalized * ForcaMagnetismo;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }
    public void ColetaItem()
    {
        TocarSom(SoundManager.Som.JogadorColetouItem, null);
        GameObject gobj = Instantiate(AnimacaoTextoColetaPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        AnimTextoFlutuanteScript script = gobj.GetComponentInChildren<AnimTextoFlutuanteScript>();
        script.SetVelocidadeAnim(velocidadeAnimTextoFlutuante);
        if (ConteudoTextoFlutuante == "")
            gobj.GetComponentInChildren<AnimTextoFlutuanteScript>().texto.text = "+" + qntd.ToString();
        else
            gobj.GetComponentInChildren<AnimTextoFlutuanteScript>().texto.text = ConteudoTextoFlutuante;
        gobj.transform.SetParent(null);
        jogador.GetComponent<jogadorScript>().InterfaceJogador.AtualizaInventarioUI(item, qntd);
        SalvarEstado();
        Destroy(this.gameObject);
    }
    public void DefineItem(item it)
    {
        item = it;
    }
    public item ReferenciaItem()
    {
        return item;
    }
    public void DefineQuantidadeItem(int q)
    {
        qntd = q;
    }
    public void LancaRecurso(float forca, float direcaoX, float direcaoY)
    {
        float localDeDropX;
        float localDeDropY;
        if (direcaoX != 0)        
            localDeDropX = direcaoX;        
        else
        {
            localDeDropX = Random.Range(-1f, 2f);
            if (localDeDropX == 0)
                localDeDropX = 1;
        }
        if (direcaoY != 0)
            localDeDropY = direcaoY;
        else
        {
            localDeDropY = Random.Range(-1f, 2f);
            if (localDeDropY == 0)
                localDeDropY = 1;
        }
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(localDeDropX, localDeDropY).normalized * forca);
    }
    IEnumerator ligarColeta()
    {
        yield return new WaitForSeconds(tempoParaLiberarColeta);
        areaMagnetismo.enabled = true;
        areaFisica.enabled = true;
        areaDetectaColeta.enabled = true;
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
        {
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
            if (NPCRelacionado != null)
                NPCRelacionado.GetComponent<NPCscript>().SalvarEstado();//salvar item necessário para quest
        }
    }
    public void AcaoSeEstadoJaModificado()
    {
        Destroy(this.gameObject);
    }
    public void SetNPC(GameObject npc)
    {
        NPCRelacionado = npc;
    }
    public void TocarSom(SoundManager.Som som, Transform origemSom)
    {
        SoundManager.Instance.TocarSom(som, origemSom);
    }
}
