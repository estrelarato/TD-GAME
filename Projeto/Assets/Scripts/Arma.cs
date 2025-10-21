using UnityEngine;

public class Arma : MonoBehaviour
{
    public Transform saidaDoTiro;
    public GameObject balaPrefab;
    public float intervaloDeDisparo = 0.25f;
    public float velocidadeBala = 10f;

    private float tempoDeDisparo;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Mira
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = mousePos - transform.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Atira
        if (Input.GetMouseButton(0) && Time.time > tempoDeDisparo)
        {
            tempoDeDisparo = Time.time + intervaloDeDisparo;
            GameObject bala = Instantiate(balaPrefab, saidaDoTiro.position, saidaDoTiro.rotation);
            bala.GetComponent<Rigidbody2D>().linearVelocity = saidaDoTiro.right * velocidadeBala;
        }

        
    }
    void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + transform.right * 1f);
    }
}


