using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public static class UtilityFunctions
{
    public static Transform FindClosestWayPointToSelfAndTarget(Transform unit, Transform wayPointTarget, Transform target)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("WayPoint");
        GameObject closest = null;
        GameObject secondClosest = null;
        var distanceToSelf = Mathf.Infinity;
        var secondDistanceToSelf = Mathf.Infinity;
        var position = UseUnitZPosition(unit, unit.position);
        // Iterate through them and find the two closest ones
        foreach (GameObject go in gos)
        {
            if (wayPointTarget == null || go.transform != wayPointTarget)
            {
                var diff = (UseUnitZPosition(unit, go.transform.position) - UseUnitZPosition(unit, position));
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distanceToSelf)
                {
                    if (closest != null)
                    {
                        secondClosest = closest;
                        secondDistanceToSelf = distanceToSelf;
                    }
                    closest = go;
                    distanceToSelf = curDistance;
                }
                else if (curDistance < secondDistanceToSelf)
                {
                    secondClosest = go;
                    secondDistanceToSelf = curDistance;
                }
            }
        }
        UtilityFunctions.DebugMessage(closest.name + "s distance to target: " + (UseUnitZPosition(unit, closest.transform.position) - UseUnitZPosition(unit, target.position)).sqrMagnitude);
        UtilityFunctions.DebugMessage(secondClosest.name + "s distance to target: " + (UseUnitZPosition(unit, secondClosest.transform.position) - UseUnitZPosition(unit, target.position)).sqrMagnitude);
        UtilityFunctions.DebugMessage("Units distance to target: " + ((UseUnitZPosition(unit, unit.position) - UseUnitZPosition(unit, target.position)).sqrMagnitude));

        // use waypoint closest to target
        if ((UseUnitZPosition(unit, closest.transform.position) - UseUnitZPosition(unit, target.position)).sqrMagnitude <
            (UseUnitZPosition(unit, secondClosest.transform.position) - UseUnitZPosition(unit, target.position)).sqrMagnitude)
        {
            UtilityFunctions.DebugMessage(closest.name + " chosen.");
            return closest.transform;
        }
        else
        {
            UtilityFunctions.DebugMessage(secondClosest.name + " chosen.");
            return secondClosest.transform;
        }
    }

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

    public static bool IsCloseEnough(this Vector3 position1, Vector3 position2)
    {
        return ((Mathf.Abs(position1.x - position2.x) <= .04f) && (Mathf.Abs(position1.y - position2.y) <= .04f));
    }

    //public static IEnumerable<Vector2> GetWayPointsRelevantToObject(Vector2 objectPosition, float radius)
    //{
    //    IEnumerable<Transform> wayPoints 
    //}
}
