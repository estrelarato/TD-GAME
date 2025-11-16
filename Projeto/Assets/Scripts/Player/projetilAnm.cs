using UnityEngine;

public class ProjetilAnm : MonoBehaviour
{
    public Sprite[] frames;
    public float tempoPorFrame = 0.1f;

    private SpriteRenderer sr;
    private int frameAtual;
    private float tempo;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        tempo += Time.deltaTime;
        if (tempo >= tempoPorFrame)
        {
            tempo = 0f;
            frameAtual = (frameAtual + 1) % frames.Length;
            sr.sprite = frames[frameAtual];
        }
    }
}
