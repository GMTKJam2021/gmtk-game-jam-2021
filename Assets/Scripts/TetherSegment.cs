using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSegment : MonoBehaviour
{
    public TetherSegment parent;
    public GameObject tetherPrefab;

    private void Awake()
    {
        Debug.Assert(tetherPrefab != null);
        Debug.Assert(GetComponent<Rigidbody2D>() != null);
        Debug.Assert(GetComponent<Collider2D>() != null);
        Debug.Assert(GetComponent<HingeJoint2D>() != null);
    }

    public TetherSegment AddSegmentBefore()
    {
        GameObject n = Instantiate(tetherPrefab);
        n.transform.parent = transform.parent;

        parent.GetComponent<HingeJoint2D>().connectedBody = n.GetComponent<Rigidbody2D>();
        n.GetComponent<HingeJoint2D>().connectedBody = GetComponent<Rigidbody2D>();

        TetherSegment newParent = n.GetComponent<TetherSegment>();
        newParent.parent = parent;
        parent = newParent;

        return newParent;
    }

    public void RemoveSegmentBefore()
    {

    }
}
