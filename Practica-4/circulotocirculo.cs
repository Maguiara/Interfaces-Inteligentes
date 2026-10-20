using UnityEngine;

public class circulotocirculo : MonoBehaviour
{
    public Ejercicio1 notificador;
   
    public float velocidad = 2f;
    public Transform objetivo1;
    private bool desplazando;

    private Transform destino;

    void Start()
    {
        if (CompareTag("tipo 1"))
        {
            notificador.OnContactoConCubo += moviminentoTipo1;
        }
        if (CompareTag("tipo 2"))
        {
            notificador.OnContactoConCubo += movimientoTipo2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (desplazando)
        {
            Vector3 direccion = (destino.position - transform.position).normalized;
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
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
}
