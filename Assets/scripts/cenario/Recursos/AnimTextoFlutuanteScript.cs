using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimTextoFlutuanteScript : MonoBehaviour
{
    public GameObject objPai;
    public TMP_Text texto;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void DestruirObjeto()
    {
        Destroy(objPai);
    }
    public void SetVelocidadeAnim(float velocidade)
    {
        if (velocidade > 0)
            animator.SetFloat("Vel", velocidade);
        else
            animator.SetFloat("Vel", 1f);
    }
}
