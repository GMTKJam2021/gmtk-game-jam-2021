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
        if(highScoreText)
            highScoreText.text = "HighScore: " + data.highScore;
        if(currentScoreText)
            currentScoreText.text = "Score: 0";
    }

    /// <summary> Adds points to the score</summary>
    /// <param name="points">Number of points to add</param>
    public void AddPoints(int points)
    {
        currentScore += points;

        if(currentScoreText)
            currentScoreText.text = "Score: " + currentScore;
        if(currentScore > data.highScore && highScoreText!=null)
            highScoreText.text = "HighScore: " + currentScore;
    }

    /// <summary> Resets the score to 0</summary>
    public void ResetPoints()
    {
        currentScore = 0;
        if(currentScoreText)
            currentScoreText.text = "Score: 0";
    }
}
