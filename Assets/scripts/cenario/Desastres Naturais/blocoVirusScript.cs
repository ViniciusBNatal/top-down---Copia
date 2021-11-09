using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blocoVirusScript : MonoBehaviour
{
    [SerializeField] private float chanceDeMultiplicar;
    [SerializeField] private int tentativasDeMultiplicar;
    [SerializeField] private float intervaloEntreMultiplicacao;
    [SerializeField] private float tempoAteDestruir;
    [SerializeField] private LayerMask blocoVirusMascara;
    [SerializeField] private Vector3 diferenca;
    private BoxCollider2D tamanho;
    private GameObject blocoVirus;
    private float largura;
    private float altura;
    private int direcao;
    // Start is called before the first frame update
    void Start()
    {
        blocoVirus = this.gameObject;
        tamanho = GetComponent<BoxCollider2D>();
        desastreManager.Instance.AdionarVirusALista(this.gameObject);
        StartCoroutine(this.DestruirObjeto());
        StartCoroutine(this.Multiplicar());
    }
    IEnumerator Multiplicar()
    {
        while (tentativasDeMultiplicar > 0)
        {
            largura = 0;
            altura = 0;
            yield return new WaitForSeconds(intervaloEntreMultiplicacao);
            tentativasDeMultiplicar--;
            float rand = Random.Range(1, 101);
            if (rand <= chanceDeMultiplicar)
            {
                int r = Random.Range(0, 2);
                switch (r)//dependendo do número direciona a criação do bloco para ser em X ou em Y
                {
                    case 0://X
                        largura = tamanho.size.x;
                        break;
                    case 1://Y
                        altura = tamanho.size.y;
                        break;
                }
                direcao = Random.Range(-1, 2);
                if (direcao == 0)
                    direcao = 1;
                Collider2D[] obj = Physics2D.OverlapBoxAll(transform.position + new Vector3(largura, altura, 0f) * direcao, new Vector2(tamanho.size.x - diferenca.x, tamanho.size.y - diferenca.y), 0f, blocoVirusMascara);
                if (obj.Length == 0)
                {
                    GameObject parede = Instantiate(blocoVirus,transform.position + new Vector3(largura, altura, 0f) * direcao, Quaternion.identity);
                }
            }
        }
    }
    IEnumerator DestruirObjeto()
    {
        yield return new WaitForSeconds(tempoAteDestruir);
        desastreManager.Instance.RemoverVirusDaLista(this.gameObject);
        Destroy(this.gameObject);
    }
    //private void OnDrawGizmosSelected()
    //{
    //    largura = this.transform.localScale.x;
    //    direcao = 1;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position + new Vector3(largura, altura, 0f) * direcao, new Vector2(this.transform.localScale.x - diferenca.x, this.transform.localScale.y - diferenca.y));
    //}
}
