using UnityEngine;

public class movimientoWASD : MonoBehaviour
{

    public float speed = 2.0f; // Velocidad de movimiento
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        float movimientoHorizontal = 0;
        float movimientoVertical = 0;

        if (Input.GetKey(KeyCode.W))
            movimientoVertical = 1;

        if (Input.GetKey(KeyCode.S))
            movimientoVertical = -1;

        if (Input.GetKey(KeyCode.A))
            movimientoHorizontal = -1;

        if (Input.GetKey(KeyCode.D))
            movimientoHorizontal = 1;

        transform.Translate(movimientoHorizontal * speed * Time.deltaTime, movimientoVertical * speed * Time.deltaTime, 0);
    }
}
