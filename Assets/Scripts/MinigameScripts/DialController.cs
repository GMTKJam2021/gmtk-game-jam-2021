using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialController : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    private bool dragging;
    private Vector3 mousePosition = Vector3.zero;
    public float rotation = 0;
    public bool unlocked = false;

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (dragging && !unlocked)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
            transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            rotation = transform.rotation.eulerAngles.z;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }
}
