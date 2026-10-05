using UnityEngine;

public class buscaEsfera : MonoBehaviour
{
    public float speed = 2.0f;

    void Update()
    {
        GameObject esfera = GameObject.Find("Sphere");
        Vector3 moveDirection = esfera.transform.position - transform.position;
        moveDirection = moveDirection.normalized; // La velocidad de movimiento se mantiene constante y no depende de la distancia a la esfera
        transform.Translate(moveDirection.x * speed * Time.deltaTime, 0, moveDirection.z * speed * Time.deltaTime);
    }
}