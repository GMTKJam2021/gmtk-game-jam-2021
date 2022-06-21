using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class DebrisField : MonoBehaviour
{
    [System.Serializable]
    public class SpawnMetrics{
        public string typeName;
        public bool enabled;
        public SpaceDebris prefab;
        public float directionLowerBound;
        public float directionUpperBound;
        public float velocityLowerBound;
        public float velocityUpperBound;
        public float probability;
        public int maxInstances;
    }
    
    public float spawnDistance = 20f;
    public SpawnMetrics[] spawnOptions;
    private float spawnRate = 2;
    private int brokenMinimum = 2;

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
            obj.transform.localPosition = Vector3.forward;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for(int i=0; i<spawnOptions.Length;i++){
            SpawnMetrics metrics = spawnOptions[i];
            if(!metrics.enabled) continue;
            Transform child = transform.GetChild(i);

            if(child.childCount<metrics.maxInstances)
            {
                TrySpawn(metrics, child);
            }
        }
        //The spawn rate will increase by 1 every 20 seconds
        spawnRate += .005f * Time.deltaTime;

    }

    public void TrySpawn(SpawnMetrics metrics, Transform parent){
        if(ModuleState.brokenModules > brokenMinimum)
        {
            if (Random.value > metrics.probability * Time.deltaTime * spawnRate) return;
        }
        else if(Random.value > metrics.probability * Time.deltaTime * spawnRate * 2) return;

        float direction = Random.Range(metrics.directionLowerBound, metrics.directionUpperBound)%360f;
        float speed = Random.Range(metrics.velocityLowerBound, metrics.velocityUpperBound);
        float side2side = Random.Range(-spawnDistance,spawnDistance);

        //x-axis will be spread, y-axis will be distance from origin
        Vector2 startLocation = RotateVector2( new Vector2(side2side,spawnDistance), direction+180f);
        Vector2 velocity =  RotateVector2( Vector2.up, direction)*speed;

        SpaceDebris debris = Instantiate(metrics.prefab).GetComponent<SpaceDebris>();
        debris.parent = this;
        debris.transform.SetParent(parent);
        debris.transform.localPosition = startLocation;
        debris.rb.velocity = velocity;
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
