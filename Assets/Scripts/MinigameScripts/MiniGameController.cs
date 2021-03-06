using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameController : MonoBehaviour
{
    public bool run = false;
    public Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = FindObjectOfType<Camera>();
    }

    /// <summary>Exits the minigame with the given result.</summary>
    /// <param name="result">True if a player won, false if they lost or cancelled.</param>
    public void EndMiniGame(bool result)
    {
        FindObjectOfType<MiniGameWindow>().MiniGameEnd(result);
    }
}
