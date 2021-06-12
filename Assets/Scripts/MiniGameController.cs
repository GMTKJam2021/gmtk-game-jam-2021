using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour
{
    /// <summary>Exits the minigame with the given result.</summary>
    /// <param name="result">True if a player won, false if they lost or cancelled.</param>
    public void EndMiniGame(bool result)
    {
        FindObjectOfType<ScoreKeeper>().MiniGameEnd(result);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
