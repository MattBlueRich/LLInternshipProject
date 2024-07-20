using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int currentScore;
    public static ScoreManager instance; // This allows the script to be accessed from outside scripts.
    public static int savedScore = 0;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // Carries on the score from the previous level.
        currentScore = savedScore;

        // Set the score text's default value.
        scoreText.text = currentScore.ToString("0000000000");
    }

    // This function updates the player's current score by scoreValue, given from each Item Drop's ItemDrop.cs.
    public void UpdateScore(int scoreValue)
    {
        // Add to the current score.
        currentScore += scoreValue;
        // Update the score text.
        scoreText.text = currentScore.ToString("0000000000");
    }

    public void SaveScore()
    {
        savedScore = currentScore;
    }

    public void ResetScore()
    {
        savedScore = 0;
    }
}
