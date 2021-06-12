using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CanvasGroup startButton;
    public CanvasGroup loadingBar;
    AsyncOperation loadingOp;

    public string startingScene;
    public void StartGame(){
        loadingOp = SceneManager.LoadSceneAsync(startingScene);
        Hide(startButton);
        Show(loadingBar);
    }
    void Update(){
        if(loadingOp != null){
            Debug.Log("loading");
            //loadingBar.GetChild(0).GetChild(0).GetComponent<RectTransform>().
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
