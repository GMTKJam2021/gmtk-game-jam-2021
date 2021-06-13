using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ModuleState currentModule;
    private OxygenTank oxygenTank;
    [SerializeField] private bool tethered = false;
    public static bool noModule;

    private void Start()
    {
        oxygenTank = GetComponent<OxygenTank>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            if (currentModule != null && !PlayerMouseMovement.inGame)
            {
                currentModule.AttemptFix();
            }
            else
                Debug.Log("No Module Available");
        if (tethered)
            oxygenTank.ReplenishOxygen(Time.deltaTime);
        else
            oxygenTank.DepleteOxygen(Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Module"))
        {
            currentModule = collision.GetComponent<ModuleState>();
            Debug.Log(currentModule.name + "in range");
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Module"))
        {
            Debug.Log(currentModule.name + "out of range");
            currentModule = null;
        }
    }
}
