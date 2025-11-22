using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configuração de Pontos")]
    public int pontosParaBoss = 100;
    public bool bossSpawnado = false;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform localSpawnBoss;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void VerificarBoss(int pontuacaoAtual)
    {
        if (!bossSpawnado && pontuacaoAtual >= pontosParaBoss)
        {
            bossSpawnado = true;

            // Para todos os spawners da cena
            Criarinimigos[] spawners = FindObjectsOfType<Criarinimigos>();
            foreach (var s in spawners)
                s.PararSpawn();

            // Spawn do boss
            Instantiate(bossPrefab, localSpawnBoss.position, Quaternion.identity);

            // Troca de música do boss
            if (AudioManager.instance != null)
            {
                AudioManager.instance.TrocarMusica(AudioManager.instance.musicaBoss);
            }
        }
    }
}
