using UnityEngine;

public class Criarinimigos : MonoBehaviour
{
    public GameObject[] tiposDeInimigos;
    public Transform[] pontosDeSpawn;
    public float intervalo = 3f;

    private bool spawnAtivo = true;

    void Start()
    {
        InvokeRepeating(nameof(GerarInimigo), 2f, intervalo);
    }

    void GerarInimigo()
    {
        if (!spawnAtivo || GameManager.instance.bossSpawnado)
        {
            PararSpawn();
            return;
        }

        if (pontosDeSpawn.Length == 0 || tiposDeInimigos.Length == 0)
            return;

        Transform ponto = pontosDeSpawn[Random.Range(0, pontosDeSpawn.Length)];
        GameObject prefab = tiposDeInimigos[Random.Range(0, tiposDeInimigos.Length)];

        Instantiate(prefab, ponto.position, Quaternion.identity);
    }

    public void PararSpawn()
    {
        spawnAtivo = false;
        CancelInvoke(nameof(GerarInimigo));
    }
}
