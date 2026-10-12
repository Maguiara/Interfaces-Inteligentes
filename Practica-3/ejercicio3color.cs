using UnityEngine;

public class ejercicio3color : MonoBehaviour
{
    private Color colorOriginal;
    void OnTriggerEnter(Collider collision)
    {
        Renderer rendererObjeto = collision.gameObject.GetComponent<Renderer>();

        if (rendererObjeto != null && collision.gameObject.name != "Plane")
        {
            colorOriginal = rendererObjeto.material.color;
            rendererObjeto.material.color = Color.green;
        }
    }
    void OnTriggerExit(Collider collision)
    {
        Renderer rendererObjeto = collision.gameObject.GetComponent<Renderer>();

        if (rendererObjeto != null && collision.gameObject.name != "Plane")
        {
            rendererObjeto.material.color = colorOriginal;
        }
    }
}
