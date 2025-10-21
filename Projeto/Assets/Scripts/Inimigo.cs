using UnityEngine;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    public float velocidade = 2f;
    public int vidaMaxima = 50;
    public int vidaAtual;
    public int pontosAoMorrer = 10;

    public Slider barraVida;

    private Transform player;

    void Start()
    {
        vidaAtual = vidaMaxima;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player != null)
        {
            // Movimento
            Vector2 direcao = (player.position - transform.position).normalized;
            transform.position += (Vector3)direcao * velocidade * Time.deltaTime;

            // Rotação para olhar na direção do movimento
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo - 90f); // ajuste -90 se o sprite aponta para cima
        }

        // Atualiza barra de vida
        if (barraVida != null)
            barraVida.value = (float)vidaAtual / vidaMaxima;
    }

    public void LevarDano(int dano)
    {
        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            player.GetComponent<Player>().AdicionarPontuacao(pontosAoMorrer);
            Destroy(gameObject);
        }
    }
}
