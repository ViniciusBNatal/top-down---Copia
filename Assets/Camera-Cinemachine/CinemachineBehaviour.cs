using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera CM;
    private float visaoPadrao;
    // Start is called before the first frame update
    void Start()
    {
        CM = this.GetComponent<CinemachineVirtualCamera>();
        visaoPadrao = CM.m_Lens.OrthographicSize;
    }
    public void MudaFocoCamera(Transform obj, float visao)
    {
        CM = this.GetComponent<CinemachineVirtualCamera>();
        CM.LookAt = obj;
        CM.Follow = obj;
        if (visao > 0)
            CM.m_Lens.OrthographicSize = visao;
        else if (visao == 0)
            CM.m_Lens.OrthographicSize = visaoPadrao;
    }
    public Transform GetFocoDaCamera()
    {
        return CM.LookAt;
    }
}
