using UnityEngine;

public class movimientoCubo : MonoBehaviour
{
    private Vector3 desplazamiento;

    public GameObject marcadorCubo;

  void Start()
  {
    marcadorCubo = GameObject.Find("marcadorCubo");
  }
  // Update is called once per frame
  void Update()
    {
        desplazamiento = marcadorCubo.transform.position;
        if (Input.GetAxis("Jump") > 0)
        {
            transform.position = desplazamiento;
        }
    }
}
