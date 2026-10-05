using UnityEngine;

public class CalculoVelocidadCubo : MonoBehaviour
{
    public float velocidad = 5.0f;

    void Update()
    {
         // si usamos GetAxis normal se produce un "suavizado", con Raw nos da el valor -1 1 de golpe
        float valorHorizontal = Input.GetAxisRaw("Horizontal");
        float valorVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.UpArrow))
        {
            float resultado = velocidad * valorVertical;
            Debug.Log("Flecha Arriba pulsada. Resultado: " + resultado);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            float resultado = velocidad * valorVertical;
            Debug.Log("Flecha Abajo pulsada. Resultado: " + resultado);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float resultado = velocidad * valorHorizontal;
            Debug.Log("Flecha Derecha pulsada. Resultado: " + resultado);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            float resultado = velocidad * valorHorizontal;
            Debug.Log("Flecha Izquierda pulsada. Resultado: " + resultado);
        }
    }
}