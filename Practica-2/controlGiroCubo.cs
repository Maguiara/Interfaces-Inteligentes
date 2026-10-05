using UnityEngine;

public class controlGiroCubo : MonoBehaviour
{
    public float speed = 10.0f; // Velocidad de movimiento


    // Update is called once per frame
    void Update()
    {
        float movimientoHorizontal = Input.GetAxis("Horizontal") * 500.0f * Time.deltaTime; // Velocidiad de giro en el eje Y
        transform.Rotate(0, movimientoHorizontal, 0); // Gira el cubo alrededor del eje Y según la entrada del usuario
        transform.Translate(0, 0, speed * Time.deltaTime); // Se mueve hacia delante (eje z positivo)

        Debug.DrawRay(transform.position, transform.forward * 5, Color.red); // Dibuja un rayo rojo en la dirección del movimiento del cubo
    }
}
