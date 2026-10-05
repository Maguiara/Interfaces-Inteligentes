
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class desplazamientoCubo : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 0, 1);

    public float speed = 2.0f; 
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float nuevoX = moveDirection.x * speed * Time.deltaTime;
        float nuevoY = moveDirection.y * speed * Time.deltaTime;
        float nuevoZ = moveDirection.z * speed * Time.deltaTime;
        transform.Translate(nuevoX, nuevoY, nuevoZ);
    }
}
