using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Problem))]
public class ModuleState : MonoBehaviour
{
    public bool moduleFixed;
    public MiniGameWindow miniGameWindow;
    private ScoreKeeper scorekeeper;
    public int modulePoints;

    public Problem beacon;

    public int maxhp = 5;
    public int hp = 0;

    public Sprite undamagedSprite;
    public Sprite repairedSprite;
    public Sprite damagedSprite;
    public Sprite brokenSprite;
    public SpriteRenderer visual;

    public static int brokenModules;
    [SerializeField] private string miniGameName = "TestMinigame";

    // Start is called before the first frame update
    void Start()
    {
        visual = GetComponentInChildren<SpriteRenderer>();
        scorekeeper = FindObjectOfType<ScoreKeeper>();
        miniGameWindow = FindObjectOfType<MiniGameWindow>();
        beacon = GetComponent<Problem>();
        beacon.SetStatus(!moduleFixed);

        Debug.Assert(visual != null);
        UpdateVisual(State.Undamaged);
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
            UpdateVisual(State.Repaired);
            moduleFixed = true;
            brokenModules--;
            if (brokenModules == 0)
                scorekeeper.Win();
            hp = maxhp;
            scorekeeper.AddPoints(modulePoints);
            beacon.ProblemSolved();
            return;
        }
        Debug.Log(gameObject.name + " is still broken.");
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            hp = 0;
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
            UpdateVisual(State.Damaged);
            moduleFixed = false;
            brokenModules++;
            Debug.Log(gameObject.name + " is now broken.");
            beacon.RequestHelp();
            return;
        }
        Debug.Log(gameObject.name + " is already broken.");
    }

    public void UpdateVisual(State state)
    {
        switch (state)
        {
            case State.Undamaged:
                visual.sprite = undamagedSprite;
                break;
            case State.Damaged:
                visual.sprite = damagedSprite;
                break;
            case State.Repaired:
                visual.sprite = repairedSprite;
                break;
            case State.Broken:
                visual.sprite = brokenSprite;
                break;
        }
    }

    public enum State
    {
        Undamaged,
        Damaged,
        Repaired,
        Broken
    }
}
