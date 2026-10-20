using UnityEngine;

public class Ejercicio5 : MonoBehaviour
{
    public static int points = 0;
    public delegate void puntuacion();
    public event puntuacion OnPuntuacion;
    public delegate void recompensa();
    public event recompensa OnRecompensa;

    void Start()
    {
        points = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tipo 1"))
        {
            points += 5;
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("tipo 2"))
        {
            points += 10;
            Destroy(other.gameObject);
        }
        
        //Ejercicio 6
        OnPuntuacion();

        //Ejercicicio 7
        if (points % 100 == 0 && points != 0) {
            OnRecompensa();
        }
    }
}
