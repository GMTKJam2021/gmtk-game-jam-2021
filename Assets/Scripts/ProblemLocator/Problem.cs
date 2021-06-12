using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{
    void OnEnable(){
        foreach(ProblemLocator pl in FindObjectsOfType<ProblemLocator>()){
            pl.AddProblem(this);
        }
    }

}
