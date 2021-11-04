using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalvarEstadoDoObjeto : MonoBehaviour
{
    public static List<string> objs;
    public static List<bool> JaFoiUsado = new List<bool>();
    private static int VidaDaBase = -1;
    static Dictionary<string, SlotModulo> modulos = new Dictionary<string, SlotModulo>();
    static Dictionary<string, bool> estadoPortas = new Dictionary<string, bool>();
    static Dictionary<string, CentroDeRecursoInfinito> dadosCentroRecursos = new Dictionary<string, CentroDeRecursoInfinito>();
    static Dictionary<string, NPCscript> dadosNPCs = new Dictionary<string, NPCscript>();
    // Start is called before the first frame update
    void Start()
    {
        if (objs == null)
            objs = new List<string>();
        AdicionarALista(this.gameObject);
        if (objs.Count > 0)
        {
            for (int i = 0; i < JaFoiUsado.Count; i++)
            {
                if (objs[i] == gameObject.name)
                {
                    if (JaFoiUsado[i])
                        if (gameObject.GetComponent<SalvamentoEntreCenas>() != null)
                            gameObject.GetComponent<SalvamentoEntreCenas>().AcaoSeEstadoJaModificado();
                    break;
                }
            }
        }
    }
    public void AdicionarALista(GameObject obj)
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i] == gameObject.name)
            {
                return;
            }
        }
        objs.Add(obj.name);
        JaFoiUsado.Add(false);
    }
    public void SalvarSeJaFoiModificado()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            if (objs[i] == gameObject.name)
            {
                JaFoiUsado[i] = true;
                break;
            }
        }
    }
    public void Salvar_CarregarDadosDaBase(BaseScript baseScript, int acao)
    {
        switch (acao)
        {
            case 0:
                VidaDaBase = baseScript.GetVidaAtual();
                break;
            case 1:
                if (VidaDaBase != -1)
                    baseScript.SetVidaAtual(VidaDaBase);
                break;
        }    
    }
    public void Salvar_CarregarDadosDosModulos(SlotModulo moduloScript, int acao)
    {
        switch (acao)
        {
            case 0:
                PlayerPrefs.SetInt(moduloScript.gameObject.name, moduloScript.GetModulo());
                if (modulos.ContainsKey(moduloScript.gameObject.name))
                {
                    modulos[moduloScript.gameObject.name].SetModulo(moduloScript.GetModulo());
                    modulos[moduloScript.gameObject.name].SetNomeDesastre(moduloScript.GetNomeDesastre());
                    modulos[moduloScript.gameObject.name].SetValorResistencia(moduloScript.GetvalorResistencia());
                }
                else
                {
                    SlotModulo temp = new SlotModulo();
                    temp.SetModulo(moduloScript.GetModulo());
                    temp.SetNomeDesastre(moduloScript.GetNomeDesastre());
                    temp.SetValorResistencia(moduloScript.GetvalorResistencia());
                    modulos.Add(moduloScript.gameObject.name, temp);
                }
                break;
            case 1:
                //carrega
                if (modulos.ContainsKey(moduloScript.gameObject.name))
                {
                    moduloScript.SetModulo(modulos[moduloScript.gameObject.name].GetModulo());
                    moduloScript.SetNomeDesastre(modulos[moduloScript.gameObject.name].GetNomeDesastre());
                    moduloScript.SetValorResistencia(modulos[moduloScript.gameObject.name].GetvalorResistencia());
                    moduloScript.ConstruirModulo(moduloScript.GetvalorResistencia(), moduloScript.GetNomeDesastre(), moduloScript.GetModulo());
                }
                break;
        }
    }
    public void Salvar_CarregarDadosDasPortas(Porta portaScript, int acao)
    {
        switch (acao)
        {
            case 0:
                //salva
                if (estadoPortas.ContainsKey(portaScript.gameObject.name))
                {                    
                    estadoPortas[portaScript.gameObject.name] = portaScript.GetAberto_Fechado();
                }
                else
                {
                    estadoPortas.Add(portaScript.gameObject.name, portaScript.GetAberto_Fechado());
                }
                break;
            case 1:
                //carrega
                if (estadoPortas.ContainsKey(portaScript.gameObject.name))
                {
                    portaScript.SetAberto_Fechado(estadoPortas[portaScript.gameObject.name]);
                }
                break;
        }
    }
    public void Salvar_CarregarDadosDosCentrosDeRecursos(CentroDeRecursoInfinito centroScript, int acao)
    {
        switch (acao)
        {
            case 0:
                //salva
                if (dadosCentroRecursos.ContainsKey(centroScript.gameObject.name))
                {
                    dadosCentroRecursos[centroScript.gameObject.name].SetTempoRestanteCooldown(centroScript.GetTempoRestanteCooldown());
                    dadosCentroRecursos[centroScript.gameObject.name].SetCentroDeInimigos(centroScript.GetCentroDeInimigos());
                    dadosCentroRecursos[centroScript.gameObject.name].SetCentroDeInimigos(centroScript.GetCentroDeInimigos());
                    dadosCentroRecursos[centroScript.gameObject.name].SetVezesExtraida(centroScript.GetVezesExtraida());
                    dadosCentroRecursos[centroScript.gameObject.name].SetVidaAtual(centroScript.GetVidaAtual());
                }
                else
                {
                    CentroDeRecursoInfinito temp = new CentroDeRecursoInfinito();
                    temp.SetTempoRestanteCooldown(centroScript.GetTempoRestanteCooldown());
                    temp.SetCentroDeInimigos(centroScript.GetCentroDeInimigos());
                    temp.SetVezesExtraida(centroScript.GetVezesExtraida());
                    temp.SetVidaAtual(centroScript.GetVidaAtual());
                    dadosCentroRecursos.Add(centroScript.gameObject.name, temp);
                }
                break;
            case 1:
                //carrega
                if (dadosCentroRecursos.ContainsKey(centroScript.gameObject.name))
                {
                    centroScript.SetTempoRestanteCooldown(dadosCentroRecursos[centroScript.gameObject.name].GetTempoRestanteCooldown() - (Mathf.FloorToInt(Time.time - CooldownDosRecursosManager.Instance.TempoDeSaidaDaFase(SceneManager.GetActiveScene().buildIndex))));
                    centroScript.SetCentroDeInimigos(dadosCentroRecursos[centroScript.gameObject.name].GetCentroDeInimigos());
                    centroScript.SetVezesExtraida(dadosCentroRecursos[centroScript.gameObject.name].GetVezesExtraida());
                    centroScript.SetVidaAtual(dadosCentroRecursos[centroScript.gameObject.name].GetVidaAtual());
                }
                break;
        }
    }
    public void Salvar_CarregarDadosDosNPCs(NPCscript npcScript, int acao)
    {
        switch (acao)
        {
            case 0:
                //salva
                if (dadosNPCs.ContainsKey(npcScript.gameObject.name))
                {
                    dadosNPCs[npcScript.gameObject.name].SetObjDaMissao(npcScript.GetObjDaMissao());
                    dadosNPCs[npcScript.gameObject.name].SetMissaoCumprida(npcScript.GetMissaoCumprida());
                }
                else
                {
                    NPCscript temp = new NPCscript();
                    temp.SetObjDaMissao(npcScript.GetObjDaMissao());
                    temp.SetMissaoCumprida(npcScript.GetMissaoCumprida());
                    dadosNPCs.Add(npcScript.gameObject.name, temp);
                }
                break;
            case 1:
                //carrega
                if (dadosNPCs.ContainsKey(npcScript.gameObject.name))
                {
                    npcScript.SetObjDaMissao(dadosNPCs[npcScript.gameObject.name].GetObjDaMissao());
                    npcScript.SetMissaoCumprida(dadosNPCs[npcScript.gameObject.name].GetMissaoCumprida());
                }
                break;
        }
    }
}