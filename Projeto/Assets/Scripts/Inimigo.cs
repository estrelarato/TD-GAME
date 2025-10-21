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
            Vector2 direcao = (player.position - transform.position).normalized;
            transform.position += (Vector3)direcao * velocidade * Time.deltaTime;
        }

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
