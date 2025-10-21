using UnityEngine;

public class Personagem : MonoBehaviour
{
    [SerializeField] private int vida;
    [SerializeField] private int energia;
    [SerializeField] private float velocidade;

    public void setVida(int vida)
    {
        this.vida = vida;
    }

    public int getVida()
    {
        return this.vida;
    }

    public void setEnergia(int energia)
    {
        this.energia = energia;
    }

    public int getEnergia()
    {
        return this.energia;
    }

    public void setVelocidade(float velocidade)
    {
        this.velocidade = velocidade;
    }
    
    public float getVelocidade()
    {
        return this.velocidade;
    }
}

