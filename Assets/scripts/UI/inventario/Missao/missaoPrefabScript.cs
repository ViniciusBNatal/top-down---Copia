using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class missaoPrefabScript : MonoBehaviour
{
    public Image iconeStatusMssao;
    [SerializeField] private Sprite iconeMissaoConcluida;
    public TMP_Text textoResumoMissao;
    public TMP_Text textoDetalhesMissao;
    public RectTransform caixaIconeEResumoMissao;
    public Missao missaoScrObj;
    public Animator animCaixaMissao;
    public Animator animIcone;
    public Animator animResumoMissao;
    public Animator animDetalheMissao;
    public void EscreverMissao()
    {
        iconeStatusMssao.sprite = missaoScrObj.iconeMissao;
        textoResumoMissao.text = missaoScrObj.TextoResumoMissao;
        textoDetalhesMissao.text = missaoScrObj.TextoDetalheMissao;
    }
    public void ConcluirMissao()
    {
        iconeStatusMssao.sprite = iconeMissaoConcluida;
        animIcone.enabled = true;
    }
    public void RemoverMissao()
    {
        Destroy(MissoesManager.Instance.missoesAtivasMenu[missaoScrObj].gameObject);
        Destroy(MissoesManager.Instance.missoesAtivasTracker[missaoScrObj].gameObject);
        MissoesManager.Instance.posicaoMissoesNoTracker.Remove(this.gameObject);
        MissoesManager.Instance.missoesAtivasMenu.Remove(missaoScrObj);
        MissoesManager.Instance.missoesAtivasTracker.Remove(missaoScrObj);
        MissoesManager.Instance.ReposicionarMissoesNoTracker();
    }
}
