using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SpaceDebris : MonoBehaviour
{
    public DebrisField parent;

    public Rigidbody2D rb {
        get {
            return GetComponent<Rigidbody2D>();
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(parent==null)
            return;
        
        if(collider == parent.Bounds){
            parent.DespawnDebris(this);
        }
    }
}
