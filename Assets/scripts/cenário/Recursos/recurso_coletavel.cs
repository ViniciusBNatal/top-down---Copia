using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso_coletavel : MonoBehaviour
{
    public Item item;
    public int qntd;
    private SpriteRenderer icone;
    // Start is called before the first frame update
    void Start()
    {
        icone = GetComponent<SpriteRenderer>();
        icone.sprite = item.icone;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<jogadorScript>().inventario.AtualizaInventarioUI(item, qntd);
            Destroy(this.gameObject);
        }
    }
}
