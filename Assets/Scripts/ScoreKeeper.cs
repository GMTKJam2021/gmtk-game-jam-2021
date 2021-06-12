using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private SaveData data;
    private int currentScore;
    [SerializeField] private TextMesh highScoreText;
    [SerializeField] private TextMesh currentScoreText;

    private void Awake()
    {
        data = SaveSystem.Load();
        highScoreText.text = "HighScore: " + data.highScore;
        currentScoreText.text = "Score: 0";
    }

    public void AddPoints(int points)
    {
        currentScore += points;
    }
}
