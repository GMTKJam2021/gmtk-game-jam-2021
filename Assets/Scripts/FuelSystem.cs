using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelSystem : MonoBehaviour
{
    public float fuelRemaining;

    public float DepleteFuel(float amount){
        if(fuelRemaining>=amount)
        {
            fuelRemaining-=amount;
            return amount;
        }
        else
        {
            float temp = fuelRemaining;
            fuelRemaining = 0f;
            return temp;
        }
    }
    
}
