using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleState : MonoBehaviour
{
    public bool moduleFixed;
    private SpriteRenderer sRend;
    public MiniGameWindow miniGameWindow;
    private ScoreKeeper scorekeeper;
    public int modulePoints;

    [SerializeField] private string miniGameName = "TestMinigame";

    // Start is called before the first frame update
    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
        scorekeeper = FindObjectOfType<ScoreKeeper>();
        miniGameWindow = FindObjectOfType<MiniGameWindow>();
    }

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
            sRend.color = Color.green;
            scorekeeper.AddPoints(modulePoints);
            return;
        }
        Debug.Log(gameObject.name + " is still broken.");
    }

    /// <summary>
    /// Sets the module to the broken state.
    /// </summary>
    public void Break()
    {
        if (moduleFixed)
        {
            moduleFixed = false;
            sRend.color = Color.red;
            Debug.Log(gameObject.name + " is now broken.");
            return;
        }
        Debug.Log(gameObject.name + " is already broken.");
    }
}
