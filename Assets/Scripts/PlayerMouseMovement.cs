using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private FuelTank fuelTank;
    public static bool inGame;
    private Animator anim;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float depletionRate = 10f;

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
            if (Input.GetMouseButton(0))
            {
                fuelTank.DepleteFuel( depletionRate * Time.fixedDeltaTime);
                rb.AddForce(transform.up * speed);
                anim.SetBool("Boosting", true);
            }
            else
                anim.SetBool("Boosting", false);

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }

    }



}
