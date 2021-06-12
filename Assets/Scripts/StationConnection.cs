using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationConnection
{
    public Vector2 position;
    public float outDirectionAngle;
    public StationConnection otherConnection;
}
