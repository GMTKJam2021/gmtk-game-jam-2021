using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    /// <summary>The meter that displays how much fuel remains in the tank.</summary>
    public MeterBar fuelGauge;
    /// <summary> The max amount of fuel the tank can hold</summary>
    [SerializeField] private float fuelMax = 100f;
    /// <summary> The amount of fuel remaining</summary>
    public float fuelRemaining = 100f;
    /// <summary> The rate at which the fuel refills</summary>
    [SerializeField] private float refillRate = 5f;
    /// <summary> The rate at which the fuel depletes</summary>
    [SerializeField] private float usageRate = 2f;

    void Start(){
        if(fuelGauge){
            fuelGauge.SetMaxAmount(fuelMax);
            fuelGauge.SetAmount(fuelRemaining);
        }
    }

    /// <summary> Refills the fuel using the time provided and the refill rate</summary>
    /// <param name="timeDelta">The amount of time since the last refill point</param>
    public void ReplenishFuel(float timeDelta)
    {
        if (fuelRemaining < fuelMax)
        {
            fuelRemaining += timeDelta * refillRate;
            if (fuelRemaining >= fuelMax)
            {
                fuelRemaining = fuelMax;
                //Debug.Log("Fuel Tank Full");
            }
        }

        if (fuelGauge)
            fuelGauge.SetAmount(fuelRemaining);
    }


    /// <summary> Depletes the fuel using the time provided and the usage rate</summary>
    /// <param name="timeDelta">The amount of time since the last moment of depletion</param>
    public void DepleteFuel(float timeDelta)
    {
        if (fuelRemaining > 0)
        {
            fuelRemaining -= timeDelta * usageRate;
            if (fuelRemaining <= 0)
            {
                fuelRemaining = 0;
                //Debug.Log("Fuel Tank Empty");
                FindObjectOfType<MiniGameWindow>().MiniGameEnd(false);
                GetComponent<ScoreKeeper>().Complete();
            }
        }

        if (fuelGauge)
            fuelGauge.SetAmount(fuelRemaining);
    }

}
