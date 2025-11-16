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
            Player p = collision.GetComponent<Player>();
            if (p != null)
                p.LevarDano(damage);

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}
