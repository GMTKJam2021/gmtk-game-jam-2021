using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTank : MonoBehaviour
{

    public MeterBar oxygenGauge;

    /// <summary> The max amount of oxygen the tank can hold</summary>
    [SerializeField] private float oxygenMax = 50f;
    /// <summary> The amount of oxygen remaining</summary>
    [SerializeField] float oxygenRemaining = 50f;
    /// <summary> The rate at which the oxygen refills</summary>
    [SerializeField] private float refillRate = 5f;
    /// <summary> The rate at which the oxygen depletes</summary>
    [SerializeField] private float usageRate = 2f;

    /// <summary> Refills the oxygen using the time provided and the refill rate</summary>
    /// <param name="timeDelta">The amount of time since the last refill point</param>
    public void ReplenishOxygen(float timeDelta)
    {
        if(oxygenRemaining < oxygenMax)
        {
            oxygenRemaining += timeDelta * refillRate;
            if (oxygenRemaining>= oxygenMax)
            {
                oxygenRemaining = oxygenMax;
                Debug.Log("Oxygen Tank Full");
            }
        }

        if(oxygenGauge)
            oxygenGauge.SetAmount(oxygenRemaining);
    }


    /// <summary> Depletes the oxygen using the time provided and the usage rate</summary>
    /// <param name="timeDelta">The amount of time since the last moment of depletion</param>
    public void DepleteOxygen(float timeDelta)
    {
        if (oxygenRemaining > 0)
        {
            oxygenRemaining -= timeDelta * usageRate;
            if (oxygenRemaining <= 0)
            {
                oxygenRemaining = 0;
                Debug.Log("Oxygen Tank Empty");
            }
        }

        if(oxygenGauge)
            oxygenGauge.SetAmount(oxygenRemaining);
    }

    void Start(){
        if(oxygenGauge)
            oxygenGauge.SetMaxAmount(oxygenMax);
    }

}
