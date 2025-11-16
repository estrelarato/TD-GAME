using UnityEngine;

public class Serra : MonoBehaviour
{
    [Header("Configuração do projétil")]
    public float velocidade = 7f;        // Velocidade do projétil
    public float tempoDeVida = 5f;       // Destruição automática
    public int dano = 10;                // Dano que o projétil causa no player

    [Header("Rotação")]
    public float velocidadeRotacao = 360f; // Graus por segundo

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Encontrar o player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector2 direcao = (playerObj.transform.position - transform.position).normalized;
            rb.linearVelocity = direcao * velocidade;
        }

        // Destruir depois de tempoDeVida segundos
        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {
        // Gira visualmente
        transform.Rotate(0f, 0f, velocidadeRotacao * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano); // usa o valor público definido no Inspector

            Destroy(gameObject);
        }

        // Evita destruir em outros inimigos
        if (outro.CompareTag("Inimigo") || outro.CompareTag("EnemyBullet"))
            return;

        // Destroi ao colidir com paredes ou outros obstáculos
        if (outro.CompareTag("Parede"))
            Destroy(gameObject);
    }
}
