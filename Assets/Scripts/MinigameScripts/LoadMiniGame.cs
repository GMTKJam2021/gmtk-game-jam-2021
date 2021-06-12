using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMiniGame : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update

    Scene scene;
    MinigameRoot minigame;
    Transform ret;

    bool shouldLoad = false;
    bool isLoaded = false;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene s, LoadSceneMode mode){
        try{
            
            scene = s;
            minigame = scene.GetRootGameObjects()[0].GetComponent<MinigameRoot>();
            ret = scene.GetRootGameObjects()[1].transform;

            minigame.transform.SetParent(this.transform);
            minigame.transform.localPosition = new Vector3();
            minigame.transform.localScale = new Vector3(1f,1f,1f);

           shouldLoad = true;
        }catch{
            Debug.Log("Error");
            minigame = null;
        }
    }

    void OnSceneUnloaded(Scene current){
        isLoaded = false;
    }


    // Update is called once per frame
    void Update()
    {
        if(!shouldLoad && !isLoaded){
            if(Input.GetKeyDown(KeyCode.Space)){
                SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            }
        }

        if(shouldLoad && !isLoaded){
            transform.localScale += new Vector3(1f,1f,0) * Time.deltaTime*10f;
            if(transform.localScale.x>1f){
                isLoaded = true;
                minigame.run = true;
            }
        }

        if(shouldLoad && isLoaded){
            if( Input.GetKeyDown(KeyCode.Space) ){
                shouldLoad = false;
                minigame.run = false;
                
            }
        }

        if(!shouldLoad && isLoaded){
            transform.localScale -= new Vector3(1f,1f,0) * Time.deltaTime*10f;
            if(transform.localScale.x<0.01f && scene.IsValid()){
                minigame.transform.SetParent(ret);
                isLoaded=false;
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        
    }
}
