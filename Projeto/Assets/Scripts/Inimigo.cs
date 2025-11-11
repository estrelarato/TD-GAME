using UnityEngine;
using UnityEngine.UI;

public class Inimigo : MonoBehaviour
{
    public float velocidade = 2f;
    public int vidaMaxima = 50;
    public int vidaAtual;
    public int dano = 10; // Dano ajustável pelo Inspector
    public int pontosAoMorrer = 10;

    public Slider barraVida;
    public Transform corpo;

    private Transform player;

    void Start()
    {
        vidaAtual = vidaMaxima;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direcao = (player.position - transform.position).normalized;
            transform.position += (Vector3)direcao * velocidade * Time.deltaTime;

            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            if (corpo != null)
                corpo.rotation = Quaternion.Euler(0, 0, angulo - 90f);
        }

        if (barraVida != null)
            barraVida.value = (float)vidaAtual / vidaMaxima;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano);

            // 🧨 Faz o inimigo morrer ao causar o primeiro dano
            if (player != null)
            {
                Player playerScript = player.GetComponent<Player>();
                if (playerScript != null)
                    playerScript.AdicionarPontuacao(pontosAoMorrer);
            }

            Destroy(gameObject);
        }
    }


    public void LevarDano(int danoRecebido)
    {
        vidaAtual -= danoRecebido;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            if (player != null)
            {
                Player p = player.GetComponent<Player>();
                if (p != null)
                    p.AdicionarPontuacao(pontosAoMorrer);
            }

            Destroy(gameObject);
        }
    }
}