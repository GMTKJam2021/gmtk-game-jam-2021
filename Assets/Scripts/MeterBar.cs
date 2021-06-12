using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterBar : MonoBehaviour
{
    Slider slider;

    void Awake(){
        slider = GetComponent<Slider>();
    }

    public void SetMaxAmount(float amount){
        slider.maxValue = amount;
    }
    public void SetAmount(float amount){
        slider.value = amount;
    }
}
