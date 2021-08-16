using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup startButton;
    public CanvasGroup loadingBar;
    public CanvasGroup creditsGroup;
    public CanvasGroup controlsGroup;

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

    private SaveData data;
    [SerializeField] private TMP_Text highScore;
    [SerializeField] private Slider controlsSlider;
    [SerializeField] private TMP_Text mControl1;
    [SerializeField] private TMP_Text mControl2;
    [SerializeField] private TMP_Text kControl1;
    [SerializeField] private TMP_Text kControl2;

    //public DebrisField.SpawnMetrics[] meteorStart;

    private void Start()
    {
        data = SaveSystem.Load();
        if(data.highScore > 0)
            highScore.text = data.highScore.ToString();
        if (data.keyBoardControls)
            controlsSlider.value = 1;
            
    }

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
        Hide(controlsGroup);
        Show(creditsGroup);
    }
    public void ShowControls()
    {
        Hide(startButton);
        Hide(loadingBar);
        Show(controlsGroup);
        Hide(creditsGroup);
    }
    public void ShowStartMenu(){
        Show(startButton);
        Hide(loadingBar);
        Hide(controlsGroup);
        Hide(creditsGroup);
    }

    public void ToggleControls(float controlState)
    {
        if (controlState == 0)
        {
            kControl1.color = kControl2.color = Color.grey;
            mControl1.color = mControl2.color = Color.white;
            data.keyBoardControls = false;
        }
        else if (controlState == 1)
        {
            kControl1.color = kControl2.color = Color.white;
            mControl1.color = mControl2.color = Color.grey;
            data.keyBoardControls = true;
        }
        SaveSystem.Save(data);
    }

    void Update(){
        if(loadingOp != null){
            if(pauseElapsed<pauseDuration)
                pauseElapsed += Time.deltaTime;
            else if (wipeElapsed < wipeDuration){
                wipeElapsed += Time.deltaTime;
                debrisField.enabled=false;
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
