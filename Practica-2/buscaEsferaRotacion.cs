using UnityEngine;

public class buscaEsferaRotacion : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        GameObject esfera = GameObject.Find("Sphere");
        
        transform.LookAt(new Vector3(esfera.transform.position.x, transform.position.y, esfera.transform.position.z));
        transform.Translate(0, 0, speed * Time.deltaTime); // Mover hacia adelante en la dirección de la rotación
    }
}