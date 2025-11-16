using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public enum EstadoDoBoss { Parado, Perseguindo, PreparandoDash, Dash, AtirandoArea, AtirandoDireto }
    private EstadoDoBoss estadoAtual;

    [Header("Referências")]
    public Transform jogador;
    public Slider sliderVida;
    private Rigidbody2D rb;

    [Header("Atributos")]
    public float vidaMaxima = 100f;
    private float vidaAtual;
    public float danoDoBoss = 10f;

    [Header("Movimentação")]
    public float velocidadeMovimento = 2f;
    public float forcaDoDash = 12f;
    public float tempoDePreparacaoDoDash = 1f;
    public float tempoEntreDash = 2.5f;

    [Header("Projeteis")]
    public GameObject projetilPrefab;
    public float quantidadeProjetilArea = 12;
    public float quantidadeProjetilDireto = 5;
    public float velocidadeProjetil = 6f;

    [Header("Feedback de dano")]
    public float flashDuration = 0.25f;
    public int flashPulses = 2;
    public Color flashColor = Color.red;

    private bool podeAgir = true;
    private SpriteRenderer[] spriteRenderers;
    private Color[] originalColors;
    private Coroutine flashCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
        estadoAtual = EstadoDoBoss.Parado;

        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMaxima;
            sliderVida.value = vidaAtual;
        }

        // Captura sprites para flash
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
            originalColors[i] = spriteRenderers[i].color;

        StartCoroutine(MaquinaDeEstados());
    }

    void Update()
    {
        // Se o jogador ainda não existir, tenta encontrá-lo (suporte a spawn tardio)
        if (jogador == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) jogador = p.transform;
        }
    }

    IEnumerator MaquinaDeEstados()
    {
        while (vidaAtual > 0)
        {
            if (jogador == null || !podeAgir) { yield return null; continue; }

            int acao = Random.Range(0, 3);

            switch (acao)
            {
                case 0:
                    yield return StartCoroutine(PerseguirJogador());
                    break;
                case 1:
                    yield return StartCoroutine(AtaqueDash());
                    break;
                case 2:
                    yield return StartCoroutine(AtaqueProjetil());
                    break;
            }
        }
    }

    IEnumerator PerseguirJogador()
    {
        estadoAtual = EstadoDoBoss.Perseguindo;
        float tempoPerseguindo = Random.Range(1.5f, 3.5f);

        while (tempoPerseguindo > 0f && jogador != null)
        {
            Vector2 direcao = (jogador.position - transform.position).normalized;
            rb.linearVelocity = direcao * velocidadeMovimento;
            tempoPerseguindo -= Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
    }

    IEnumerator AtaqueDash()
    {
        estadoAtual = EstadoDoBoss.PreparandoDash;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(tempoDePreparacaoDoDash);

        if (jogador != null)
        {
            estadoAtual = EstadoDoBoss.Dash;
            Vector2 direcaoDash = (jogador.position - transform.position).normalized;
            rb.linearVelocity = direcaoDash * forcaDoDash;
        }

        yield return new WaitForSeconds(0.35f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(tempoEntreDash);
    }

    IEnumerator AtaqueProjetil()
    {
        int tipo = Random.Range(0, 2);

        if (tipo == 0)
            yield return StartCoroutine(DisparoEmArea());
        else
            yield return StartCoroutine(DisparoDireto());

        yield return new WaitForSeconds(1f);
    }

    IEnumerator DisparoEmArea()
    {
        estadoAtual = EstadoDoBoss.AtirandoArea;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < quantidadeProjetilArea; i++)
        {
            float angulo = i * (360f / quantidadeProjetilArea);
            Vector2 direcao = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));
            DispararProjetil(direcao);
        }

        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator DisparoDireto()
    {
        estadoAtual = EstadoDoBoss.AtirandoDireto;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < quantidadeProjetilDireto; i++)
        {
            if (jogador == null) break;
            Vector2 direcao = (jogador.position - transform.position).normalized;
            DispararProjetil(direcao);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void DispararProjetil(Vector2 direcao)
    {
        GameObject projetil = Instantiate(projetilPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rbProjetil = projetil.GetComponent<Rigidbody2D>();
        if (rbProjetil != null)
            rbProjetil.linearVelocity = direcao * velocidadeProjetil;
    }

    // ----------------------------- RECEBER DANO --------------------------------

    public void ReceberDano(float quantidade)
    {
        vidaAtual -= quantidade;

        if (sliderVida != null)
            sliderVida.value = vidaAtual;

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(FlashDamage());

        if (vidaAtual <= 0)
            Morrer();
    }

    private IEnumerator FlashDamage()
    {
        float singlePulse = flashDuration / (flashPulses * 2);

        for (int p = 0; p < flashPulses; p++)
        {
            // vermelho
            for (int i = 0; i < spriteRenderers.Length; i++)
                spriteRenderers[i].color = flashColor;

            yield return new WaitForSeconds(singlePulse);

            // retorna
            for (int i = 0; i < spriteRenderers.Length; i++)
                spriteRenderers[i].color = originalColors[i];

            yield return new WaitForSeconds(singlePulse);
        }

        // segurança
        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].color = originalColors[i];

        flashCoroutine = null;
    }

    // ----------------------------- MORTE --------------------------------

    void Morrer()
    {
        StopAllCoroutines();
        if (sliderVida != null)
            sliderVida.gameObject.SetActive(false);

        Destroy(gameObject);
    }

    // ----------------------------- DANO POR CONTATO --------------------------------

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.CompareTag("Player"))
        {
            Player p = colisao.GetComponent<Player>();
            if (p != null)
                p.LevarDano((int)danoDoBoss);
        }
    }
}
