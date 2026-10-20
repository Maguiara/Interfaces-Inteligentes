using UnityEngine;

public class Ejercicio1Aprox : MonoBehaviour
{
    public delegate void TriggerCubo();
    public event TriggerCubo OnTriggerCubo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cubo"))
        {
            OnTriggerCubo();
        }
    }
}

