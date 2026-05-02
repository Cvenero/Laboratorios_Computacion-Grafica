using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Retry()
    {
        Debug.Log("Reintentar el juego");
        SceneManager.LoadScene("Game2D");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
