using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class SpaceDebris : MonoBehaviour
{
    public DebrisField parent;
    public int damage;
    public float kickMagnitude = 4f;

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

    void OnTriggerEnter2D(Collider2D collider){
        ModuleState ms = collider.GetComponent<ModuleState>();
        if(ms){
            ms.TakeDamage(damage);
            //Add a kick
            Vector2 velocity = rb.velocity;
            float direction = Vector2.SignedAngle(Vector2.up,velocity);
            float kickDirection = Random.Range(direction+90f,direction+180f);
            Vector2 kickVelocity = DebrisField.RotateVector2(Vector2.up,kickDirection) *  kickMagnitude;
            rb.AddForce(kickVelocity);
        }
        
    }
}
