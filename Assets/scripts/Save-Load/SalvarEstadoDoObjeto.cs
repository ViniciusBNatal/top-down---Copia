using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SalvarEstadoDoObjeto : MonoBehaviour
{
    public static List<string> objs;
    public static List<bool> JaFoiUsado = new List<bool>();
    private static int VidaDaBase = -1;
    //Dados dos modulos
    static Dictionary<string, string> desastreModulos = new Dictionary<string, string>();
    static Dictionary<string, int> forcaModulos = new Dictionary<string, int>();
    static Dictionary<string, int> tipoDosModulos = new Dictionary<string, int>();
    //dados das portas
    static Dictionary<string, bool> estadoPortas = new Dictionary<string, bool>();
    static Dictionary<string, UnityEvent> eventoAbrirPortas = new Dictionary<string, UnityEvent>();
    static Dictionary<string, UnityEvent> eventoFecharPortas = new Dictionary<string, UnityEvent>();
    //dados dos centros de recurso
    static Dictionary<string, CentroDeRecursoInfinito> dadosCentroRecursos = new Dictionary<string, CentroDeRecursoInfinito>();
    //dados dos npcs
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
            case 0://salvar
                //salva desastre
                if (desastreModulos.ContainsKey(moduloScript.gameObject.name))
                    desastreModulos[moduloScript.gameObject.name] = moduloScript.GetNomeDesastre();
                else
                    desastreModulos.Add(moduloScript.gameObject.name, moduloScript.GetNomeDesastre());
                //salva resistencia
                if (forcaModulos.ContainsKey(moduloScript.gameObject.name))
                    forcaModulos[moduloScript.gameObject.name] = moduloScript.GetvalorResistencia();
                else
                    forcaModulos.Add(moduloScript.gameObject.name, moduloScript.GetvalorResistencia());
                //salva tipo do modulo
                if (tipoDosModulos.ContainsKey(moduloScript.gameObject.name))
                    tipoDosModulos[moduloScript.gameObject.name] = moduloScript.GetModulo();
                else
                    tipoDosModulos.Add(moduloScript.gameObject.name, moduloScript.GetModulo());
                break;
            case 1://carregar
                //salva desastre
                if (desastreModulos.ContainsKey(moduloScript.gameObject.name))
                    moduloScript.SetNomeDesastre(desastreModulos[moduloScript.gameObject.name]);
                //salva resistencia
                if (forcaModulos.ContainsKey(moduloScript.gameObject.name))
                    moduloScript.SetValorResistencia(forcaModulos[moduloScript.gameObject.name]);
                //salva tipo do modulo
                if (tipoDosModulos.ContainsKey(moduloScript.gameObject.name))
                    moduloScript.SetModulo(tipoDosModulos[moduloScript.gameObject.name]);
                break;
        }
    }
    public void Salvar_CarregarDadosDasPortas(Porta portaScript, int acao)
    {
        switch (acao)
        {
            case 0://salvar
                //salva estado porta
                if (estadoPortas.ContainsKey(portaScript.gameObject.name))
                    estadoPortas[portaScript.gameObject.name] = portaScript.GetAberto_Fechado();
                else
                    estadoPortas.Add(portaScript.gameObject.name, portaScript.GetAberto_Fechado());
                //salva eventos de abrir porta
                if (eventoAbrirPortas.ContainsKey(portaScript.gameObject.name))
                    eventoAbrirPortas[portaScript.gameObject.name] = portaScript.GetEventosAbrirPorta();
                else
                    eventoAbrirPortas.Add(portaScript.gameObject.name, portaScript.GetEventosAbrirPorta());
                //salva eventos de fechar porta
                if (eventoFecharPortas.ContainsKey(portaScript.gameObject.name))
                    eventoFecharPortas[portaScript.gameObject.name] = portaScript.GetEventosFecharPorta();
                else
                    eventoFecharPortas.Add(portaScript.gameObject.name, portaScript.GetEventosFecharPorta());
                break;
            case 1://carregar
                //salva estado porta
                if (estadoPortas.ContainsKey(portaScript.gameObject.name))
                    estadoPortas[portaScript.gameObject.name] = portaScript.GetAberto_Fechado();
                else
                    estadoPortas.Add(portaScript.gameObject.name, portaScript.GetAberto_Fechado());
                //salva eventos de abrir porta
                if (eventoAbrirPortas.ContainsKey(portaScript.gameObject.name))
                    eventoAbrirPortas[portaScript.gameObject.name] = portaScript.GetEventosAbrirPorta();
                else
                    eventoAbrirPortas.Add(portaScript.gameObject.name, portaScript.GetEventosAbrirPorta());
                //salva eventos de fechar porta
                if (eventoFecharPortas.ContainsKey(portaScript.gameObject.name))
                    eventoFecharPortas[portaScript.gameObject.name] = portaScript.GetEventosFecharPorta();
                else
                    eventoFecharPortas.Add(portaScript.gameObject.name, portaScript.GetEventosFecharPorta());
                break;
        }
    }
    public void Salvar_CarregarDadosDosCentrosDeRecursos(CentroDeRecursoInfinito centroScript, int acao)
    {
        switch (acao)
        {
            case 0://salva
                if (dadosCentroRecursos.ContainsKey(centroScript.gameObject.name))
                {
                    dadosCentroRecursos[centroScript.gameObject.name].SetTempoRestanteCooldown(centroScript.GetTempoRestanteCooldown());
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