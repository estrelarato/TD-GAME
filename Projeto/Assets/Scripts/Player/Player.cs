using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
    public TextMeshProUGUI textoPontuacao;
    public Image fadeImage; // Imagem do fade (UI)

    private Rigidbody2D rb;
    private Vector2 direcao;

    private SpriteRenderer spriteRenderer;
    private Color corOriginal;
    private bool morto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        vidaAtual = vidaMaxima;

        if (spriteRenderer != null)
            corOriginal = spriteRenderer.color;

        // Zera o alpha do fade ao iniciar
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        AtualizarHUD();
    }

    void Update()
    {
        if (morto) return; // Bloqueia movimento após morte

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direcao = new Vector2(moveX, moveY).normalized;

        AtualizarHUD();
    }

    void FixedUpdate()
    {
        if (!morto)
            rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    // ------------------------------------
    // DANO + INVENCIBILIDADE + HIT FLASH
    // ------------------------------------
    public void LevarDano(int dano)
    {
        if (invencivel || morto) return;

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
                spriteRenderer.color = Color.white;

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

    // ------------------------------------
    // MORTE + FADE + BLOQUEIO DE CONTROLE
    // ------------------------------------
    void Morrer()
    {
        if (morto) return;
        morto = true;

        // Desativa sprite (some da tela imediatamente)
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // trava movimento
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true;

        StartCoroutine(FadeGameOver());
    }

    private System.Collections.IEnumerator FadeGameOver()
    {
        float duracao = 0.4f;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, tempo / duracao);

            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = alpha;
                fadeImage.color = c;
            }

            yield return null;
        }

        // Carrega a cena após o fade
        SceneManager.LoadScene("GameOver");
    }

    // ------------------------------------
    // PONTUAÇÃO
    // ------------------------------------
    public void AdicionarPontuacao(int pontos)
    {
        pontuacao += pontos;
        AtualizarHUD();

        if (GameManager.instance != null)
            GameManager.instance.VerificarBoss(pontuacao);
    }

    // ------------------------------------
    // HUD
    // ------------------------------------
    void AtualizarHUD()
    {
        if (barraVidaUI != null)
            barraVidaUI.value = (float)vidaAtual / vidaMaxima;

        if (textoPontuacao != null)
            textoPontuacao.text = "" + pontuacao;
    }

    // ------------------------------------
    // CURA
    // ------------------------------------
    public void Curar(int quantidade)
    {
        vidaAtual += quantidade;

        if (vidaAtual > vidaMaxima)
            vidaAtual = vidaMaxima;

        AtualizarHUD();
    }
}
