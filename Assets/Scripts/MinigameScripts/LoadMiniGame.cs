using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMiniGame : MonoBehaviour
{
    public string sceneName;
    public IMinigameWindowHandler window;
    // Start is called before the first frame update

    Scene scene;
    MinigameRoot minigame;
    Transform ret;


    void OnSceneLoaded(Scene s, LoadSceneMode mode){
        try{
            
            scene = s;
            minigame = scene.GetRootGameObjects()[0].GetComponent<MinigameRoot>();
            ret = scene.GetRootGameObjects()[1].transform;

            minigame.transform.SetParent(window.transform);
            minigame.transform.localPosition = new Vector3();
            minigame.transform.localScale = new Vector3(1f,1f,1f);

            window.OnMinigameLoaded(s,mode,minigame);
        }catch (Exception ex){
            Debug.Log(ex);
            minigame = null;
        }
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneUnloaded(Scene current){
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        window.OnMinigameUnloaded(current);
    }

    public void LoadMinigame(){
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
    }
    public void UnloadMinigame(){
        if(!scene.IsValid()) return;
        minigame.transform.SetParent(ret);
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.UnloadSceneAsync(scene);
    }

    


    


}
