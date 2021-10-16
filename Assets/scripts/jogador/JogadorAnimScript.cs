using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorAnimScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movimento;
    [SerializeField] private jogadorScript jogador;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jogador.GetPodeAnimar())
        {
            movimento.x = Input.GetAxisRaw("Horizontal");
            movimento.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("HORIZONTAL", movimento.x);
            animator.SetFloat("VERTICAL", movimento.y);
            animator.SetFloat("VELOCIDADE", movimento.sqrMagnitude);
        }
    }
    public void AnimarAtaqueMelee(float dirX, float dirY)
    {
        animator.SetFloat("HORZMELEE", dirX);
        animator.SetFloat("VERTMELEE", dirY);
        animator.SetTrigger("ATACANDO");
        animator.SetTrigger("MELEE");
    }
    public void AnimarDisparo(float dirX, float dirY)
    {
        animator.SetFloat("HORZDISPARO", dirX);
        animator.SetFloat("VERTDISPARO", dirY);
        animator.SetTrigger("ATACANDO");
        animator.SetTrigger("DISPARO");
    }
}
