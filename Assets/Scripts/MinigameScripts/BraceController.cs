using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BraceController : MonoBehaviour, IPointerClickHandler
{

    private bool broken = true;
    [SerializeField] private Sprite unBrokenSprite;
    [SerializeField] private Sprite fixedSprite;
    [SerializeField] private Image image;

    public void unBroken()
    {
        broken = false;
        image.sprite = unBrokenSprite;
        BraceGame.brokenCount--;
        print(gameObject.name + " is not broken");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (broken)
        {
            broken = false;
            image.sprite = fixedSprite;
            BraceGame.brokenCount--;
            print(gameObject.name + " is fixed");
        }
    }
}
