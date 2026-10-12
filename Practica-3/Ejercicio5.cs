using UnityEngine;

public class Ejercicio5 : MonoBehaviour
{
    public float fuerza = 10.0f;

    private Rigidbody rigidbodyEsfera;

    void Start()
    {
        rigidbodyEsfera = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            rigidbodyEsfera.AddForce(Vector3.right * fuerza, ForceMode.Impulse);
        }
    }
}