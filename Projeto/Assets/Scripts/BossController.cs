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

    private bool podeAgir = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        vidaAtual = vidaMaxima;
        estadoAtual = EstadoDoBoss.Parado;

        // Ajustando slider de vida
        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMaxima;
            sliderVida.value = vidaAtual;
        }

        StartCoroutine(MaquinaDeEstados());
    }

    IEnumerator MaquinaDeEstados()
    {
        while (vidaAtual > 0)
        {
            if (!podeAgir) yield return null;

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

        while (tempoPerseguindo > 0f)
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

        Vector2 direcaoDash = (jogador.position - transform.position).normalized;

        yield return new WaitForSeconds(tempoDePreparacaoDoDash);

        estadoAtual = EstadoDoBoss.Dash;
        rb.linearVelocity = direcaoDash * forcaDoDash;

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
            Vector2 direcao = (jogador.position - transform.position).normalized;
            DispararProjetil(direcao);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void DispararProjetil(Vector2 direcao)
    {
        GameObject projetil = Instantiate(projetilPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rbProjetil = projetil.GetComponent<Rigidbody2D>();
        rbProjetil.linearVelocity = direcao * velocidadeProjetil;
    }

    public void ReceberDano(float quantidade)
    {
        vidaAtual -= quantidade;

        if (sliderVida != null)
            sliderVida.value = vidaAtual;

        if (vidaAtual <= 0)
            Morrer();
    }

    void Morrer()
    {
        StopAllCoroutines();
        if (sliderVida != null)
            sliderVida.gameObject.SetActive(false);

        Destroy(gameObject);
    }
}
