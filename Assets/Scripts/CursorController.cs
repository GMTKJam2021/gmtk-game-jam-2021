using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;



public class CursorController : MonoBehaviour
{
    private SpriteRenderer sRend;
    private Vector2 cursorPosition;
    private Animator anim;
    private bool locked;

    private void Awake()
    {
        locked = false;
        sRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Cursor.visible = false;
        Normal();
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }

    public void Normal()
    {
        transform.up = Vector2.zero;
        anim.SetInteger("State", 0);
        locked = false;
    }

    public void LockedNormal()
    {
        transform.up = Vector2.zero;
        anim.SetInteger("State", 0);
        locked = true;
    }

    public void Target()
    {
        if (locked)
            return;
        transform.up = Vector2.zero;
        anim.SetInteger("State", 1);
    }

    public void Found()
    {
        if (locked)
            return;
        transform.up = Vector2.zero;
        anim.SetInteger("State", 2);
    }
    public void Arrow(Vector2 rotation)
    {
        if (locked)
            return;
        transform.up = rotation;
        anim.SetInteger("State", 3);
    }
    public void Problem()
    {
        if (locked)
            return;
        transform.up = Vector2.zero;
        anim.SetInteger("State", 4);
    }
}
