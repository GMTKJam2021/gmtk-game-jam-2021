using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    public Rigidbody2D rb;

    public float jetpackForce = 100f;
    public float jetpackTorque = 100f;
    public bool isSwinging = false;
    public Vector2 tetherHook;
    public float swingForce = 4f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.up * jetpackForce * Time.deltaTime);
            //fuelsystem.DepleteFuel
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddTorque(jetpackTorque * Time.deltaTime);
            //fuelSystem
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddTorque(-jetpackTorque * Time.deltaTime);
        }


    }
}
