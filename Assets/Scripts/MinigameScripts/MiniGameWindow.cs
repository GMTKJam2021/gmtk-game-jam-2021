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

    private MiniGameWindow window;

    private Scene scene;
    private MinigameRoot minigame;
    private Transform ret;

    private void Start()
    {
        //ret = transform;
    }

    public void OnMinigameLoaded(Scene scene, LoadSceneMode mode, MinigameRoot minigameRoot)
    {
        minigame = minigameRoot;
        minigame.transform.localScale = new Vector3(minigame.transform.localScale.x / minigame.dimensions.x, minigame.transform.localScale.y / minigame.dimensions.y, 1f);
        windowState++;
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
            //Checks to see if the window needs to be closed.
            case 2:
                WindowRun();
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
    public void LoadMinigame(String sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive); // Loads the scene for the minigame
        windowState++; // Starts opening the window
    }

    private void WindowOpen()
    {
        transform.localScale += new Vector3(1f, 1f, 0) * Time.deltaTime * 10f;
        if (transform.localScale.x > 10f)
            windowState++; // Marks window as fully open
    }
    private void WindowRun()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            windowState++; // Starts closing window
    }
    private void WindowClose()
    {
        transform.localScale -= new Vector3(1f, 1f, 0) * Time.deltaTime * 10f;
        if (transform.localScale.x < 0.01f)
        {
            if (!scene.IsValid()) return;
            minigame.transform.SetParent(ret);
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.UnloadSceneAsync(scene);
        }
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        try
        {
            scene = s;
            minigame = scene.GetRootGameObjects()[0].GetComponent<MinigameRoot>();
            ret = scene.GetRootGameObjects()[1].transform;

            minigame.transform.SetParent(window.transform);
            minigame.transform.localPosition = new Vector3();
            minigame.transform.localScale = new Vector3(1f, 1f, 1f);

            window.OnMinigameLoaded(s, mode, minigame);
        }
        catch (Exception ex)
        {
            Debug.Log(scene.GetRootGameObjects().Length);
            Debug.Log(ret);
            Debug.Log(ex);
            minigame = null;
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneUnloaded(Scene current)
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        windowState = 0; // Marks window as fully closed
    }
}
