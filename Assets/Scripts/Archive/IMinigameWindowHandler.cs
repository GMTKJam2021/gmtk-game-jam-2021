using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class IMinigameWindowHandler : MonoBehaviour
{
    public abstract void OnMinigameLoaded(Scene minigame, LoadSceneMode mode, MinigameRoot minigameRoot);
    public abstract void OnMinigameUnloaded(Scene current);
}
