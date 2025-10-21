using UnityEngine;

public class Arma : MonoBehaviour
{
    public Transform saidaDoTiro;
    public GameObject bala;
    public float intevaloDeDisparo = 0.2f;
    
    private float tempoDeDisparo = 0;
    
    private Camera camera;
    public GameObject cursor;
  void Start()
    {
        camera = Camera.main;
    }
   void Update()
    {
         
        if (gameObject.transform.rotation.eulerAngles.z > -90 
            && gameObject.transform.rotation.eulerAngles.z < 90)
        {
            transform.localScale = new Vector3(1 , 1, 1);
        }
        
        if (gameObject.transform.rotation.eulerAngles.z > 90 
            && gameObject.transform.rotation.eulerAngles.z < 270)
        {
            transform.localScale = new Vector3( 1, -1, 1);
        }


        
        
        // Distância da câmera ao objeto. Precisamos disso para fazer o cálculo correto.
        float camDis = camera.transform.position.y - transform.position.y;

        // Obtém a posição do mouse no espaço mundial. Usando camDis para o eixo Z.
        Vector3 mouse = camera.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, camDis));

        float AngleRad = Mathf.Atan2 (mouse.y - transform.position.y, mouse.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;

        transform.rotation =  Quaternion.AngleAxis( angle, Vector3.forward);
       
        Debug.Log("Angilo: "+angle);
        
        cursor.transform.position = new Vector3(mouse.x, mouse.y, cursor.transform.position.z);
        
        Debug.DrawLine(transform.position, mouse , Color.red);
        
        
        if (tempoDeDisparo <=0 && Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            Debug.Log("Bala disparada");
         
            GameObject b = Instantiate (this.bala,saidaDoTiro.position, saidaDoTiro.rotation) as GameObject;
            
            tempoDeDisparo = intevaloDeDisparo;
        }

        if (tempoDeDisparo > 0)
        {
            tempoDeDisparo -= Time.deltaTime;
        }

    }
}
