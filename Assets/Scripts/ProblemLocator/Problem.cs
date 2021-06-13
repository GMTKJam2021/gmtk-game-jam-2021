using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Problem : MonoBehaviour
{

    public void RequestHelp(){
        foreach(ProblemLocator pl in FindObjectsOfType<ProblemLocator>()){
            pl.AddProblem(this);
        }
    }
    public void ProblemSolved(){
        foreach(ProblemLocator pl in FindObjectsOfType<ProblemLocator>()){
            pl.RemoveProblem(this);
        }
    }
    public void SetStatus(bool help){
        if(help)
            RequestHelp();
        else
            ProblemSolved();
    }
}
