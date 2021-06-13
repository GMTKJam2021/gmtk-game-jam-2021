using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup startButton;
    public CanvasGroup loadingBar;
    public CanvasGroup creditsGroup;

    public AudioListener ears;
    public DebrisField debrisField;
    AsyncOperation loadingOp;

    public string startingScene;
    public float pauseDuration = 2f;
    public float wipeDuration = 2f;
    public float unloadDuration = 2f;

    [SerializeField]
    float pauseElapsed = 0f;
    [SerializeField]
    float wipeElapsed = 0f;
    [SerializeField]
    float unloadElapsed = 0f;

    //public DebrisField.SpawnMetrics[] meteorStart;

    public void StartGame(){
        loadingOp = SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Additive);
        loadingOp.allowSceneActivation = false;
        Hide(startButton);
        Show(loadingBar);

        debrisField.spawnOptions[3].enabled=true;
        debrisField.spawnOptions[4].enabled=true;
        debrisField.spawnOptions[5].enabled=true;
    }
    public void ShowCredits(){
        Hide(startButton);
        Hide(loadingBar);
        Show(creditsGroup);
    }
    public void ShowStartMenu(){
        Show(startButton);
        Hide(loadingBar);
        Hide(creditsGroup);
    }
    void Update(){
        if(loadingOp != null){
            if(pauseElapsed<pauseDuration)
                pauseElapsed += Time.deltaTime;
            else if (wipeElapsed < wipeDuration){
                wipeElapsed += Time.deltaTime;
                debrisField.enabled=false;
                Debug.Break();
            }
            else if(unloadElapsed < unloadDuration){
                unloadElapsed += Time.deltaTime;
                ears.enabled=false;
                loadingOp.allowSceneActivation=true;
            }else{
                loadingOp = SceneManager.UnloadSceneAsync("MainMenu");
                //loadingOp.allowSceneActivation=false;
            }
            
        }
    }
    void Hide(CanvasGroup ui){
        ui.alpha=0f;
        ui.blocksRaycasts = false;
    }
    void Show(CanvasGroup ui){
        ui.alpha=1f;
        ui.blocksRaycasts=true;
    }
}
