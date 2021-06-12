using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProblemArrow : MonoBehaviour
{
    public Problem target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target!=null){
            Vector2 direction = target.transform.position - transform.position;
            transform.eulerAngles =  new Vector3(0f, 0f, Vector2.SignedAngle(Vector2.up,direction) );
        }
    }
}
