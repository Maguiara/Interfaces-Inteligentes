using UnityEngine;

public class Ejercicio1 : MonoBehaviour
{
    public delegate void ContactoConCubo();
    public event ContactoConCubo OnContactoConCubo;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cubo"))
        {
            OnContactoConCubo();
        }
    }
}
