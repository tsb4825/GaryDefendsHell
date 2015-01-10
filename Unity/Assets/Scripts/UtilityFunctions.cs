using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

public static class UtilityFunctions
{
    public static Transform FindClosestWayPointToSelfAndTarget(Transform unit, Transform wayPointTarget, Transform target)
    {
        GameObject[] wayPoints;
        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        GameObject closest = null;
        GameObject secondClosest = null;
        var distanceToSelf = Mathf.Infinity;
        var secondDistanceToSelf = Mathf.Infinity;
        var position = UseUnitZPosition(unit, unit.position);
        // Iterate through them and find the two closest ones
        foreach (GameObject wayPoint in wayPoints)
        {
            if (wayPointTarget == null || wayPoint.transform != wayPointTarget)
            {
                var diff = (UseUnitZPosition(unit, wayPoint.transform.position) - UseUnitZPosition(unit, position));
                var curDistance = diff.sqrMagnitude;
                if (curDistance < distanceToSelf)
                {
                    if (closest != null)
                    {
                        secondClosest = closest;
                        secondDistanceToSelf = distanceToSelf;
                    }
                    closest = wayPoint;
                    distanceToSelf = curDistance;
                }
                else if (curDistance < secondDistanceToSelf)
                {
                    secondClosest = wayPoint;
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

    public static bool CameraIsCloseEnough(this Vector3 position1, Vector3 position2)
    {
        return ((Mathf.Abs(position1.x - position2.x) <= .04f) && (Mathf.Abs(position1.y - position2.y) <= .04f));
    }

    public static List<List<Vector3>> BuildAIPaths()
    {
        // get spawn points
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");

        // get base
        Transform baseBuilding = GameObject.FindGameObjectWithTag("Base").transform;

        var paths = new List<List<Vector3>>();

        // loop and build each one
        foreach(GameObject spawn in spawns)
        {
            List<Vector3> path = new List<Vector3>();

            Transform currentPosition = new GameObject().transform;
            currentPosition.position = spawn.transform.position;

            Transform previousWayPoint = null;

            currentPosition = FindClosestWayPointToSelfAndTarget(currentPosition, previousWayPoint, baseBuilding);
            Debug.Break();
            // add nodes to path
            path.Add(spawn.transform.position);
            path.Add(currentPosition.transform.position);

            int loopBreak = 0;
            // loop till base reached, always take closest, exclude previous node
            while (!baseBuilding.collider2D.bounds.Contains(UseUnitZPosition(baseBuilding, currentPosition.transform.position)) && loopBreak < 50)
            {
                loopBreak++;
                previousWayPoint = currentPosition;
                currentPosition = FindClosestWayPointToSelfAndTarget(currentPosition, previousWayPoint, baseBuilding);

                //add vector to path
                path.Add(currentPosition.transform.position);
            }

            paths.Add(path);
        }

        return paths;
    }
}
