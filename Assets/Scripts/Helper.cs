using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }

    public static float Vector2ToRadian(Vector2 vector2)
    {
        return Mathf.Atan2(vector2.y, vector2.x);
    }

    public static float Vector2ToDegree(Vector2 vector2)
    {
        return Vector2ToRadian(vector2) * Mathf.Rad2Deg;
    }

    public static Vector2 RotateVector2ByRadian(Vector2 vector2, float radian)
    {
        float vec = Vector2ToRadian(vector2);
        float sum = vec + radian;
        return RadianToVector2(sum);
    }

    public static Vector2 RotateVector2ByDegree(Vector2 vector2, float degree)
    {
        return RotateVector2ByRadian(vector2, degree * Mathf.Deg2Rad);
    }
}
