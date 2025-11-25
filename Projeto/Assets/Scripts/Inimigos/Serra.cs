using UnityEngine;

public class Serra : MonoBehaviour
{
    [Header("Configuração do projétil")]
    public float velocidade = 7f;        
    public float tempoDeVida = 5f;     
    public int dano = 10;                

    [Header("Rotação")]
    public float velocidadeRotacao = 360f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Vector2 direcao = (playerObj.transform.position - transform.position).normalized;
            rb.linearVelocity = direcao * velocidade;
        }


        Destroy(gameObject, tempoDeVida);
    }

    void Update()
    {

        transform.Rotate(0f, 0f, velocidadeRotacao * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            Player p = outro.GetComponent<Player>();
            if (p != null)
                p.LevarDano(dano);

            Destroy(gameObject);
        }


        if (outro.CompareTag("Inimigo") || outro.CompareTag("EnemyBullet"))
            return;


        if (outro.CompareTag("Parede"))
            Destroy(gameObject);
    }
}
