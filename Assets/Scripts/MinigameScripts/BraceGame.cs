using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BraceGame : MonoBehaviour
{
    [SerializeField] private BraceController[] braces = new BraceController[12];
    public static int brokenCount = 12;

    // Start is called before the first frame update
    void Start()
    {
        brokenCount = 12;
        for (int i = 0; i < 12; i++)
        {
            //print("Check" + i);
            if (Random.Range(-1f, 1f) > 0)
                braces[i].unBroken();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (brokenCount == 0)
            FindObjectOfType<MiniGameController>().EndMiniGame(true);
    }
}
