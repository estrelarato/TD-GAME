using UnityEngine;

public class Inimigo : Personagem
{
    [SerializeField] private int dano = 1;
    public float raioDeVisao = 1;
    public CircleCollider2D _visaoCollider2D;

    [SerializeField] private Transform posicaoDoPlayer;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool andando = false;
    private bool morto = false; // <- novo controle

    public void setDano(int dano) => this.dano = dano;
    public int getDano() => this.dano;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (posicaoDoPlayer == null)
        {
            posicaoDoPlayer = GameObject.Find("Player").transform;
        }

        raioDeVisao = _visaoCollider2D.radius;
    }

    void Update()
    {
        andando = false;

        if (getVida() > 0)
        {
            if (posicaoDoPlayer.position.x - transform.position.x > 0)
                spriteRenderer.flipX = false;
            else if (posicaoDoPlayer.position.x - transform.position.x < 0)
                spriteRenderer.flipX = true;

            if (posicaoDoPlayer != null &&
                Vector3.Distance(posicaoDoPlayer.position, transform.position) <= raioDeVisao)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    posicaoDoPlayer.transform.position,
                    getVelocidade() * Time.deltaTime);
                andando = true;
            }
        }
        else if (!morto) // <- só executa uma vez quando morre
        {
            morto = true;
            animator.SetTrigger("Morte");
            Debug.Log("Inimigo morreu!");
            Destroy(gameObject, 2f); // <- desaparece depois de 2 segundos
        }

        animator.SetBool("Andando", andando);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && getVida() > 0)
        {
            int novaVida = collision.gameObject.GetComponent<Personagem>().getVida() - getDano();
            collision.gameObject.GetComponent<Personagem>().setVida(novaVida);
            setVida(0);
        }
    }

    public void desativa()
    {
        Destroy(gameObject);
        Debug.Log("Inimigo removido");
    }
}
