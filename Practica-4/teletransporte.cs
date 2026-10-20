using UnityEngine;

public class teletransporte : MonoBehaviour
{
    public Ejercicio1Aprox notificador;
    public Transform escudo;
    public Transform escudo2;


    void Start()
    {
        if (CompareTag("tipo 1"))
        {
            notificador.OnTriggerCubo += teletransporteTipo1;
        }
        if (CompareTag("tipo 2"))
        {
            notificador.OnTriggerCubo += orientacionTipo2HaciaEscudo;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void teletransporteTipo1()
    {
      transform.position = escudo.position;
    }

    void orientacionTipo2HaciaEscudo()
    {
        Vector3 direccion = (escudo2.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direccion);
    }

  
}
