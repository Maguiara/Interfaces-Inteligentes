using UnityEngine;

public class figuratofigura : MonoBehaviour
{
    public Ejercicio1 notificador;
    public Ejercicio2 notificador2;
   
    public float velocidad = 2f;
    public Transform objetivo1;
    public Transform escudo;
    private bool desplazando;

    private Transform destino;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (CompareTag("tipo 1"))
        {
            notificador.OnContactoConCubo += moviminentoTipo1;
            notificador2.OnContactoTipo2 += movimientoEscudo;
        }
        if (CompareTag("tipo 2"))
        {
            notificador.OnContactoConCubo += movimientoTipo2;
            notificador2.OnContactoTipo1 += movimientoEscudo;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (desplazando)
        {
            Vector3 direccion = (destino.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(direccion);
            rb.MovePosition(transform.position + direccion * velocidad * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, destino.position) < 0.1f)
                desplazando = false;
        }
    }

    void moviminentoTipo1()
    {
      destino = objetivo1;
      desplazando = true;
    }

    void movimientoTipo2()
    {
        destino = notificador.transform;
        desplazando = true;
    }

    void movimientoEscudo()
    {
        destino = escudo;
        desplazando = true;
    }
}
