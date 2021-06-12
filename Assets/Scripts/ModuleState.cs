using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleState : MonoBehaviour
{
    public bool moduleFixed;
    private SpriteRenderer sRend;

    // Start is called before the first frame update
    void Start()
    {
        sRend = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Sets to module to the fixed state.
    /// </summary>
    public void Fix()
    {
        if (moduleFixed)
        {
            Debug.Log(gameObject.name + " is already fixed");
            return;
        }
        Debug.Log(gameObject.name + " is now fixed");
        moduleFixed = true;
        sRend.color = Color.green;
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
            Debug.Log(gameObject.name + " is now broken");
            return;
        }
        Debug.Log(gameObject.name + " is already broken");
    }

    public enum State
    {
        Undamaged,
        Damaged,
        Repaired,
        Broken
    }
}
