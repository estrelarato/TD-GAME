using UnityEngine;
using UnityEngine.UI;

public class Carapaça : MonoBehaviour
{
    [Header("Atributos Gerais")]
    public float velocidade = 2f;
    public int vidaMaxima = 40;
    public int vidaAtual;
    public int dano = 10;
    public int pontosAoMorrer = 15;

    [Header("Ataque")]
    public GameObject projetilPrefab;
    public Transform pontoDisparo;
    public float distanciaMinima = 3f;
    public float distanciaMaxima = 6f;
    public float tempoEntreTiros = 1.2f;
    private float timerTiro;

    [Header("UI / Visual")]
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
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);
        Vector2 direcao = (player.position - transform.position).normalized;

        // Movimento inteligente
        if (distancia > distanciaMaxima)
        {
            transform.position += (Vector3)direcao * velocidade * Time.deltaTime;
        }
        else if (distancia < distanciaMinima)
        {
            transform.position -= (Vector3)direcao * velocidade * Time.deltaTime;
        }

        // Rotaciona para mirar
        if (corpo != null)
        {
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            corpo.rotation = Quaternion.Euler(0, 0, angulo - 90f);
        }

        // Disparo
        timerTiro -= Time.deltaTime;
        if (distancia <= distanciaMaxima && distancia >= distanciaMinima && timerTiro <= 0f)
        {
            Atirar(direcao);
            timerTiro = tempoEntreTiros;
        }

        if (barraVida != null)
            barraVida.value = (float)vidaAtual / vidaMaxima;
    }

    void Atirar(Vector2 direcao)
    {
        GameObject projetil = Instantiate(projetilPrefab, pontoDisparo.position, Quaternion.identity);
        Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direcao * 7f;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano);

            AwardPoints();
            Destroy(gameObject);
        }

        if (outro.CompareTag("PlayerBullet"))
        {
            Bala bala = outro.GetComponent<Bala>();
            if (bala != null)
                LevarDano(bala.dano);

            Destroy(outro.gameObject);
        }
    }

    public void LevarDano(int danoRecebido)
    {
        vidaAtual -= danoRecebido;

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
