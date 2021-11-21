using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoAnimScript : MonoBehaviour
{
    private Animator animator;
    private inimigoScript inimigoScript;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        inimigoScript = GetComponent<inimigoScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!inimigoScript.GetParalisado())
        {
            animator.SetFloat("MOVHORIZONTAL", inimigoScript.direcaoDeMovimentacao.x);
            animator.SetFloat("MOVVERTICAL", inimigoScript.direcaoDeMovimentacao.y);
        }
    }
    public void AtaqueMelee()
    {
        animator.SetTrigger("ATQMELEE");
    }
    public void AtaqueRanged(float multiplicador)
    {
        animator.SetFloat("TXDISP", multiplicador);
        animator.SetBool("ATQRANGE", true);
    }
    public void Esconder()
    {
        animator.SetTrigger("ESCONDER");
    }
    public void Surgir(bool b)
    {
        animator.SetBool("SURGIR", b);
    }
    public void PararDisparos()
    {
        animator.SetBool("ATQRANGE", false);
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    public void SetDirecaoProjetil(Vector2 vec)
    {
        animator.SetFloat("RANGEHORZ", vec.x);
        animator.SetFloat("RANGEVERT", vec.y);
    }
}
