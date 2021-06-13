using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordGame : MonoBehaviour
{
    [SerializeField] private TMP_Text[] dials;
    [SerializeField] private TMP_Text passwordNote;
    private int[] password = new int[5];
    private int[] entry = { 0, 0, 0, 0, 0 };

    private void Start()
    {
        for (int i = 0; i < 5; i++)
            password[i] = Random.Range(0, 9);
        passwordNote.text = "Password: " + password[0] + password[1] + password[2] + password[3] + password[4];
    }

    public void ClickDial(int dialIndex)
    {
        //Moves Dial
        if (entry[dialIndex]++ >= 9)
            entry[dialIndex] = 0;
        dials[dialIndex].text = entry[dialIndex].ToString();

        //Checks Code
        for (int i = 0; i < 5; i++)
        {
            if (entry[i] != password[i])
                return;
        }
        FindObjectOfType<MiniGameController>().EndMiniGame(true);
    }

}
