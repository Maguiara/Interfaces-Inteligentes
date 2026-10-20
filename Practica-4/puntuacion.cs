using TMPro;
using UnityEngine;
public class puntuacion : MonoBehaviour
{
    public Ejercicio5 notificador;
    private int points = 0;
    void Start()
    {
      notificador.OnPuntuacion += MostrarPuntuacion;
      notificador.OnRecompensa += MostrarRecompensa;
    }

    // Ejercicio 6
  void MostrarPuntuacion()
    {
      TextMeshProUGUI puntuacionText = GetComponent<TextMeshProUGUI>();
      points = Ejercicio5.points;
      puntuacionText.SetText("{0}", points);
    }

  // Ejercicio 7
  void MostrarRecompensa()
  {
    TextMeshProUGUI puntuacionText = GetComponent<TextMeshProUGUI>();
    puntuacionText.SetText("¡Felicidades! Has alcanzado " + Ejercicio5.points + " puntos.");
  }
  
}
