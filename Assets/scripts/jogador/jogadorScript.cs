using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jogadorScript : MonoBehaviour
{
    public static jogadorScript Instance { get; private set; }
    //variáveis publicas
    [Header ("Valores Numéricos")]
    public float velocidade;
    //private float vidaMaxima = 1;
    public float velocidadeProjetil;
    public float taxaDeDisparo;
    public float taxaDeAtaqueMelee;
    public float alcanceMelee;
    public float distanciaAtaqueMelee;
    public float danoMelee;
    public float danoProjetil;
    [Header("Componentes")]
    public JogadorAnimScript animScript;
    public Transform posicaoMelee;
    public Camera mainCamera;
    public GameObject projetil;
    public GameObject armaMelee;
    public Transform pontoDeDisparo;
    public LayerMask objetosAcertaveisLayer;
    public UIinventario inventario;
    public SpriteRenderer iconeInteracao;
    public CinemachineBehaviour comportamentoCamera;
    //variáveis privadas
    //private float vidaAtual;
    private Vector2 movimento;
    private Rigidbody2D rb;
    private float podeAtacar = 0f;
    private Animator animatorPicareta;
    private bool atirando = false;
    private bool sendoEmpurrado = false;
    [Header("Não Mexer")]
    public Vector2 baguncarControles = new Vector2(1,1);
    public enum estados
    {
        EmAcao,
        EmMenus,
        EmContrucao
    };
    public estados estadosJogador;// 0 = em acao, 1 = em menus, 2 = em construcao 
    public itemCrafting moduloCriado;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //vidaAtual = vidaMaxima;
        //inventario.BarraDeVida.GetComponent<barraDeVida>().AtualizaBarraDeVida(vidaAtual);
        animatorPicareta = armaMelee.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (estadosJogador)
        {
            case estados.EmAcao: //movimentando
                MovimentoInput();
                Atirar();
                ataqueMelee();
                break;
            case estados.EmMenus://interface aberta

                break;
            case estados.EmContrucao://construindo
                mousePrecionado();
                break;
        }
    }
    private void FixedUpdate()
    {
        if (!sendoEmpurrado && estadosJogador == estados.EmAcao)
        rb.velocity = movimento * velocidade + baguncarControles;
        
    }
    private void MovimentoInput()
    {
        float movX = Input.GetAxisRaw("Horizontal");
        float movY = Input.GetAxisRaw("Vertical");
        MudaAreaAtaque(movX, movY);
        movimento = new Vector2(movX, movY).normalized;//normalized faz com q o movimento seja igual para todas as direções, não passando de um limite de 1
    }
    private Vector3 PegaPosicoMouse()
    {
        Vector3 posicaoMouse = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        posicaoMouse.z = 0f;
        return posicaoMouse;
    }
    private void Atirar()//dispara ao apertar o botão direito do mouse
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            StartCoroutine("atirar");
        }
    }
    private void ataqueMelee()// animação e inflinge dano caso encontre algo
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > podeAtacar)
        {
            animatorPicareta.SetTrigger("ATACANDO");
            animScript.AnimarAtaqueMelee(posicaoMelee.localPosition.x, posicaoMelee.localPosition.y);
            Collider2D[] objetosAcertados = Physics2D.OverlapCircleAll(posicaoMelee.position, alcanceMelee, objetosAcertaveisLayer);//hit em objetos
            foreach(Collider2D objeto in objetosAcertados)
            {
                if (objeto.gameObject.layer == 8)
                {
                    objeto.GetComponent<hitbox_inimigo>().inimigo.GetComponent<inimigo>().mudancaVida(-danoMelee);
                }
                else if (objeto.gameObject.layer == 9)
                {
                    objeto.gameObject.GetComponent<CentroDeRecurso>().DropaRecursos();
                }
            }
            podeAtacar = Time.time + 1 / taxaDeAtaqueMelee;
        }
    }
    private void MudaAreaAtaque(float movX, float movY)// atualiza a direção do ataque melee sempre que o jogador muda de direção
    {
        if (Input.GetButton("Horizontal")) //&& movX != 0)
        {
            posicaoMelee.localPosition =  new Vector3(movX * Mathf.Abs(distanciaAtaqueMelee), 0f, 0f);
            pontoDeDisparo.localPosition = new Vector3(movX * Mathf.Abs(pontoDeDisparo.localPosition.x), pontoDeDisparo.localPosition.y, 0f);
        } 
        if (Input.GetButton("Vertical")) //&& movY != 0)
        {
            posicaoMelee.localPosition = new Vector3(0f, movY * Mathf.Abs(distanciaAtaqueMelee), 0f);
            pontoDeDisparo.localPosition = new Vector3(pontoDeDisparo.localPosition.x, movY * Mathf.Abs(pontoDeDisparo.localPosition.y), 0f);
        }
    }
    private void mousePrecionado()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousePos = PegaPosicoMouse();
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {
                Clicavel clicavel = hit.collider.GetComponent<Clicavel>();
                clicavel?.Click(this.gameObject);
            }
        }
    }
    public void MudarEstadoJogador(int i)
    {
        switch (i)
        {
            case 0:
                estadosJogador = estados.EmAcao;
                break;
            case 1:
                estadosJogador = estados.EmMenus;
                break;
            case 2:
                estadosJogador = estados.EmContrucao;
                break;
        }
        //estadoJogador = i;
        if (i != 1)
            rb.velocity = Vector2.zero;
    }
    public IEnumerator Knockback(float duracaoEmpurrao, float forca, Transform obj)
    {
        sendoEmpurrado = true;
        MudarEstadoJogador(1);
        float tempo = 0f;
        Vector2 direcao = (obj.transform.position - this.transform.position).normalized;
        while (duracaoEmpurrao > tempo)
        {
            tempo += Time.deltaTime;
            rb.AddForce(-forca * direcao);
            yield return null;
        }
        sendoEmpurrado = false;
        MudarEstadoJogador(0);
    }
    IEnumerator atirar()
    {
        if (!atirando)
        {
            animScript.AnimarDisparo(PegaPosicoMouse().x, PegaPosicoMouse().y);
            atirando = true;
            GameObject bala = Instantiate(projetil, pontoDeDisparo.position, Quaternion.identity);
            Vector3 direcao = PegaPosicoMouse() - pontoDeDisparo.position;
            bala.GetComponent<Rigidbody2D>().velocity = direcao * velocidadeProjetil;
            bala.GetComponent<balaHit>().dano = danoProjetil;
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(taxaDeDisparo);
            atirando = false;
        }
    }
    public void mudancaRelogio(float valor)
    {
        if (desastreManager.Instance.desastreAcontecendo)
        {
            desastreManager.Instance.tempoAcumulado += valor;
        }
        else
        {
            desastreManager.Instance.ConfigurarTimer(desastreManager.Instance.tempoRestante += valor, 0f);
        }
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
        else if(!visivel)
        {
            iconeInteracao.enabled = false;
        }
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
}
