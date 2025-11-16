using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Atributos")]
    public float velocidade = 5f;
    public int vidaMaxima = 100;
    public int vidaAtual;
    public int pontuacao;

    [Header("Invencibilidade")]
    public float tempoInvencivel = 1.2f;
    private bool invencivel = false;

    [Header("UI")]
    public Slider barraVidaUI;
    public TMPro.TextMeshProUGUI textoPontuacao;

    private Rigidbody2D rb;
    private Vector2 direcao;

    private SpriteRenderer spriteRenderer;
    private Color corOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        vidaAtual = vidaMaxima;

        if (spriteRenderer != null)
            corOriginal = spriteRenderer.color;

        AtualizarHUD();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direcao = new Vector2(moveX, moveY).normalized;

        AtualizarHUD();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    // =============================================
    // DANO + INVENCIBILIDADE + PISCAR
    // =============================================
    public void LevarDano(int dano)
    {
        if (invencivel) return;

        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            AtualizarHUD();
            Morrer();
            return;
        }

        AtualizarHUD();
        StartCoroutine(Invencibilidade());
    }

    private System.Collections.IEnumerator Invencibilidade()
    {
        invencivel = true;
        float tempo = 0f;

        while (tempo < tempoInvencivel)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;  // efeito flash claro

            yield return new WaitForSeconds(0.1f);

            if (spriteRenderer != null)
                spriteRenderer.color = corOriginal;

            yield return new WaitForSeconds(0.1f);

            tempo += 0.2f;
        }

        invencivel = false;

        if (spriteRenderer != null)
            spriteRenderer.color = corOriginal;
    }

    void Morrer()
    {
        Destroy(gameObject);
    }

    public void AdicionarPontuacao(int pontos)
    {
        pontuacao += pontos;
        AtualizarHUD();


        if (GameManager.instance != null)
            GameManager.instance.VerificarBoss(pontuacao);
    }

    void AtualizarHUD()
    {
        if (barraVidaUI != null)
            barraVidaUI.value = (float)vidaAtual / vidaMaxima;

        if (textoPontuacao != null)
            textoPontuacao.text = "" + pontuacao;
    }

    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;

        if (vidaAtual > vidaMaxima)
        vidaAtual = vidaMaxima;

        AtualizarHUD();
    }

}
