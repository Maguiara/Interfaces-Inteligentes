using UnityEngine;

public class Ejercicio2 : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Ha colisionado con: " + collision.gameObject.name);

        Renderer rendererObjeto = collision.gameObject.GetComponent<Renderer>();

        if (rendererObjeto != null && collision.gameObject.name != "Plane")
        {
            rendererObjeto.material.color = Random.ColorHSV();
        }
    }
}