using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorWaringController : MonoBehaviour
{
    public float warningDuration;
    public DebrisField debrisField;

    public SpriteRenderer icon;
    public Transform arm;

    float time;

    public void DisplayWarning(Vector2 direction){
        DisplayWarning(Vector2.SignedAngle(Vector2.up,direction));
    }
    public void DisplayWarning(float direction){
        icon.enabled=true;
        arm.transform.localEulerAngles = new Vector3(0f,0f,direction);
        time = warningDuration;
    }

    void Update(){
        time-=Time.deltaTime;

        if(time<=0f)
            HideWarning();
    }

    public void HideWarning(){
        icon.enabled=false;
    }
}
