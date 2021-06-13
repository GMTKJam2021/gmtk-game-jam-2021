using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    public float maxFuel = 100f;
    public float fuelRemaining = 100f;
    public MeterBar fuelGauge;

    void Start(){
        if(fuelGauge){
            fuelGauge.SetMaxAmount(maxFuel);
            fuelGauge.SetAmount(fuelRemaining);
        }
    }

    /**
      * Adds amount to fuelRemaining up to the maxfuel limit, and returns the amount of fuel added.
    **/
    public float ReplenishFuel(float amount){
        if(fuelRemaining+amount>=maxFuel){
            float temp = maxFuel - fuelRemaining;
            fuelRemaining = maxFuel;

            if(fuelGauge){
                fuelGauge.SetAmount(fuelRemaining);
            }
            
            return temp;
        }
        else
        {
            fuelRemaining += amount;

            if(fuelGauge)
                fuelGauge.SetAmount(fuelRemaining);
            
            return amount;
        }
    }

    /**
      * Subtracts amount from fuelRemaining down to the minimum of zero, and returns the fuel removed;
    **/
    public float DepleteFuel(float amount){
        if(fuelRemaining>=amount)
        {
            fuelRemaining-=amount;

            if(fuelGauge)
                fuelGauge.SetAmount(fuelRemaining);
            
            return amount;
        }
        else
        {
            float temp = fuelRemaining;
            fuelRemaining = 0f;

            if(fuelGauge)
                fuelGauge.SetAmount(fuelRemaining);
            
            return temp;
        }
    }
    
}
