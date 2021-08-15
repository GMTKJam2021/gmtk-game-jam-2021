using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameWindow : MonoBehaviour
{
    
    /// <summary>
    /// 0 - window is not open
    /// 1 - window is opening
    /// 2 - window is open
    /// 3 - window is closing
    /// </summary>
    public int windowState;

    private Scene scene;
    private MiniGameController miniGame;
    private Transform ret;
    private ModuleState currentModule;
    [SerializeField] private float miniGameWidth = .8f;
    [SerializeField] private CursorController cursor;


    public void OnMinigameLoaded(Scene scene, LoadSceneMode mode, MiniGameController miniGameController)
    {
        miniGame = miniGameController;
        windowState = 1;
    }

    void Update()
    {
        switch (windowState)
        {
            //Nothing happens while the window is closed.
            case 0:
                break;
            //Keeps opening the window until it's fully open.
            case 1:
                WindowOpen();
                break;
            //Nothing happens while the window is open.
            case 2:
                break;
            //Keeps closing the window until it's fully closed.
            case 3:
                WindowClose();
                break;
        }
    }

    /// <summary>
    /// Loads up the assigned minigame.
    /// </summary>
    public void LoadMinigame(String sceneName, ModuleState newModule)
    {
        currentModule = newModule;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // Loads the scene for the minigame
        windowState = 1; // Starts opening the window
    }

    private void WindowOpen()
    {
        cursor.Normal();
        transform.localScale += new Vector3(1f, 1f, 0) * Time.deltaTime * 2f;
        if (transform.localScale.x >= miniGameWidth)
            windowState = 2; // Marks window as fully open
    }
    public void MiniGameEnd(bool result)
    {
        if(currentModule != null)
        {
            Debug.Log("Close Window");
            currentModule.FixResult(result);
            windowState = 3; // Starts closing window
        }
    }
    private void WindowClose()
    {
        transform.localScale -= new Vector3(1f, 1f, 0) * Time.deltaTime * 2f;
        if (transform.localScale.x < 0.01f)
        {
            if (!scene.IsValid()) return;
            miniGame.transform.SetParent(ret);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        try
        {
            scene = s;
            miniGame = scene.GetRootGameObjects()[0].GetComponent<MiniGameController>();
            ret = scene.GetRootGameObjects()[1].transform;

            miniGame.transform.SetParent(transform);
            miniGame.transform.localPosition = new Vector3();
            miniGame.transform.localScale = new Vector3(1f, 1f, 1f);

            OnMinigameLoaded(s, mode, miniGame);
        }
        catch (Exception ex)
        {
            Debug.Log(scene.GetRootGameObjects().Length);
            Debug.Log(ex);
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneUnloaded(Scene current)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        windowState = 0; // Marks window as fully closed
    }
}
