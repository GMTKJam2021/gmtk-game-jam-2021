using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProblemLocator : MonoBehaviour
{
    public int numberProbsToShow = 5;

    public ProblemArrow arrowPrefab;

    List<Problem> problems2PointAt = new List<Problem>();

    public void AddProblem(Problem prob){
        problems2PointAt.Add(prob);
    }
    public void RemoveProblem(Problem prob){
        problems2PointAt.Remove(prob);
        //Debug.Log("problems:"+problems2PointAt.Count);
    }

    void Start(){
        for(int i=0; i<numberProbsToShow; i++){
            ProblemArrow go = Instantiate(arrowPrefab);
            go.transform.SetParent(this.transform);
            go.transform.localPosition = new Vector3();
            go.transform.localScale = new Vector3(1f,1f,0f);
        }
        
        // Problem[] probs = FindObjectsOfType<Problem>();
        // problems2PointAt.AddRange(probs);
    }
    void Update(){
        System.Array.ForEach(GetComponentsInChildren<ProblemArrow>(),p=>p.target=null);

        int i=0;
        problems2PointAt
            .OrderBy(p=>Vector3.Distance(p.transform.position,this.transform.position))
            .Take(numberProbsToShow)
            .ToList()
            .ForEach(p=>{
                GetComponentsInChildren<ProblemArrow>()[i++].target = p;
            });

    }
}
