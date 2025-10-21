using UnityEngine;

public class CriarInimigos : MonoBehaviour
{
   
    public GameObject[] inimigos; 
    public Transform[] posicaoDosInimigos;
    
    public float tempoDoNovoInimigo = 15; //segundos
    
    private float cronometroInimigo = 0;
    
    
    
    void Start()
    {
        
    }

    
    void Update()
    {
        cronometroInimigo += Time.deltaTime;

        if (cronometroInimigo >= tempoDoNovoInimigo)
        {
            Transform posicao = posicaoDosInimigos[Random.Range(0, posicaoDosInimigos.Length)];
           
            GameObject inimigo = Instantiate( 
                inimigos[Random.Range(0, inimigos.Length)], 
                posicao) as GameObject;
            
            cronometroInimigo = 0;
        }
    }
}
