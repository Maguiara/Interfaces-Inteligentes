using UnityEngine;

public class movimientoCapsula : MonoBehaviour
{
    private Vector3 desplazamiento;

    public GameObject marcadorCapsula;

  void Start()
  {
    marcadorCapsula = GameObject.Find("marcadorCapsula");
  }
  // Update is called once per frame
  void Update()
    {
        desplazamiento = marcadorCapsula.transform.position;
        if (Input.GetAxis("Jump") > 0)
        {
            transform.position = desplazamiento;
        }
    }
}
