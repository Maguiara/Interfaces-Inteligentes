using UnityEngine;

public class ejercicio6 : MonoBehaviour
{
    private GameObject objetivo;
    private float velocidad = 5.0f;
    private GameObject obstaculo;

    void Start()
    {
        objetivo = GameObject.Find("Cylinder");
        obstaculo = GameObject.Find("Cube (1)");
    }

    void Update()
    {
        if (objetivo != null)
        {
            Vector3 direccion = (objetivo.transform.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
        }

        // Cuando deteca el obstaculo, cambia el color del obstaculo a rojo
        if (obstaculo != null)
        {
            float distancia = Vector3.Distance(transform.position, obstaculo.transform.position);
            if (distancia < 1.0f)
            {
                Renderer rendererObjeto = obstaculo.GetComponent<Renderer>();
                if (rendererObjeto != null)
                {
                    rendererObjeto.material.color = Color.red;
                }
            }
        }
    }
}
