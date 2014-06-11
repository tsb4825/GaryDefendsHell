using UnityEngine;
using System.Collections;

public class ArcingProjectile : ProjectileScript
{
		public Transform Target;
		public Vector3 GuessedTargetLocation;
		public Vector3 MidPoint;
		public float MidPointDistance;
		public bool IsMoving;
		public float MaxAdditionalScale = 10f;
		public float RemoveProjectileTime;
		public float ProjectileAliveTime = .1f;
		public float ActualTimeToTarget;

		void Start ()
		{
		// move this logic to update on first time it is called to use delta time for unit location prediction
				Vector3 targetLocation = Target.position;
				Vector3 distanceToTarget = targetLocation - transform.position;

				// Guess location when reaching target
				float anticipatedTimeToTarget = distanceToTarget.sqrMagnitude / ProjectileSpeed;
				Transform targetsTarget = Target.GetComponent<CreepScript> ().WayPointTarget;
				bool IsFighting = Target.GetComponent<CreepScript> ().IsFighting;
				bool IsMovingTowardUnit = Target.GetComponent<CreepScript> ().IsMovingTowardFighter;
				float TargetSpeed = Target.GetComponent<CreepScript> ().UnitSpeed;
				Transform targetsFightingTarget = Target.GetComponent<CreepScript> ().FightingTarget;
		
				if (!IsFighting) {
						targetLocation = Vector2.MoveTowards (targetLocation, targetsTarget.position, TargetSpeed * anticipatedTimeToTarget);
						// adjust by time off due to not knowing location of target after time
						Vector3 newDistanceToTarget = targetLocation - transform.position;
						float newAnticipatedTimeToTarget = newDistanceToTarget.sqrMagnitude / ProjectileSpeed;
						targetLocation = Vector2.MoveTowards (targetLocation, targetsTarget.position, TargetSpeed * newAnticipatedTimeToTarget);
						UtilityFunctions.DebugMessage ("Anticipated Time to target: " + newAnticipatedTimeToTarget);
				} else if (IsFighting && IsMovingTowardUnit) {
						targetLocation = Vector2.MoveTowards (targetLocation, targetsFightingTarget.position, TargetSpeed * anticipatedTimeToTarget);
						// adjust by time off due to not knowing location of target after time
						Vector3 newDistanceToTarget = targetLocation - transform.position;
						float newAnticipatedTimeToTarget = newDistanceToTarget.sqrMagnitude / ProjectileSpeed;
						targetLocation = Vector2.MoveTowards (targetLocation, targetsFightingTarget.position, TargetSpeed * newAnticipatedTimeToTarget);
						UtilityFunctions.DebugMessage ("Anticipated Time to target: " + newAnticipatedTimeToTarget);		
				}
				GuessedTargetLocation = targetLocation;
				
				UtilityFunctions.DebugMessage ("Guessed Target Location: " + targetLocation);
				UtilityFunctions.DebugMessage ("Projectile Location: " + transform.position);

				// adjust direction on new location
				distanceToTarget = targetLocation + transform.position;

				MidPoint = distanceToTarget * .5f;
				MidPointDistance = Vector3.Distance (targetLocation, transform.position) / 2f;
				UtilityFunctions.DebugMessage ("Target: " + distanceToTarget);
				UtilityFunctions.DebugMessage ("Distance To Target: " + Vector3.Distance (targetLocation, transform.position));
				UtilityFunctions.DebugMessage ("Mid Point: " + MidPoint);
				UtilityFunctions.DebugMessage ("Mid Point Distance: " + MidPointDistance + ", " + Vector3.Distance (targetLocation, transform.position) / 2f);

				UtilityFunctions.DebugMessage ("Arching Projectile velocity: " + (targetLocation - transform.position).normalized * ProjectileSpeed);
				transform.GetComponent<Rigidbody2D> ().velocity = (targetLocation - transform.position).normalized * ProjectileSpeed;
				UtilityFunctions.DebugMessage ("Anticipated Time to target: " + distanceToTarget.magnitude / transform.GetComponent<Rigidbody2D> ().velocity.magnitude);
				IsMoving = true;
		}

		void Update ()
		{
				ActualTimeToTarget += Time.deltaTime;
				// Scale based on position of arc, and adjust z
				// going up
				Vector3 unitPositionZeroZ = new Vector3 (transform.position.x, transform.position.y, 0);
				if (IsMoving && (GuessedTargetLocation - unitPositionZeroZ).sqrMagnitude >= MidPointDistance) {
						UtilityFunctions.DebugMessage ("Going up");
						float distanceToMidPoint = Vector3.Distance (MidPoint, unitPositionZeroZ);
						UtilityFunctions.DebugMessage ("Arching Projectile distanceToMidPoint: " + distanceToMidPoint + ", MidPointDistance: " + MidPointDistance);
						UtilityFunctions.DebugMessage ("Arching Projectile scale: " + (1f + ((MidPointDistance - distanceToMidPoint) / MidPointDistance * MaxAdditionalScale)));
						float newScale = 1f + ((MidPointDistance - distanceToMidPoint) / MidPointDistance * MaxAdditionalScale);
						transform.localScale = new Vector3 (newScale, newScale, 0);
						transform.position = new Vector3 (transform.position.x, transform.position.y, newScale);
				}
		// going down
		else if (IsMoving) {
						UtilityFunctions.DebugMessage ("Going down");
						float distanceAwayFromMidPoint = Vector3.Distance (unitPositionZeroZ, MidPoint);
						UtilityFunctions.DebugMessage ("Arching Projectile distanceAwayFromMidPoint: " + distanceAwayFromMidPoint + ", MidPointDistance: " + MidPointDistance);
						UtilityFunctions.DebugMessage ("Arching Projectile scale: " + (1f + ((MidPointDistance - distanceAwayFromMidPoint) / MidPointDistance * MaxAdditionalScale)));
						float newScale = 1f + ((MidPointDistance - distanceAwayFromMidPoint) / MidPointDistance * MaxAdditionalScale);
						transform.localScale = new Vector3 (newScale, newScale, 0);
						transform.position = new Vector3 (transform.position.x, transform.position.y, newScale);
						if (newScale <= 1) {
								UtilityFunctions.DebugMessage ("Actual Target Location: " + Target.position);
								UtilityFunctions.DebugMessage ("Actual Time to Target: " + ActualTimeToTarget);
								IsMoving = false;
								transform.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
								RemoveProjectileTime = Time.time + ProjectileAliveTime;
								transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
						}
				} else {
						if (Time.time >= RemoveProjectileTime) {
								Destroy (this.gameObject);
						}
				}

				// if just hit z = 0, see if hit, otherwise remove collider, stop
		}
}
