using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMinigameWindow : IMinigameWindowHandler
{
    public LoadMiniGame loader;

    bool shouldLoad = false;
    bool isLoaded = false;
    
    MinigameRoot minigame;

    public override void OnMinigameLoaded(Scene scene, LoadSceneMode mode, MinigameRoot minigameRoot){
        minigame = minigameRoot;
        shouldLoad = true;
    }
    public override void OnMinigameUnloaded(Scene current){
        isLoaded = false;
    }

    void Update()
    {
        if(!shouldLoad && !isLoaded){
            WindowIdle();
        }else if(shouldLoad && !isLoaded){
            WindowOpen();
        }else if(shouldLoad && isLoaded){
            WindowRun();
        }else if(!shouldLoad && isLoaded){
            WindowClose();
        }
    }

    void WindowIdle(){
        if(Input.GetKeyDown(KeyCode.Space)){
            loader.LoadMinigame();
        }
    }
    void WindowOpen(){
        transform.localScale += new Vector3(1f,1f,0) * Time.deltaTime*10f;
        if(transform.localScale.x>1f){
            isLoaded = true;
            minigame.run = true;
        }
    }
    void WindowRun(){
        if( Input.GetKeyDown(KeyCode.Space) ){
            shouldLoad = false;
            minigame.run = false;
            
        }
    }
    void WindowClose(){
        transform.localScale -= new Vector3(1f,1f,0) * Time.deltaTime*10f;
        if(transform.localScale.x<0.01f){
            loader.UnloadMinigame();
        }
    }
}
