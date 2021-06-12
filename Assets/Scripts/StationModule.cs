using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationModule : MonoBehaviour
{
    public float rotationSpeed;
    public List<StationConnection> connections = new List<StationConnection>();
    // For the sake of simplicity, the first AABBCorner must be above and to the left of the second
    public Vector3[] AABBCorners = new Vector3[2];
    [SerializeField]
    private Rigidbody2D rb;

    private void Awake()
    {
        Debug.Assert(rb != null);
        Debug.Assert(rotationSpeed != 0);
        Debug.Assert(connections.Count > 0);
        Debug.Assert(AABBCorners.Length == 2);
        Debug.Assert(AABBCorners[0] != AABBCorners[1]);
        Debug.Assert(AABBCorners[0].x < AABBCorners[1].x);
        Debug.Assert(AABBCorners[0].y > AABBCorners[1].y);
    }

    private void Update()
    {

    }

    private void FindValidConnections()
    {
        Transform stationTransform = transform.parent;
        SpaceStation station = stationTransform.GetComponent<SpaceStation>();
        List<StationModule> validModules = station.GetModules(true);


    }

    private bool CheckIfPlacementIsValid(StationConnection thisConnection, StationConnection otherConnection)
    {
        if (otherConnection.otherConnection != null)
        {
            return false; // there is already another connected module here
        }

        // Calculate AABB width and height
        float width = Mathf.Abs(AABBCorners[0].x - AABBCorners[1].x);
        float height = Mathf.Abs(AABBCorners[0].y - AABBCorners[1].y);

        // Calculate center point on one side
        float halfHeight = height / 2f;
        Vector3 centerSidePoint = new Vector3(0f, AABBCorners[0].y - halfHeight);

        // Rederive AABBCorners based on centerSidePoint
        Vector3[] adjustedCorners = new Vector3[2];
        adjustedCorners[0] = new Vector3(centerSidePoint.x, centerSidePoint.y + halfHeight);
        adjustedCorners[1] = new Vector3(centerSidePoint.x + width, centerSidePoint.y + halfHeight);

        // Check if collider exists in rectangle
        Collider2D col = Physics2D.OverlapArea(adjustedCorners[0] + transform.position, adjustedCorners[1] + transform.position);
        if (col == null) // If there are no colliders in the area
        {
            return true;
        }
        return false;
    }

    private void AlignToConnection(StationConnection other)
    {
        StartCoroutine(AlignToAngle(Vector2.Angle(Vector2.zero, other.outDirection)));
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