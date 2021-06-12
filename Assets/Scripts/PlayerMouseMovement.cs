using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private FuelTank fuelTank;
    public static bool inGame;

    [SerializeField] private float speed = 10f;

    private Vector3 mousePosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuelTank = GetComponent<FuelTank>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!inGame)
        {
            if (Input.GetMouseButton(0))
            {
                fuelTank.DepleteFuel(Time.fixedDeltaTime);
                rb.AddForce(transform.up * speed);
            }

            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

        }
        else
            rb.velocity = Vector2.zero;
    }
}
