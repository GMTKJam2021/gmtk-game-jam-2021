using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Problem))]
public class ModuleState : MonoBehaviour
{
    public bool moduleFixed;
    private SpriteRenderer sRend;
    public MiniGameWindow miniGameWindow;
    private ScoreKeeper scorekeeper;
    public int modulePoints;

    public Problem beacon;

    public int maxhp = 5;
    public int hp = 0;

    public static int brokenModules;
    [SerializeField] private string miniGameName = "TestMinigame";

    // Start is called before the first frame update
    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        scorekeeper = FindObjectOfType<ScoreKeeper>();
        miniGameWindow = FindObjectOfType<MiniGameWindow>();
        beacon = GetComponent<Problem>();
        beacon.SetStatus(!moduleFixed);
        brokenModules = FindObjectsOfType<ModuleState>().Length;
    }
    
    // bool firstUpdate = true;
    // void FixedUpdate(){
    //     if(firstUpdate){
    //         beacon.SetStatus(moduleFixed)
    //     }
    // }

    public void AttemptFix()
    {
        if (moduleFixed)
        {
            Debug.Log(gameObject.name + " is already fixed");
            return;
        }
        PlayerMouseMovement.inGame = true;
        miniGameWindow.LoadMinigame(miniGameName, this);
    }

    /// <summary>
    /// Sets to module to the fixed state.
    /// </summary>
    public void FixResult(bool isFixed)
    {
        PlayerMouseMovement.inGame = false;
        if (isFixed)
        {
            Debug.Log(gameObject.name + " is now fixed.");
            moduleFixed = true;
            brokenModules--;
            if (brokenModules == 0)
                scorekeeper.Win();
            hp = maxhp;
            sRend.color = Color.green;
            scorekeeper.AddPoints(modulePoints);
            beacon.ProblemSolved();
            return;
        }
        Debug.Log(gameObject.name + " is still broken.");
    }

    public void TakeDamage(int amount){
        hp-= amount;
        if(hp<=0){
            hp=0;
            Break();
        }
    }

    /// <summary>
    /// Sets the module to the broken state.
    /// </summary>
    public void Break()
    {
        if (moduleFixed)
        {
            moduleFixed = false;
            brokenModules++;
            sRend.color = Color.red;
            Debug.Log(gameObject.name + " is now broken.");
            beacon.RequestHelp();
            return;
        }
        Debug.Log(gameObject.name + " is already broken.");
    }

    public enum State
    {
        Undamaged,
        Damaged,
        Repaired,
        Broken
    }
}
