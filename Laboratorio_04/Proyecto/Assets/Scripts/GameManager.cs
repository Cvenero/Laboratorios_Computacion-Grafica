using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Canvas")]
    public GameObject canvasMenu;
    public GameObject canvasGameOver;
    public GameObject canvasWin;

    private bool gameStarted = false;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Colision con: " + hit.gameObject.name + " Tag: " + hit.gameObject.tag);

        if (hit.gameObject.CompareTag("WinZone"))
        {
            // Buscar el GameManager directamente en la escena
            GameManager gm = GameObject.FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.WinGame();
            }
            else
            {
                Debug.Log("GameManager no encontrado!");
            }
        }
    }


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        canvasMenu.SetActive(true);
        canvasGameOver.SetActive(false);
        canvasWin.SetActive(false);

        Time.timeScale = 0f;

        // Cursor libre al inicio para poder clickear botones
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        canvasMenu.SetActive(false);
        Time.timeScale = 1f;
        gameStarted = true;

        // Bloquear cursor al iniciar el juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameOver()
    {
        if (!gameStarted) return;
        canvasGameOver.SetActive(true);
        Time.timeScale = 0f;

        // Liberar cursor al mostrar game over
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void WinGame()
    {
        if (!gameStarted) return;
        canvasWin.SetActive(true);
        Time.timeScale = 0f;

        // Liberar cursor al ganar
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}