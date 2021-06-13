using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;



public class CursorController : MonoBehaviour
{
    private SpriteRenderer sRend;
    private Vector2 cursorPosition;
    [SerializeField] private Sprite cursorNormal;
    [SerializeField] private Sprite cursorArrow;
    [SerializeField] private Sprite cursorTarget;

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
#endif
        return false;
    }

    private void Awake()
    {

        sRend = GetComponent<SpriteRenderer>();
        if (isMobile())
            sRend.enabled = false;
        Cursor.visible = false;
        Normal();
    }

    // Update is called once per frame
    void Update()
    {
        cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }

    public void Arrow(Vector2 rotation)
    {
        transform.up = rotation;
        sRend.sprite = cursorArrow;
    }

    public void Normal()
    {
        transform.up = Vector2.zero;
        sRend.sprite = cursorNormal;
    }

    public void Target()
    {
        transform.up = Vector2.zero;
        sRend.sprite = cursorTarget;
    }

}
