using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialGame : MonoBehaviour
{
    [SerializeField] private DialController[] dials;
    [SerializeField] private TMP_Text[] timerTexts;
    [SerializeField] private RawImage[] lights;

    private float[] password = new float[6];
    private float[] entry = { 0, 0, 0 };
    private int[] timers = { 0, 0, 0 };

    // Start is called before the first frame update
    void Start()
    {
        print(dials[0].unlocked);
        print(dials[1].unlocked);
        print(dials[2].unlocked);
        for (int i = 0; i < 6; i++)
        {
            float temp = Random.Range(1, 359);
            password[i] = temp - 5;
            print(password[i]);
            i++;
            password[i] = temp + 5;
            print(password[i]);
            print(temp);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dials[0].unlocked && dials[1].unlocked && dials[2].unlocked)
            FindObjectOfType<MiniGameController>().EndMiniGame(true);
        entry[0] = dials[0].rotation;
        entry[1] = dials[1].rotation;
        entry[2] = dials[2].rotation;
        if (!dials[0].unlocked)
            if (entry[0] > password[0] && entry[0] < password[1])
            {
                print("unlocked");
                dials[0].unlocked = true;
                lights[0].color = Color.green;
                timers[0] = 5;
                InvokeRepeating("TimerCountDown0", 0, 1);
            }
        if (!dials[1].unlocked)
            if (entry[1] > password[2] && entry[1] < password[3])
            {
                dials[1].unlocked = true;
                lights[1].color = Color.green;
                timers[1] = 5;
                InvokeRepeating("TimerCountDown1", 0, 1);
            }
        if (!dials[2].unlocked)
            if (entry[2] > password[4] && entry[2] < password[5])
            {
                dials[2].unlocked = true;
                lights[2].color = Color.green;
                timers[2] = 5;
                InvokeRepeating("TimerCountDown2", 0, 1);
            }
    }

    private void TimerCountDown0()
    {
        if(timers[0] == 0)
        {
            CancelInvoke("TimerCountDown0");
            timerTexts[0].text = "-";
            lights[0].color = Color.red;
            dials[0].unlocked = false; 
            float temp = Random.Range(1, 359);
            while (temp > password[0] && temp < password[1])
                temp = Random.Range(1, 359);
            password[0] = temp - 5;
            password[1] = temp + 5;
            return;
        }
        timerTexts[0].text = timers[0].ToString();
        timers[0]--;

    }
    private void TimerCountDown1()
    {
        if (timers[1] == 0)
        {
            CancelInvoke("TimerCountDown1");
            timerTexts[1].text = "-";
            lights[1].color = Color.red;
            dials[1].unlocked = false;
            float temp = Random.Range(1, 359);
            while (temp > password[2] && temp < password[3])
                temp = Random.Range(1, 359);
            password[2] = temp - 5;
            password[3] = temp + 5;
            return;
        }
        timerTexts[1].text = timers[1].ToString();
        timers[1]--;

    }
    private void TimerCountDown2()
    {
        if (timers[2] == 0)
        {
            CancelInvoke("TimerCountDown2");
            timerTexts[2].text = "-";
            lights[2].color = Color.red;
            dials[2].unlocked = false;
            float temp = Random.Range(1, 359);
            while (temp > password[4] && temp < password[5])
                temp = Random.Range(1, 359);
            password[4] = temp - 5;
            password[5] = temp + 5;
            return;
        }
        timerTexts[2].text = timers[2].ToString();
        timers[2]--;

    }
}
