using UnityEngine;

public class SalirDelJuego : MonoBehaviour
{
    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit(); //Salimos de la escena
    }
}