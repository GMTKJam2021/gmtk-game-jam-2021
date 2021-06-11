using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSource : MonoBehaviour
{
    public GameObject tetherPrefab;
    public DistanceJoint2D joint;
    public float maxStretchBeforeAddition = 0.5f;

    private void Awake()
    {
        Debug.Assert(tetherPrefab != null);
        Debug.Assert(joint != null);
    }

    public void AddNewTether()
    {
        GameObject newTether = Instantiate(tetherPrefab, transform.position + Vector3.up * 0.1f, Quaternion.identity);
        newTether.transform.parent = transform.parent;
        HingeJoint2D newJoint = newTether.GetComponent<HingeJoint2D>();
        Rigidbody2D oldTether = joint.connectedBody;
        joint.connectedBody = newTether.GetComponent<Rigidbody2D>();
        newJoint.connectedBody = oldTether;
    }

    public bool CheckForNewTether()
    {
        return joint.distance > maxStretchBeforeAddition;
    }

    private void FixedUpdate()
    {
        if (CheckForNewTether())
        {
            AddNewTether();
        }
    }
}
