using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ClickEmObjetos : MonoBehaviour
{
    [SerializeField] private Material materialOutline;
    private Material materialOriginal = null;
    private SpriteRenderer objAcertado;
    //void Click(jogadorScript jogador);
    //Todos Precisam ter uma lista com os materiais default e outline
    private void Update()
    {
        if (jogadorScript.Instance.GetEstadoAtualJogador() == jogadorScript.estados.EmContrucao)
        {
            Vector2 mousePos = jogadorScript.Instance.PegaPosicoMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                ClickInter clicavel = hit.collider.GetComponent<ClickInter>();
                if (clicavel != null)
                {
                    if (objAcertado != hit.collider.gameObject.GetComponent<SpriteRenderer>() && objAcertado != null)//verificar se agr é um obj diferente
                    {
                        //Debug.Log("outro clicavel");
                        objAcertado.material = materialOriginal;
                        materialOriginal = null;
                    }
                    objAcertado = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                    if (materialOriginal == null)
                        materialOriginal = objAcertado.material;
                    if (objAcertado.material != materialOutline)
                        objAcertado.material = materialOutline;
                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        objAcertado.material = materialOriginal;
                        clicavel.Acao(jogadorScript.Instance);
                    }
                }
                else
                {
                    if (objAcertado != null)
                    {
                        //Debug.Log("outro com colisao");
                        objAcertado.material = materialOriginal;
                        objAcertado = null;
                        materialOriginal = null;
                    }
                }
            }
            else
            {
                if (objAcertado != null)
                {
                    //Debug.Log("algo sem colisao");
                    objAcertado.material = materialOriginal;
                    objAcertado = null;
                    materialOriginal = null;
                }
            }
        }
    }

}
