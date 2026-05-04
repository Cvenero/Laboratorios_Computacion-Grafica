using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int score = 0;

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = "Puntos: " + score;
    }
}