using UnityEngine;

public class Criarinimigos : MonoBehaviour
{
    public GameObject[] tiposDeInimigos; // arraste 3 prefabs diferentes
    public Transform[] pontosDeSpawn;    // posições específicas no mapa
    public float intervalo = 3f;

    void Start()
    {
        InvokeRepeating(nameof(GerarInimigo), 2f, intervalo);
    }

    void GerarInimigo()
    {
        if (pontosDeSpawn.Length == 0 || tiposDeInimigos.Length == 0)
            return;

        Transform ponto = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
        GameObject prefab = tiposDeInimigos[Random.Range(0, tiposDeInimigos.Length)];

        Instantiate(prefab, ponto.position, Quaternion.identity);
    }
}
