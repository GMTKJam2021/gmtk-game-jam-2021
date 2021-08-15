using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StationModuleDebug : MonoBehaviour
{
    public StationModule module;

    private void OnDrawGizmosSelected()
    {
        if (module == null)
        {
            module = GetComponent<StationModule>();
        }

        if (module != null)
        {
            foreach (var connection in module.connections)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(transform.position + (Vector3)(Vector2)connection.connectedModuleGridLocation, 0.1f);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + (Vector3)(Vector2)connection.connectedModuleGridLocation, transform.position + (Vector3)(Vector2)connection.connectedModuleGridLocation + (Vector3)Helper.DegreeToVector2(connection.outDirectionAngle) * 0.5f);
            }
        }

    }
}
