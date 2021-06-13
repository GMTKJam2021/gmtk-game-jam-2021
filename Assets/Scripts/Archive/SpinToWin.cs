using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinToWin : MonoBehaviour
{
    public MinigameRoot root;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles+=Vector3.forward*root.deltaTime*180f;
    }
}
