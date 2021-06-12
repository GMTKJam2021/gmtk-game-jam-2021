using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DebrisField : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnMetrics{
        public string typeName;
        public SpaceDebris prefab;
        public float directionLowerBound;
        public float directionUpperBound;
        public float velocityLowerBound;
        public float velocityUpperBound;
        public float probability;
        public int maxInstances;
    }
    
    public SpawnMetrics[] spawnOptions; 

    CircleCollider2D bounds;
    public Collider2D Bounds { get{ return bounds; }}

    void Awake(){
        bounds = GetComponent<CircleCollider2D>();
        bounds.isTrigger = true;
    }

    void Start(){
        foreach(SpawnMetrics option in spawnOptions){
            GameObject obj = new GameObject(option.typeName);
            obj.transform.SetParent(this.transform);
        }
        // Vector2 start = Random.insideUnitCircle.normalized * bounds.radius;
        // Vector3 direction = Random.insideUnitCircle.normalized * 10f;

        // SpaceDebris debris = Instantiate(debrisPrefab.gameObject).GetComponent<SpaceDebris>();
        // debris.parent = this;
        // debris.transform.position = start;
        // debris.rb.AddForce(direction*10f);
        // debris.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<spawnOptions.Length;i++){
            SpawnMetrics metrics = spawnOptions[i];
            Transform child = transform.GetChild(i);

            if(child.childCount<metrics.maxInstances)
            {
                TrySpawn(metrics, child);
            }

            
        }
    }

    public void TrySpawn(SpawnMetrics metrics, Transform parent){
        if(Random.value > metrics.probability) return;

        float direction = Random.Range(metrics.directionLowerBound, metrics.directionUpperBound)%360f;
        float speed = Random.Range(metrics.velocityLowerBound, metrics.velocityUpperBound);
        float side2side = Random.Range(-bounds.radius,bounds.radius);

        //x-axis will be spread, y-axis will be distance from origin
        Vector2 startLocation = RotateVector2( new Vector2(side2side,bounds.radius), direction+180f);
        Vector2 velocity =  RotateVector2( Vector2.up, direction)*speed;

        SpaceDebris debris = Instantiate(metrics.prefab).GetComponent<SpaceDebris>();
        debris.parent = this; 
        debris.transform.position = startLocation;
        debris.rb.velocity = velocity;
        debris.transform.SetParent(parent);
    }

    public static Vector2 RotateVector2(Vector2 v, float degrees){
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public void DespawnDebris(SpaceDebris debris){
        Destroy(debris.gameObject);
    }
}
