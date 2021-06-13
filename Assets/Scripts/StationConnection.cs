using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationConnection
{
    public float outDirectionAngle;
    public Vector2Int connectedModuleGridLocation;
    public bool isBraceConnection = false;
}
