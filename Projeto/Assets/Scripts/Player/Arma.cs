using UnityEngine;

public class Arma : MonoBehaviour
{
    public Transform saidaDoTiro;
    public GameObject balaPrefab;
    public float intervaloDeDisparo = 0.25f;
    public float velocidadeBala = 10f;

    // Balanço visual
    public float intensidadeBalanço = 20f;
    public float duracaoBalanço = 0.1f;

    // Áudio
    public AudioClip somDoTiro;       // Coloque o AudioClip no inspector
    private AudioSource audioSource;   // Componente para tocar o som

    private float tempoDeDisparo;
    private Camera cam;
    private float anguloAlvo;
    private float anguloAtual;
    private bool balancoAtivo = false;
    private float tempoBalanço = 0f;

    void Start()
    {
        cam = Camera.main;

        // Adiciona AudioSource se não houver
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
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

            // Toca o som do tiro
            if (somDoTiro != null)
                audioSource.PlayOneShot(somDoTiro);

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
            float offsetBalanço = Mathf.Sin(proporcao * Mathf.PI) * intensidadeBalanço;
            anguloAtual += offsetBalanço;

            if (tempoBalanço >= duracaoBalanço)
                balancoAtivo = false;
        }

        transform.rotation = Quaternion.Euler(0, 0, anguloAtual);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * 1f);
    }
}
