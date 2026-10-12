using UnityEngine;

public class ejercicio3daño : MonoBehaviour
{
    public int daño = 10;
    public int dañoRecibido = 0;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Personaje")
        {
            Debug.Log("El jugador ha recibido " + daño + " puntos de daño.");
        }
    }
    
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Personaje")
        {
           dañoRecibido += daño;
            
        }
    }
}
