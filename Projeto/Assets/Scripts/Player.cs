using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float velocidade = 5f;
    public int vidaMaxima = 100;
    public int vidaAtual;
    public int pontuacao;

    public Slider barraVidaUI;
    public TMPro.TextMeshProUGUI textoPontuacao;

    private Rigidbody2D rb;
    private Vector2 direcao;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
        AtualizarHUD();
    }

    void Update()
    {
        // Entrada de movimento
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        direcao = new Vector2(moveX, moveY).normalized;

        // Atualiza HUD
        AtualizarHUD();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            // Aqui você pode chamar uma tela de Game Over
        }
        AtualizarHUD();
    }

    public void AdicionarPontuacao(int pontos)
    {
        pontuacao += pontos;
        AtualizarHUD();
    }

    void AtualizarHUD()
    {
        if (barraVidaUI != null)
            barraVidaUI.value = (float)vidaAtual / vidaMaxima;

        if (textoPontuacao != null)
            textoPontuacao.text = $"{pontuacao}";
    }
}
