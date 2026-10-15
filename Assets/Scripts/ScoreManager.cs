
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

    private int score = 0;
    private int lives = 5;

    // Sets the shared instance and initializes the score display.
    void Awake()
    {
        Instance = this;
        UpdateScoreText();
    }

    // Adds points to the score and refreshes the display.
    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Removes a life and resets the score and lives when none remain.
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

    // Updates the on-screen score if a text component is assigned.
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
}






