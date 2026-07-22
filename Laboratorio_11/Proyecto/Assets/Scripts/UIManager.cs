using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject gameOver;
    public TextMeshProUGUI winLoseText;

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        gameOver.SetActive(false);
    }

    public void ShowGameOver(bool isWin)
    {
        winLoseText.text = isWin ? "GANASTE!" : "PERDISTE!";
        gameOver.SetActive(true);
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

}
