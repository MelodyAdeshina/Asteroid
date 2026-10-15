
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

    private int score = 0;
    private int lives = 5;

    void Awake()
    {
        Instance = this;
        UpdateScoreText();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void LoseLife()
    {
        lives--;

        if (lives <= 0)
        {
            score = 0;
            lives = 5;
            UpdateScoreText();
        }
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}





