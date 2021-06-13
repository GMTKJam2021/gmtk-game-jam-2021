using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    
    private List<ModuleState> nearbyModules = new List<ModuleState>();
    private OxygenTank oxygenTank;
    [SerializeField] private bool tethered = false;
    public static bool noModule = true;

    private void Start()
    {
        oxygenTank = GetComponent<OxygenTank>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1))
            if ( nearbyModules.Any() && !PlayerMouseMovement.inGame)
            {
                ClosestModule().AttemptFix();
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
            nearbyModules.Add(collision.GetComponent<ModuleState>());
            Debug.Log(collision.name + "in range");
            noModule = false;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Module"))
        {
            Debug.Log(collision.name + "out of range");
            nearbyModules.Remove( collision.GetComponent<ModuleState>());
            if (nearbyModules.Count == 0)
                noModule = true;
        }
    }

    public ModuleState ClosestModule(){
        float dist = Mathf.Infinity;
        int selection = -1;
        for(int i=0; i<nearbyModules.Count; i++){
            float mydist = Vector2.Distance(nearbyModules[i].transform.position, this.transform.position);
            if(mydist<dist){
                dist=mydist;
                selection=i;
            }
        }
        return nearbyModules[selection];
    }

    // public int CompareByDistance(ModuleState a, ModuleState b){
    //     if(a==null){
    //         if(b==null) return 0;
    //         else return -1;
    //     }else if(b==null) return 1;
    
    //     float dista = Vector2.Distance(a.transform.position,transform.position);
    //     float distb = Vector2.Distance(b.transform.position,transform.position);

    //     if(dista==distb)return 0;
        
    // }
}
