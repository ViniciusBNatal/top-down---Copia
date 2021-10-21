using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineBehaviour : MonoBehaviour
{
    private CinemachineVirtualCamera CM;
    // Start is called before the first frame update
    void Start()
    {
        CM = this.GetComponent<CinemachineVirtualCamera>();
    }
    public void MudaFocoCamera(Transform obj)
    {
        CM.LookAt = obj;
        CM.Follow = obj;
    }
}
