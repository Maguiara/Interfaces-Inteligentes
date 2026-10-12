using UnityEngine;

public class Ejercicio1 : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rigidbodyCapsula;
    private Vector3 movimiento;

    void Start()
    {
        rigidbodyCapsula = GetComponent<Rigidbody>();
    }

    void Update()
    {
        movimiento = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // rigidbodyCapsula.MovePosition(rigidbodyCapsula.position + movimiento * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rigidbodyCapsula.MovePosition(rigidbodyCapsula.position + movimiento * speed * Time.fixedDeltaTime);
    }


}