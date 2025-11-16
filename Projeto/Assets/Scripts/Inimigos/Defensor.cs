using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DashEnemy : MonoBehaviour
{
    [Header("Atributos")]
    public float velocidade = 2f;
    public float forcaDash = 10f;
    public int vidaMaxima = 20;
    public int vidaAtual;
    public int dano = 20;
    public int pontosAoMorrer = 20;

    [Header("Dash")]
    public float distanciaDash = 3f;          // Quando começa o dash
    public float tempoPreparacao = 0.4f;      // Tempo parado antes de dar dash
    public float tempoVulneravel = 0.7f;      // Tempo parado depois do dash

    [Header("UI / Visual")]
    public Slider barraVida;
    public Transform corpo;

    private Transform player;
    private Rigidbody2D rb;
    private bool emDash = false;

    void Start()
    {
        vidaAtual = vidaMaxima;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MaquinaDeEstados());

        // Inicializa slider corretamente
        if (barraVida != null)
        {
            barraVida.maxValue = vidaMaxima;
            barraVida.value = vidaMaxima;
        }
    }

    IEnumerator MaquinaDeEstados()
    {
        while (vidaAtual > 0)
        {
            if (player == null) yield return null;

            float distancia = Vector2.Distance(transform.position, player.position);

            if (!emDash && distancia <= distanciaDash)
            {
                rb.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(tempoPreparacao);

                emDash = true;
                Vector2 direcaoDash = (player.position - transform.position).normalized;
                rb.linearVelocity = direcaoDash * forcaDash;

                yield return new WaitForSeconds(0.25f);
                rb.linearVelocity = Vector2.zero;
                emDash = false;

                yield return new WaitForSeconds(tempoVulneravel);
            }
            else if (!emDash)
            {
                Vector2 direcao = (player.position - transform.position).normalized;
                rb.linearVelocity = direcao * velocidade;
            }

            if (corpo != null && player != null)
            {
                Vector2 direcao = (player.position - transform.position).normalized;
                float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
                corpo.rotation = Quaternion.Euler(0, 0, angulo - 90f);
            }

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano);
        }

        if (outro.CompareTag("PlayerBullet"))
        {
            Bala bala = outro.GetComponent<Bala>();
            if (bala != null)
            {
                LevarDano(bala.dano);
                Destroy(outro.gameObject);
            }
        }
    }

    public void LevarDano(int danoRecebido)
    {
        vidaAtual -= danoRecebido;

        if (barraVida != null)
            barraVida.value = vidaAtual;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            AwardPoints();
            Destroy(gameObject);
        }
    }

    void AwardPoints()
    {
        if (player != null)
        {
            Player p = player.GetComponent<Player>();
            if (p != null)
                p.AdicionarPontuacao(pontosAoMorrer);
        }
    }
}
