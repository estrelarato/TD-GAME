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
        direcao = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direcao * velocidade * Time.fixedDeltaTime);
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;
        AtualizarHUD();

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            Morrer();
        }
    }

    void Morrer()
    {
        Destroy(gameObject);
    }

    public void AdicionarPontuacao(int pontos)
    {
        pontuacao += pontos;
        AtualizarHUD();
    }

    void AtualizarHUD()
    {
        if (barraVidaUI)
            barraVidaUI.value = (float)vidaAtual / vidaMaxima;

        if (textoPontuacao)
            textoPontuacao.text = $"{pontuacao}";
    }
}
