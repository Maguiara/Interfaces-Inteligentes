using UnityEngine;

public class Ejercicio2 : MonoBehaviour
{
    public delegate void ContactoTipo1();
    public delegate void ContactoTipo2();
    public event ContactoTipo1 OnContactoTipo1;
    public event ContactoTipo2 OnContactoTipo2;

    private void OnCollisionEnter(Collision collision)
    {
      if (collision.gameObject.CompareTag("tipo 1"))
        {
            OnContactoTipo1();
        } 
      if (collision.gameObject.CompareTag("tipo 2"))
        {
            OnContactoTipo2();
        }
    }
}
