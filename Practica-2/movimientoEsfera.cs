using UnityEngine;

public class movimientoEsfera : MonoBehaviour
{
    private Vector3 desplazamiento;

    public GameObject marcadorEsfera;

  void Start()
  {
    marcadorEsfera = GameObject.Find("marcadorEsfera");
  }
  // Update is called once per frame
  void Update()
    {
        desplazamiento = marcadorEsfera.transform.position;
        if (Input.GetAxis("Jump") > 0)
        {
            transform.position = desplazamiento;
        }
    }
}
