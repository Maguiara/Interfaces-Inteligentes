using UnityEngine;

public class movimientoFlechas : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad de movimiento
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        float movimientoHorizontal = 0;
        float movimientoVertical = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
            movimientoHorizontal = -1;

        if (Input.GetKey(KeyCode.RightArrow))
            movimientoHorizontal = 1;

        if (Input.GetKey(KeyCode.UpArrow))
            movimientoVertical = 1;

        if (Input.GetKey(KeyCode.DownArrow))
            movimientoVertical = -1;

        transform.Translate(movimientoHorizontal * speed, movimientoVertical * speed, 0);
    }
}
