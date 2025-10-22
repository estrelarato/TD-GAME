using UnityEngine;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    public float velocidade = 2f;
    public int vidaMaxima = 50;
    public int vidaAtual;
    public int pontosAoMorrer = 10;

    public Slider barraVida;
    public Transform corpo; // novo campo: o sprite que vai girar

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

            // Rotação aplicada só ao "corpo"
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            corpo.rotation = Quaternion.Euler(0, 0, angulo - 90f);
        }

        // Atualiza barra de vida
        if (barraVida != null)
            barraVida.value = (float)vidaAtual / vidaMaxima;
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            colisao.gameObject.GetComponent<Player>().LevarDano(10);
        }
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
