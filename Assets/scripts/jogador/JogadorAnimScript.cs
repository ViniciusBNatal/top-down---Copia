using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogadorAnimScript : MonoBehaviour
{
    public static JogadorAnimScript Instance { get; private set; }
    private Animator animator;
    private Vector2 movimento;
    [SerializeField] private jogadorScript jogador;
    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (jogador.GetPodeAnimar())
        {
            movimento.x = Input.GetAxisRaw("Horizontal");
            movimento.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("HORIZONTAL", movimento.x);
            animator.SetFloat("VERTICAL", movimento.y);
            animator.SetFloat("VELOCIDADE", movimento.sqrMagnitude);
            if (movimento.x != 0f)
            {
                animator.SetFloat("IDLEHORZ", movimento.x);
                animator.SetFloat("IDLEVERTC", 0f);
            }
            if (movimento.y != 0f)
            {
                animator.SetFloat("IDLEVERTC", movimento.y);
                animator.SetFloat("IDLEHORZ", 0f);
            }
        }
    }
    public void AnimarAtaqueMelee(float dirX, float dirY)
    {
        animator.SetFloat("HORZMELEE", dirX);
        animator.SetFloat("VERTMELEE", dirY);
        animator.SetTrigger("MELEE");
    }
    public void AnimarDisparo(float dirX, float dirY)
    {
        animator.SetFloat("HORZDISPARO", dirX);
        animator.SetFloat("VERTDISPARO", dirY);
        animator.SetTrigger("DISPARO");
    }
    public void Levantar(bool b)
    {
        animator.SetBool("CAIDO", b);
    }
    public void Hit(float multiplicador)
    {
        float f = Mathf.Pow(multiplicador, -1f);//se a duração do stun é de ,5 a anim deve tocar 2 vezes mais rápida
        animator.SetFloat("HITMULTI", f);
        animator.SetTrigger("HIT");
    }
    public void FimHit()
    {
        animator.SetFloat("HITMULTI", 1f);
    }
}
