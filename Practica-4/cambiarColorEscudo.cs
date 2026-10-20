using UnityEngine;

public class cambiarColorEscudo : MonoBehaviour
{

    // Update is called once per frame
    void OnCollisionEnter(Collision collision)
    {
        GetComponent<Renderer>().material.color = Random.ColorHSV();
    }
}
