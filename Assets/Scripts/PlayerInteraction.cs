using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ModuleState currentModule;
    
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            if (currentModule != null)
                currentModule.Fix();
            else
                Debug.Log("No Module Available");
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
