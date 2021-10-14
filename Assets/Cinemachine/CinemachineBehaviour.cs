using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera CM;
    public Transform centroBase;
    public Transform jogador;
    // Start is called before the first frame update
    void Start()
    {
        CM = this.GetComponent<CinemachineVirtualCamera>();
    }
    public void MudaFocoCamera(int i)
    {
        switch (i)
        {
            case 1:
                CM.LookAt = jogador;
                CM.Follow = jogador;
                break;
            case 2:
                CM.LookAt = centroBase;
                CM.Follow = centroBase;
                break;
        }
    }
}
