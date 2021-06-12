using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private SaveData data;
    private int currentScore;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text currentScoreText;

    private void Awake()
    {
        data = SaveSystem.Load();
        highScoreText.text = "HighScore: " + data.highScore;
        currentScoreText.text = "Score: 0";
    }

    /// <summary> Adds points to the score</summary>
    /// <param name="points">Number of points to add</param>
    public void AddPoints(int points)
    {
        currentScore += points;
        currentScoreText.text = "Score: " + currentScore;
        if(currentScore > data.highScore)
            highScoreText.text = "HighScore: " + currentScore;
    }

    /// <summary> Resets the score to 0</summary>
    public void ResetPoints()
    {
        currentScore = 0;
        currentScoreText.text = "Score: 0";
    }
}
