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
    public bool isCenterCore = false;
    public CoreSection coreSection = CoreSection.NotCore;
    public bool isBrace = false;

    private void Awake()
    {
        Debug.Assert(rb != null);
        Debug.Assert(rotationSpeed != 0);
        Debug.Assert(connections.Count > 0);
    }

    public bool CheckIfCanConnectTo(int x, int y, int offsetX, int offsetY)
    {
        foreach (var connection in connections)
        {
            // Debug.Log(connection + " of " + connections.Count);
            // If it has a connector that can connect to the requested spot, return true
            // Debug.Log((connection.connectedModuleGridLocation.x + offsetX) + " == " + x + " | " + (connection.connectedModuleGridLocation.y + offsetY) + " == " + y);
            if (connection.connectedModuleGridLocation.x + offsetX == x && connection.connectedModuleGridLocation.y + offsetY == y)
            {
                // Debug.Log("True");
                return true;
            }
        }
        return false;
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

    public enum CoreSection
    {
        NotCore,
        Center,
        CenterLeft,
        CenterRight,
        TopLeft,
        TopCenter,
        TopRight,
        BottomLeft,
        BottomCenter,
        BottomRight,
    }
}