using TMPro;
using UnityEngine;

public class GrabStatusUI : MonoBehaviour
{
    public TMP_Text statusText;
    public string objectName = "Objeto";

    private int grabCount;

    public void OnGrab()
    {
        grabCount++;

        if (statusText != null)
            statusText.text = "Agarrando: " + objectName
                + "\nVeces agarrado: " + grabCount;
    }

    public void OnRelease()
    {
        if (statusText != null)
            statusText.text = "Has soltado: " + objectName
                + "\nVeces agarrado: " + grabCount;
    }
}