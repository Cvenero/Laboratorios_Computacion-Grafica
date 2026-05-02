using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game2D");
    }
    public void Exit()
    {
        Debug.Log("Salir de juego");
        Application.Quit();
    }
}
