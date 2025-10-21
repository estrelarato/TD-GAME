using UnityEngine;

public class Bala : MonoBehaviour
{
    public int dano = 20;
    public float tempoDeVida = 2f;

    void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Inimigo"))
        {
            outro.GetComponent<Inimigo>().LevarDano(dano);
            Destroy(gameObject);
        }
    }
}
