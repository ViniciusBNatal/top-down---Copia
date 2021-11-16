using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SalvarEstadoDoObjeto : MonoBehaviour
{
    static Dictionary<string, bool> objsSalvos = new Dictionary<string, bool>();
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
    static Dictionary<string, bool> estadoCentroDeRecursos = new Dictionary<string, bool>();
    static Dictionary<string, int> tempoRestanteCentroDeRecursos = new Dictionary<string, int>();
    static Dictionary<string, int> vidaRestanteCentroDeRecursos = new Dictionary<string, int>();
    static Dictionary<string, int> extracoesRestanteCentroDeRecursos = new Dictionary<string, int>();
    //dados dos npcs
    //static Dictionary<string, bool> estadoMissaoNPC = new Dictionary<string, bool>();
    static Dictionary<string, int> estadoMissaoNPC = new Dictionary<string, int>();
    static Dictionary<string, Item> itemMissaoNPC = new Dictionary<string, Item>();
    //dados dialogos unicos
    static Dictionary<string, bool> dialogosFinalizados = new Dictionary<string, bool>();
    // Start is called before the first frame update
    void Start()
    {
        AdicionarALista();
        if (objsSalvos[this.gameObject.name])
        {
            if (gameObject.GetComponent<SalvamentoEntreCenas>() != null)
                 gameObject.GetComponent<SalvamentoEntreCenas>().AcaoSeEstadoJaModificado();
        }
    }
    public void AdicionarALista()
    {
        if (!objsSalvos.ContainsKey(this.gameObject.name))
            objsSalvos.Add(this.gameObject.name, false);
    }
    public void SalvarSeJaFoiModificado()
    {
        if (objsSalvos.ContainsKey(this.gameObject.name))
            objsSalvos[this.gameObject.name] = true;
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
                //carrega estado porta
                if (estadoPortas.ContainsKey(portaScript.gameObject.name))
                    portaScript.SetAberto_Fechado(estadoPortas[portaScript.gameObject.name]);
                //carrega eventos de abrir porta
                if (eventoAbrirPortas.ContainsKey(portaScript.gameObject.name))
                    portaScript.SetEventosAbrirPorta(eventoAbrirPortas[portaScript.gameObject.name]);
                //carrega eventos de fechar porta
                if (eventoFecharPortas.ContainsKey(portaScript.gameObject.name))
                    portaScript.SetEventosFecharPorta(eventoFecharPortas[portaScript.gameObject.name]);
                break;
        }
    }
    public void Salvar_CarregarDadosDosCentrosDeRecursos(CentroDeRecursoInfinito centroScript, int acao)
    {
        switch (acao)
        {
            case 0://salva
                //salva estado 
                if (estadoCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    estadoCentroDeRecursos[centroScript.gameObject.name] = centroScript.GetCentroDeInimigos();
                else
                    estadoCentroDeRecursos.Add(centroScript.gameObject.name, centroScript.GetCentroDeInimigos());
                //salva tempo restante
                if (tempoRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    tempoRestanteCentroDeRecursos[centroScript.gameObject.name] = centroScript.GetTempoRestanteCooldown();
                else
                    tempoRestanteCentroDeRecursos.Add(centroScript.gameObject.name, centroScript.GetTempoRestanteCooldown());
                //salva vida restante
                if (vidaRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    vidaRestanteCentroDeRecursos[centroScript.gameObject.name] = centroScript.GetVidaAtual();
                else
                    vidaRestanteCentroDeRecursos.Add(centroScript.gameObject.name, centroScript.GetVidaAtual());
                //salva extrações restantes
                if (extracoesRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    extracoesRestanteCentroDeRecursos[centroScript.gameObject.name] = centroScript.GetVezesExtraida();
                else
                    extracoesRestanteCentroDeRecursos.Add(centroScript.gameObject.name, centroScript.GetVezesExtraida());
                break;
            case 1://carregar
                //carrega estado 
                if (estadoCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    centroScript.SetCentroDeInimigos(estadoCentroDeRecursos[centroScript.gameObject.name]);
                //carrega tempo restante
                if (tempoRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    centroScript.SetTempoRestanteCooldown(tempoRestanteCentroDeRecursos[centroScript.gameObject.name] -(Mathf.FloorToInt(Time.time - SalvamentoDosCentrosDeRecursosManager.Instance.TempoDeSaidaDaFase(SceneManager.GetActiveScene().buildIndex))));
                //carrega vida restante
                if (vidaRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    centroScript.SetVidaAtual(vidaRestanteCentroDeRecursos[centroScript.gameObject.name]);
                //salva extrações restantes
                if (extracoesRestanteCentroDeRecursos.ContainsKey(centroScript.gameObject.name))
                    centroScript.SetVezesExtraida(extracoesRestanteCentroDeRecursos[centroScript.gameObject.name]);
                break;
        }
    }
    public void Salvar_CarregarDadosDosNPCs(NPCscript npcScript, int acao)
    {
        switch (acao)
        {
            case 0://salvar
                //salva estado de missao
                if (estadoMissaoNPC.ContainsKey(npcScript.gameObject.name))
                    //estadoMissaoNPC[npcScript.gameObject.name] = npcScript.GetMissaoCumprida();
                    estadoMissaoNPC[npcScript.gameObject.name] = npcScript.GetNDeDialogos();
                else
                    //estadoMissaoNPC.Add(npcScript.gameObject.name, npcScript.GetMissaoCumprida());
                    estadoMissaoNPC.Add(npcScript.gameObject.name, npcScript.GetNDeDialogos());
                //salva item da missao
                if (itemMissaoNPC.ContainsKey(npcScript.gameObject.name))
                    itemMissaoNPC[npcScript.gameObject.name] = npcScript.GetItemDaMissao();
                else
                    itemMissaoNPC.Add(npcScript.gameObject.name, npcScript.GetItemDaMissao());
                break;
            case 1://carregar
                //carrega estado de missao
                if (estadoMissaoNPC.ContainsKey(npcScript.gameObject.name))
                    //npcScript.SetMissaoCumprida(estadoMissaoNPC[npcScript.gameObject.name]);
                    npcScript.SetNDeDialogo(estadoMissaoNPC[npcScript.gameObject.name]);
                //carrega item da missao
                if (itemMissaoNPC.ContainsKey(npcScript.gameObject.name))
                    npcScript.SetItemDaMissao(itemMissaoNPC[npcScript.gameObject.name]);
                break;
        }
    }
    public void Salvar_CarregarDadosDosDialogosUnicos(DialogoUnico dialogoScript, int acao)
    {
        switch (acao)
        {
            case 0://salvar
                //salva se ja falou
                if (dialogosFinalizados.ContainsKey(dialogoScript.gameObject.name))
                    dialogosFinalizados[dialogoScript.gameObject.name] = dialogoScript.GetDialogoORealizado();
                else
                    dialogosFinalizados.Add(dialogoScript.gameObject.name, dialogoScript.GetDialogoORealizado());
                break;
            case 1://carregar
                //carrega se ja falou
                if (dialogosFinalizados.ContainsKey(dialogoScript.gameObject.name))
                    dialogoScript.SetDialogoORealizado(dialogosFinalizados[dialogoScript.gameObject.name]);
                break;
        }
    }
}