using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private FuelTank fuelTank;
    public static bool inGame;
    private Animator anim;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    public bool keyBoardControls;

    private Vector3 mousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuelTank = GetComponent<FuelTank>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inGame)
        {
            if(keyBoardControls)
            {
                if (Input.GetAxis("Vertical") > .1)
                {
                    if (fuelTank.fuelRemaining > 0)
                    {
                        fuelTank.DepleteFuel(Time.deltaTime);
                        rb.AddForce(transform.up * speed);
                        anim.SetBool("Boosting", true);
                    }
                }
                else
                    anim.SetBool("Boosting", false);
                if (Input.GetAxis("Horizontal") < -.1)
                    transform.Rotate(Vector3.forward, rotateSpeed); 
                if (Input.GetAxis("Horizontal") > .1)
                    transform.Rotate(Vector3.forward, -1 * rotateSpeed);
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (fuelTank.fuelRemaining > 0)
                    {
                        fuelTank.DepleteFuel(Time.deltaTime);
                        rb.AddForce(transform.up * speed);
                        anim.SetBool("Boosting", true);
                    }
                }
                else
                    anim.SetBool("Boosting", false);

                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

    }



}
