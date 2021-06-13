using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour
{
    private SaveData data;
    private int currentScore;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMP_Text newHighScoreText;
    [SerializeField] private TMP_Text finalScoreText;


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

    /// <summary> Switches to game over screen.</summary>
    /// <param name="reason">The reason they lost.</param>
    public void GameOver(string reason)
    {
        gameOverScreen.SetActive(true);
        gameOverText.text = reason;
    }

    /// <summary> Switches to win screen and saves high scores.</summary>
    public void Win()
    {
        winScreen.SetActive(true);
        if (currentScore > data.highScore)
        {
            data.highScore = currentScore;
            newHighScoreText.gameObject.SetActive(true);
            SaveSystem.Save(data);
        }
        finalScoreText.text = "Final Score: " + currentScore;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
