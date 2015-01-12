using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public static class UtilityFunctions
{
    public static Vector3 UseUnitZPosition(Transform unit, Vector3 position)
    {
        position.z = unit.position.z;
        return position;
    }

    public static void DebugMessage(string message)
    {
        if (false)
        {
            Debug.Log(message);
        }
    }

    public static bool CameraIsCloseEnough(this Vector3 position1, Vector3 position2)
    {
        return ((Mathf.Abs(position1.x - position2.x) <= .04f) && (Mathf.Abs(position1.y - position2.y) <= .04f));
    }
}
