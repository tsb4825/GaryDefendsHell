using UnityEngine;
using System.Collections;

public class WayPointScript : MonoBehaviour
{
		public bool DebugMode;
	
		void OnTriggerEnter2D (Collider2D collider)
		{
				if (collider is BoxCollider2D && collider.GetComponent<CreepScript> () != null && collider.GetComponent<CreepScript> ().WayPointTarget == transform) {
						UtilityFunctions.DebugMessage ("Finding waypoint through collider - " + collider.name);
						Transform wayPointTarget = collider.GetComponent<CreepScript> ().WayPointTarget;
						Transform target = collider.GetComponent<CreepScript> ().Target;
						wayPointTarget = UtilityFunctions.FindClosestWayPointToSelfAndTarget (transform, wayPointTarget, target);
						collider.GetComponent<CreepScript> ().WayPointTarget = wayPointTarget ?? target;
				}
		}
}
