using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;
    public FuelTank fuelTank;

    public float jetpackForwardDeltaV = 100f; //m/s^2
    public float jetpackFowaredDepletionRate = 1f; //kg/s

    
    public bool requiresFuelToRotate = false;
    public float jetpackRotateDeltaV = 100f;
    public float jetpackRotateDepletion= 1f; //kg/s
    public bool isSwinging = false;
    public Vector2 tetherHook;
    public float swingForce = 4f;

    void Awake(){
        rb=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow)){
            float forceApplied = fuelTank.DepleteFuel(jetpackFowaredDepletionRate * Time.deltaTime) * jetpackForwardDeltaV ;
            rb.AddForce( transform.up * forceApplied);
        }

        if(Input.GetKey(KeyCode.LeftArrow)){
            float torqueApplied = requiresFuelToRotate? 
                fuelTank.DepleteFuel(jetpackRotateDepletion * Time.deltaTime) * jetpackRotateDeltaV : 
                jetpackRotateDeltaV * Time.deltaTime;
            rb.AddTorque(torqueApplied);
        }
        if(Input.GetKey(KeyCode.RightArrow)){
            float torqueApplied = requiresFuelToRotate? 
                fuelTank.DepleteFuel(jetpackRotateDepletion * Time.deltaTime) * jetpackRotateDeltaV : 
                jetpackRotateDeltaV * Time.deltaTime;
            rb.AddTorque( -torqueApplied);
        }


    }
}
