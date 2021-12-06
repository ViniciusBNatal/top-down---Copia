using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimTextoFlutuanteScript : MonoBehaviour
{
    public GameObject objPai;
    public TMP_Text texto;
    public void DestruirObjeto()
    {
        Destroy(objPai);
    }
}
