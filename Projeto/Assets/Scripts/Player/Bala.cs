using UnityEngine;

public class Bala : MonoBehaviour
{
    public int dano = 20;
    public float tempoDeVida = 2f;

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossController boss = other.GetComponent<BossController>();
            if (boss != null)
                boss.ReceberDano(dano);

            Destroy(gameObject);
        }
    }
}
