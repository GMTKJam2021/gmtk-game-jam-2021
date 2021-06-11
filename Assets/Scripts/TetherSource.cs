using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherSource : MonoBehaviour
{
    public TetherSegment startingSegment;
    public DistanceJoint2D distanceJoint;
    public float distanceToExtend = 0.5f;

    private void Awake()
    {
        Debug.Assert(distanceJoint != null);
        Debug.Assert(startingSegment != null);

    }
    private void FixedUpdate()
    {
        // if tether is stretched
        HingeJoint2D j = startingSegment.GetComponent<HingeJoint2D>();
        if (distanceJoint.distance >= distanceToExtend)
        {
            // add piece of tether
            startingSegment = startingSegment.AddSegmentBefore();
        }

        // on button hold
        // slowly remove pieces of tether
    }
}
