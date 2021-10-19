using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportador : MonoBehaviour
{
    [SerializeField] private int TeleportarParaFase;
    [SerializeField] private Transform pontoDeChegada;
    private Transform destino;
    private void Start()
    {
        UIinventario.Instance.AdicionaTeleportadorALista(this);
        destino = BaseScript.Instance.GetPosicao();
    }
    public void TeleportarPorInteracao()
    {
        jogadorScript.Instance.transform.position = destino.position;
    }
    public int GetFaseDoTeleportador()
    {
        return TeleportarParaFase;
    }
    public Transform GetPosicao()
    {
        return pontoDeChegada;
    }
}
