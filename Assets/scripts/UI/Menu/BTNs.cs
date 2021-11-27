using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BTNs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textoBtn;
    [SerializeField] private Material matOutline;
    private void Awake()
    {
        textoBtn = GetComponentInChildren<TMP_Text>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //aplica o mat outline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //aplica o mat padrão;
    }
}
