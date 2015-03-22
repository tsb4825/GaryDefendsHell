using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathingScript {

    public static Transform FindClosestWayPointToSelfAndTarget(Transform unit, Transform wayPointTarget, Transform target)
    {
        GameObject[] wayPoints;
        wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        GameObject closest = null;
        GameObject secondClosest = null;
        var distanceToSelf = Mathf.Infinity;
        var secondDistanceToSelf = Mathf.Infinity;
        var position = UtilityFunctions.UseUnitZPosition(unit, unit.position);
        // Iterate through them and find the two closest ones
        foreach (GameObject wayPoint in wayPoints)
        {
            if (wayPointTarget == null || wayPoint.transform != wayPointTarget)
            {
                var diff = (UtilityFunctions.UseUnitZPosition(unit, wayPoint.transform.position) - UtilityFunctions.UseUnitZPosition(unit, position));
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
        UtilityFunctions.DebugMessage(closest.name + "s distance to target: " + (UtilityFunctions.UseUnitZPosition(unit, closest.transform.position) - UtilityFunctions.UseUnitZPosition(unit, target.position)).sqrMagnitude);
        UtilityFunctions.DebugMessage(secondClosest.name + "s distance to target: " + (UtilityFunctions.UseUnitZPosition(unit, secondClosest.transform.position) - UtilityFunctions.UseUnitZPosition(unit, target.position)).sqrMagnitude);
        UtilityFunctions.DebugMessage("Units distance to target: " + ((UtilityFunctions.UseUnitZPosition(unit, unit.position) - UtilityFunctions.UseUnitZPosition(unit, target.position)).sqrMagnitude));

        // use waypoint closest to target
        if ((UtilityFunctions.UseUnitZPosition(unit, closest.transform.position) - UtilityFunctions.UseUnitZPosition(unit, target.position)).sqrMagnitude <
            (UtilityFunctions.UseUnitZPosition(unit, secondClosest.transform.position) - UtilityFunctions.UseUnitZPosition(unit, target.position)).sqrMagnitude)
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

    public static List<List<Vector2>> BuildAIPaths()
    {
        // get spawn points
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Spawn");

        // get base
        Transform baseBuilding = GameObject.FindGameObjectWithTag("Base").transform;

        var paths = new List<List<Vector2>>();

        // loop and build each one
        foreach (GameObject spawn in spawns)
        {
            List<Vector2> path = new List<Vector2>();

            Transform currentPosition = new GameObject().transform;
            currentPosition.position = spawn.transform.position;

            Transform previousWayPoint = null;

            currentPosition = FindClosestWayPointToSelfAndTarget(currentPosition, previousWayPoint, baseBuilding);

            // add nodes to path
            path.Add(spawn.transform.position);
            path.Add(currentPosition.transform.position);

            int loopBreak = 0;
            // loop till base reached, always take closest, exclude previous node
            while (!baseBuilding.GetComponent<Collider2D>().bounds.Contains(UtilityFunctions.UseUnitZPosition(baseBuilding, currentPosition.transform.position)) && loopBreak < 50)
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
