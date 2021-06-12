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
                Gizmos.DrawSphere(transform.position + (Vector3)connection.position, 0.1f);
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position + (Vector3)connection.position, transform.position + (Vector3)connection.position + (Vector3)connection.outDirection * 0.5f);

                Gizmos.color = new Color(50, 50, 50, 75);
                Gizmos.DrawLine(module.AABBCorners[0] + transform.position, new Vector3(module.AABBCorners[0].x, module.AABBCorners[1].y) + transform.position);
                Gizmos.DrawLine(new Vector3(module.AABBCorners[0].x, module.AABBCorners[1].y) + transform.position, module.AABBCorners[1] + transform.position);
                Gizmos.DrawLine(module.AABBCorners[1] + transform.position, new Vector3(module.AABBCorners[1].x, module.AABBCorners[0].y) + transform.position);
                Gizmos.DrawLine(new Vector3(module.AABBCorners[1].x, module.AABBCorners[0].y) + transform.position, module.AABBCorners[0] + transform.position);
            }
        }

    }
}
