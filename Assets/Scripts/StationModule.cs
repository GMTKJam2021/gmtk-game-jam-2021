using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationModule : MonoBehaviour
{
    public float rotationSpeed;
    public List<StationConnection> connections = new List<StationConnection>();
    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        Debug.Assert(rb != null);
        Debug.Assert(rotationSpeed != 0);
        Debug.Assert(connections.Count > 0);
    }

    private void Update()
    {

    }

    private void AlignToConnection(StationConnection other)
    {
        transform.eulerAngles = new Vector3(0f, 0f, other.outDirectionAngle);
    }

    private IEnumerator AlignToAngle(float angle)
    {
        // Each frame, rotate by rotationSpeed * deltaTime towards the goal angle
        while (rb.rotation >= angle + rotationSpeed || rb.rotation <= angle - rotationSpeed)
        {
            rb.rotation += angle * Time.deltaTime;
            yield return null;
        }

    }

}