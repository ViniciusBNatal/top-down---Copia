using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso_coletavel : MonoBehaviour, SalvamentoEntreCenas
{
    [SerializeField] private Item item;
    [SerializeField] private int qntd;
    private Rigidbody2D rb;
    private SpriteRenderer icone;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        icone = GetComponent<SpriteRenderer>();
        //icone.sprite = item.icone;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<jogadorScript>().InterfaceJogador.AtualizaInventarioUI(item, qntd);
            SalvarEstado();
            Destroy(this.gameObject);
        }
    }
    public void DefineItem(Item it)
    {
        item = it;
        icone = GetComponent<SpriteRenderer>();
        icone.sprite = item.icone;
    }
    public Item ReferenciaItem()
    {
        return item;
    }
    public void DefineQuantidadeItem(int q)
    {
        qntd = q;
    }
    public void LancaRecurso(float forca)
    {
        float localDeDropX = Random.Range(-1, 1);
        if (localDeDropX == 0)
            localDeDropX = 1;
        float localDeDropY = Random.Range(-1, 1);
        if (localDeDropY == 0)
            localDeDropY = 1;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(localDeDropX, localDeDropY).normalized * forca);
    }
    public void SalvarEstado()
    {
        if (GetComponent<SalvarEstadoDoObjeto>() != null)
            GetComponent<SalvarEstadoDoObjeto>().SalvarSeJaFoiModificado();
    }
    public void AcaoSeEstadoJaModificado()
    {
        Destroy(this.gameObject);
    }
}
