using UnityEngine;

public class Chama : MonoBehaviour
{
    public float lifeTime = 4f;
    public int damage = 1;
    public bool destroyOnHit = true;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();

            if (ph != null)
                ph.TakeDamage(damage);

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}
