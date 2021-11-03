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
        animator.SetFloat("MOVHORIZONTAL", inimigoScript.direcaoDeMovimentacao.x);
        animator.SetFloat("MOVVERTICAL", inimigoScript.direcaoDeMovimentacao.y);
    }
    public void AtaqueMelee()
    {
        animator.SetTrigger("ATQMELEE");
    }
    public void AtaqueRanged(Vector2 direcaoDisparo)
    {
        animator.SetTrigger("ATQRANGE");
        animator.SetFloat("RANGEHORZ", direcaoDisparo.x);
        animator.SetFloat("RANGEVERT", direcaoDisparo.y);
    }
}
