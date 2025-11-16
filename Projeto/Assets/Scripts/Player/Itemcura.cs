using UnityEngine;

public class Itemcura : MonoBehaviour
{
    [Header("Cura")]
    public int quantidadeCura = 20;

    [Header("Pulsar (Scale Effect)")]
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.1f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        // Efeito de pulsar
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player p = collision.GetComponent<Player>();

            if (p != null)
                p.Curar(quantidadeCura);

            Destroy(gameObject);
        }
    }
}
