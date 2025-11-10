using UnityEngine;

public class Arma : MonoBehaviour
{
    public Transform saidaDoTiro;
    public GameObject balaPrefab;
    public float intervaloDeDisparo = 0.25f;
    public float velocidadeBala = 10f;

    // Balanço visual
    public float intensidadeBalanço = 20f; // quanto a arma balança ao clicar
    public float duracaoBalanço = 0.1f; // duração do movimento de balanço

    private float tempoDeDisparo;
    private Camera cam;
    private float anguloAlvo;
    private float anguloAtual;
    private bool balancoAtivo = false;
    private float tempoBalanço = 0f;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Mira
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direcao = mousePos - transform.position;
        anguloAlvo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        // Verifica clique do mouse para disparo e balanço
        if (Input.GetMouseButton(0) && Time.time > tempoDeDisparo)
        {
            tempoDeDisparo = Time.time + intervaloDeDisparo;

            // Cria a bala
            GameObject bala = Instantiate(balaPrefab, saidaDoTiro.position, saidaDoTiro.rotation);
            bala.GetComponent<Rigidbody2D>().linearVelocity = saidaDoTiro.right * velocidadeBala;

            // Ativa o balanço
            balancoAtivo = true;
            tempoBalanço = 0f;
        }

        // Calcula rotação da arma
        anguloAtual = anguloAlvo;

        // Aplica balanço se ativo
        if (balancoAtivo)
        {
            tempoBalanço += Time.deltaTime;
            float proporcao = tempoBalanço / duracaoBalanço;
            // Movimenta a arma com uma função senoidal para suavidade
            float offsetBalanço = Mathf.Sin(proporcao * Mathf.PI) * intensidadeBalanço;
            anguloAtual += offsetBalanço;

            if (tempoBalanço >= duracaoBalanço)
            {
                balancoAtivo = false; // finaliza o balanço
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, anguloAtual);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 1f);
    }
}
