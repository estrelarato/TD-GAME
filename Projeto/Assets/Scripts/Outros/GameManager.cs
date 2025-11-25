using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public int pontosParaBoss = 100;
    public bool bossSpawnado = false;


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


            Criarinimigos[] spawners = FindObjectsOfType<Criarinimigos>();
            foreach (var s in spawners)
                s.PararSpawn();


            Instantiate(bossPrefab, localSpawnBoss.position, Quaternion.identity);


            if (AudioManager.instance != null)
            {
                AudioManager.instance.TrocarMusica(AudioManager.instance.musicaBoss);
            }
        }
    }
}
