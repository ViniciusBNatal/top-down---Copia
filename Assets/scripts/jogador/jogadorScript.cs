using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jogadorScript : MonoBehaviour, AcoesNoTutorial
{
    public static jogadorScript Instance { get; private set; }
    //variáveis publicas
    [Header("Valores Numéricos")]
    [SerializeField] private bool tutorial;
    [SerializeField] private float velocidade;
    //private float vidaMaxima = 1;
    [SerializeField] private float velocidadeProjetil;
    [SerializeField] private float taxaDeDisparo;
    [SerializeField] private float taxaDeAtaqueMelee;
    [SerializeField] private float alcanceMelee;
    private Vector2 distanciaAtaqueMelee;
    [SerializeField] private float danoMelee;
    [SerializeField] private float tempParalisacaoProjetil;
    [Header("Componentes")]
    //[SerializeField] private JogadorAnimScript animScript;
    [SerializeField] private Transform posicaoMelee;
    public Camera mainCamera;
    [SerializeField] private GameObject projetilPrefab;
    [SerializeField] private GameObject armaMelee;
    [SerializeField] private Transform pontoDeDisparo;
    [SerializeField] private LayerMask objetosAcertaveisLayer;
    public UIinventario InterfaceJogador;
    [SerializeField] private SpriteRenderer iconeInteracao;
    public CinemachineBehaviour comportamentoCamera;
    //variáveis privadas
    //private float vidaAtual;
    private Vector2 movimento;
    private Rigidbody2D rb;
    private bool atirando = false;
    private bool atacando = false;
    private float forcaEmpurrao;
    private Vector2 direcaoEmpurrao;
    [Header("Não Mexer")]
    private Vector2 baguncarControles = new Vector2(0,0);
    public enum estados
    {
        EmAcao,
        Paralisado,
        EmContrucao,
        EmDialogo,
        SendoEmpurrado,
        EmUI
    };
    /*[HideInInspector]*/ public estados estadosJogador = estados.EmAcao;// 0 = em acao, 1 = em menus, 2 = em construcao 
    private ReceitaDeCrafting moduloCriado;
    private bool podeAnimar = true;
    private Vector2 direcaoProjetil = Vector2.zero;
    [SerializeField] private GameObject caixaComTudo;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        rb = GetComponent<Rigidbody2D>();
        distanciaAtaqueMelee.x = posicaoMelee.localPosition.x;
        distanciaAtaqueMelee.y = posicaoMelee.localPosition.y;
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))  
        //    SceneManager.LoadScene("testes");
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(caixaComTudo, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
        }
        if (!UIinventario.Instance.pausado)
        {
            switch (estadosJogador)
            {
                case estados.EmAcao: //movimentando
                    MovimentoInput();
                    InputAtirar();
                    InputAtaqueMelee();
                    break;
                //case estados.EmContrucao://construindo
                //    //executando click em objetos
                //    break;
                case estados.EmDialogo:
                    InputProsseguirDialogo();
                    break;
                default:
                    break;
            }
        }
    }
    private void FixedUpdate()
    {
         if (estadosJogador == estados.EmAcao)
         {
            rb.velocity = movimento * velocidade + baguncarControles;
         }
         else if (estadosJogador == estados.SendoEmpurrado)
         {
            //rb.AddForce(-forcaEmpurrao * direcaoEmpurrao);
            rb.velocity = -forcaEmpurrao * direcaoEmpurrao;
         }
         else
         {
            rb.velocity = Vector2.zero;
         }
    }
    private void MovimentoInput()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        MudaAreaAtaque(movX, movY);
        movimento = new Vector2(movX, movY).normalized;//normalized faz com q o movimento seja igual para todas as direções, não passando de um limite de 1
    }
    public Vector3 PegaPosicoMouse()
    {
        Vector3 posicaoMouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = -1f;
        //Debug.Log(posicaoMouse);
        return posicaoMouse;
    }
    private void InputAtirar()//dispara ao apertar o botão direito do mouse
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!atirando)
            {
                atirando = true;
                MudarEstadoJogador(1);
                Vector3 dirAnim = (PegaPosicoMouse() - transform.position).normalized;
                direcaoProjetil = (PegaPosicoMouse() - pontoDeDisparo.position).normalized;
                JogadorAnimScript.Instance.AnimarDisparo(dirAnim.x, dirAnim.y, taxaDeDisparo);
                //SoundManager.Instance.TocarSom(SoundManager.TipoSom.JogadorAtirando);
            }
        }
    }
    public void Atira()
    {
        if (atirando)
        {
            GameObject bala = Instantiate(projetilPrefab, pontoDeDisparo.position, Quaternion.identity);
            bala.transform.Rotate(new Vector3(0f, 0f, Mathf.Atan2(direcaoProjetil.y, direcaoProjetil.x) * Mathf.Rad2Deg));//rotaciona a bala
            bala.GetComponent<Rigidbody2D>().velocity = direcaoProjetil * velocidadeProjetil;//new Vector3(direcaoProjetil.x / Mathf.Abs(direcaoProjetil.x), direcaoProjetil.y / Mathf.Abs(direcaoProjetil.y), 0f)
            bala.GetComponent<balaHit>().SetDano(tempParalisacaoProjetil);
            MudarEstadoJogador(0);
            atirando = false;
        }
    }
    private void InputAtaqueMelee()// animação e inflinge dano caso encontre algo
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !atacando)
        {
            atacando = true;
            JogadorAnimScript.Instance.AnimarAtaqueMelee(posicaoMelee.localPosition.x, posicaoMelee.localPosition.y, taxaDeAtaqueMelee);
        }
    }
    public void ataqueMelee()
    {
        if (atacando)
        {
            Collider2D[] objetosAcertados = Physics2D.OverlapCircleAll(posicaoMelee.position, alcanceMelee, objetosAcertaveisLayer);//hit em objetos
            foreach (Collider2D objeto in objetosAcertados)
            {
                if (objeto.gameObject.layer == 8)
                {
                    objeto.GetComponent<hitbox_inimigo>().inimigo.GetComponent<inimigoScript>().mudancaVida(-danoMelee, this.tag);
                }
                else if (objeto.gameObject.layer == 9 && !desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
                {
                    objeto.gameObject.GetComponentInParent<CentroDeRecurso>().RecebeuHit();
                }
            }
            atacando = false;
        }
    }
    private void InputProsseguirDialogo()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DialogeManager.Instance.MostraProximoDialogo();
        }
    }
    private void MudaAreaAtaque(float movX, float movY)// atualiza a direção do ataque melee sempre que o jogador muda de direção
    {
        if (!atacando)
        {
            if (Input.GetButton("Horizontal")) //&& movX != 0)
            {
                posicaoMelee.localPosition =  new Vector3(movX * Mathf.Abs(distanciaAtaqueMelee.x), distanciaAtaqueMelee.y, 0f);
                pontoDeDisparo.localPosition = new Vector3(movX * Mathf.Abs(pontoDeDisparo.localPosition.x), 0f, 0f);
            } 
            if (Input.GetButton("Vertical")) //&& movY != 0)
            {
                posicaoMelee.localPosition = new Vector3(0f, movY * Mathf.Abs(distanciaAtaqueMelee.x), 0f);
                pontoDeDisparo.localPosition = new Vector3(pontoDeDisparo.localPosition.x, movY * Mathf.Abs(pontoDeDisparo.localPosition.y), 0f);
            }
        }
    }
    /*private void mousePrecionado()
    {
        Mydebug.mydebug.MyPrint("click do mouse");
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Mydebug.mydebug.MyPrint("click do mouse");
            Vector2 mousePos = PegaPosicoMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                Clicavel clicavel = hit.collider.GetComponent<Clicavel>();
                clicavel?.Click(this);
            }
        }
    }*/
    public void MudarEstadoJogador(int i)
    {
        switch (i)
        {
            case 0:
                ControleManager();
                estadosJogador = estados.EmAcao;
                podeAnimar = true;
                break;
            case 1:
                ControleManager();
                estadosJogador = estados.Paralisado;
                podeAnimar = false;
                break;
            case 2:
                ControleManager();
                estadosJogador = estados.EmContrucao;
                podeAnimar = false;
                break;
            case 3:
                ControleManager();
                estadosJogador = estados.EmDialogo;
                podeAnimar = false;
                break;
            case 4:
                ControleManager();
                estadosJogador = estados.SendoEmpurrado;
                //atirando = false;
                podeAnimar = false;
                break;
            case 5:
                ControleManager();
                estadosJogador = estados.EmUI;
                podeAnimar = false;
                break;
        }
    }
    private void ControleManager()
    {
        switch ((int)estadosJogador)
        {
            case 2://em construção
                if (comportamentoCamera.GetFocoDaCamera() != this.transform && BossAlho.Instance == null)
                    comportamentoCamera.MudaFocoCamera(transform, 0f);
                if (BaseScript.Instance != null)
                {
                    BaseScript.Instance.Ativar_DesativarVisualConstrucaoModulos(false);
                    //ClickEmObjetos.Instance.RetornarMaterialOriginal(); 
                }
                break;
            case 3://em dialogo
                if (DialogeManager.Instance.GetEstadoDialogo())
                {
                    Debug.Log("remover dialogo");
                    DialogeManager.Instance.FimDialogo();
                }
                break;
            case 5://em UI
                if (UIinventario.Instance.InventarioAberto)
                {
                    UIinventario.Instance.fecharTodoInventario();
                }
                break;
            default:
                break;
        }
    }
    public void Knockback(float duracaoEmpurrao, float forca, Transform obj)
    {
        MudarEstadoJogador(4);
        direcaoEmpurrao = (obj.transform.position - this.transform.position).normalized;
        forcaEmpurrao = forca;;
        StartCoroutine(this.duracaoKnockback(duracaoEmpurrao));
    }
    private IEnumerator duracaoKnockback(float duracaoEmpurrao)
    {
        yield return new WaitForSeconds(duracaoEmpurrao);
        MudarEstadoJogador(0);
    }
    public void mudancaRelogio(float valor, float duracaoStn)
    {
        if (atirando && estadosJogador == estados.Paralisado)
        {
            MudarEstadoJogador(0);
            atirando = false;
        }
        if (atacando)
        {
            atacando = false;
        }
        JogadorAnimScript.Instance.Hit(duracaoStn);
        desastreManager.Instance.AvisoDePerigoTimer();
        //if (desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
        //{
        //    desastreManager.Instance.MudarTempoAcumuladoParaDesastre(desastreManager.Instance.GetTempoAcumuladoParaDesastre() + Mathf.Clamp(valor, 0, desastreManager.Instance.GetIntervaloDeTempoEntreOsDesastres()));
        //}
        //else
        //{
        if (!desastreManager.Instance.VerificarSeUmDesastreEstaAcontecendo())
        {
            desastreManager.Instance.DiminuirTempoRestanteParaDesastre(valor);
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.GetTempoRestanteParaDesastre(), 0f, false);
        }
        //}
        //vidaAtual += valor;
        //inventario.BarraDeVida.GetComponent<barraDeVida>().AtualizaBarraDeVida(vidaAtual);
        //if (vidaAtual > vidaMaxima)
        //{
        //    vidaAtual = vidaMaxima;
        //}
        //else if (vidaAtual <= 0)
        //{
        //    vidaAtual = 0f;
        //}
    }
    public void IndicarInteracaoPossivel(Sprite imagem, bool visivel)
    {
        if (visivel)
        {
            iconeInteracao.sprite = imagem;
            iconeInteracao.enabled = true;
        }
        else
            iconeInteracao.enabled = false;
    }
    public void BaguncaControles(float unidadesX, float unidadesY, float forcaGeral)
    {
        baguncarControles = new Vector2(Random.Range(-unidadesX, unidadesX),Random.Range(-unidadesY, unidadesY)) * forcaGeral;
        if (baguncarControles.x == 0)
            baguncarControles.x = 1;
        if (baguncarControles.y == 0)
            baguncarControles.y = 1;
    }
    private void OnDrawGizmosSelected()
    {
        if (posicaoMelee == null)
            return;
        Gizmos.DrawWireSphere(posicaoMelee.position, alcanceMelee);
    }
    public bool GetPodeAnimar()
    {
        return podeAnimar;
    }
    public void SetTutorial(bool b)
    {
        tutorial = b;
    }
    public void Tutorial()
    {
        if (tutorial)
        {
            TutorialSetUp.Instance.SetupInicialJogador();
            tutorial = false;
        }
    }
    public void AoLevantar()
    {
        JogadorAnimScript.Instance.Levantar(false);
        TutorialSetUp.Instance.IniciarDialogo();
    }
    public void AoFinalizarDialogo(object origem, System.EventArgs args)
    {
        comportamentoCamera.MudaFocoCamera(transform, 0);
    }
    public void SetDirecaoDeMovimentacaoAleatoria(Vector2 vec)
    {
        baguncarControles = vec;
    }
    public ReceitaDeCrafting GetModuloConstruido()
    {
        return moduloCriado;
    }
    public void SetModuloConstruido(ReceitaDeCrafting modulo)
    {
        moduloCriado = ScriptableObject.CreateInstance<ReceitaDeCrafting>();
        moduloCriado = modulo;
    }
}
