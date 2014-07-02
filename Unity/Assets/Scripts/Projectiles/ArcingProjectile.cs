﻿using UnityEngine;
using System.Collections;

public class ArcingProjectile : ProjectileScript
{
		public Transform Target;
		public Vector3 GuessedTargetLocation;
		public Vector3 MidPoint;
		public float MidPointDistance;
		public bool IsMoving;
		public float MaxAdditionalScale;
		public float RemoveProjectileTime;
		public float ProjectileAliveTime;
		public int ActualFramesToTarget;
		public bool NeedToCalculateProjectilePath = true;

		void Update ()
		{
				if (NeedToCalculateProjectilePath) {
						CalculateProjectilePath ();
				}
				ActualFramesToTarget += 1;
				// Scale based on position of arc, and adjust z
				// going up
				Vector3 unitPositionZeroZ = new Vector3 (transform.position.x, transform.position.y, 0);
				if (IsMoving && (GuessedTargetLocation - unitPositionZeroZ).sqrMagnitude >= MidPointDistance) {
						//UtilityFunctions.DebugMessage ("Going up");
						float distanceToMidPoint = Vector3.Distance (MidPoint, unitPositionZeroZ);
						//UtilityFunctions.DebugMessage ("Arching Projectile distanceToMidPoint: " + distanceToMidPoint + ", MidPointDistance: " + MidPointDistance);
						//UtilityFunctions.DebugMessage ("Arching Projectile scale: " + (1f + ((MidPointDistance - distanceToMidPoint) / MidPointDistance * MaxAdditionalScale)));
						float newScale = 1f + ((MidPointDistance - distanceToMidPoint) / MidPointDistance * MaxAdditionalScale);
						transform.localScale = new Vector3 (newScale, newScale, 0);
						transform.position = new Vector3 (transform.position.x, transform.position.y, newScale);
				}
		// going down
		else if (IsMoving) {
						//UtilityFunctions.DebugMessage ("Going down");
						float distanceAwayFromMidPoint = Vector3.Distance (unitPositionZeroZ, MidPoint);
						//UtilityFunctions.DebugMessage ("Arching Projectile distanceAwayFromMidPoint: " + distanceAwayFromMidPoint + ", MidPointDistance: " + MidPointDistance);
						//UtilityFunctions.DebugMessage ("Arching Projectile scale: " + (1f + ((MidPointDistance - distanceAwayFromMidPoint) / MidPointDistance * MaxAdditionalScale)));
						float newScale = 1f + ((MidPointDistance - distanceAwayFromMidPoint) / MidPointDistance * MaxAdditionalScale);
						transform.localScale = new Vector3 (newScale, newScale, 0);
						transform.position = new Vector3 (transform.position.x, transform.position.y, newScale);
						if (newScale <= 1) {
								UtilityFunctions.DebugMessage ("Actual Target Location: " + Target.position);
								UtilityFunctions.DebugMessage ("Actual Frames to Target: " + ActualFramesToTarget);
								IsMoving = false;
								transform.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
								RemoveProjectileTime = Time.time + ProjectileAliveTime;
								transform.position = new Vector3 (transform.position.x, transform.position.y, 0);
								UtilityFunctions.DebugMessage ("Actual projectile hit: " + transform.position);
								// do raycast and remove collider
						}
				} else {
						if (Time.time >= RemoveProjectileTime) {
								Destroy (this.gameObject);
						}
				}
		}

		private void CalculateProjectilePath ()
		{
				Vector3 targetLocation = Target.position;
				Vector3 distanceToTarget = targetLocation - transform.position;
		
				// Guess location when reaching target
				float anticipatedFramesToTarget = distanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime);
				Transform targetsTarget = Target.GetComponent<CreepScript> ().WayPointTarget;
				bool IsFighting = Target.GetComponent<CreepScript> ().IsFighting;
				bool IsMovingTowardUnit = Target.GetComponent<CreepScript> ().IsMovingTowardFighter;
				float TargetSpeed = Target.GetComponent<CreepScript> ().UnitSpeed;
				Transform targetsFightingTarget = Target.GetComponent<CreepScript> ().FightingTarget;
		
				if (!IsFighting) {
						UtilityFunctions.DebugMessage ("Direction to target: " + (targetLocation - UtilityFunctions.UseUnitZPosition (Target, targetsTarget.position)).normalized);
						targetLocation = (UtilityFunctions.UseUnitZPosition (Target, targetsTarget.position) - targetLocation).normalized * anticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
						// adjust by time off due to not knowing location of target after time
						Vector3 newDistanceToTarget = targetLocation - transform.position;
						float newAnticipatedFramesToTarget = newDistanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime * 2);
						targetLocation = (UtilityFunctions.UseUnitZPosition (Target, targetsTarget.position) - targetLocation).normalized * newAnticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
						UtilityFunctions.DebugMessage ("Anticipated Frames to target: " + newAnticipatedFramesToTarget);
				} else if (IsFighting && IsMovingTowardUnit) {
						targetLocation = (UtilityFunctions.UseUnitZPosition (Target, targetsFightingTarget.position) - targetLocation).normalized * anticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
						// adjust by time off due to not knowing location of target after time
						Vector3 newDistanceToTarget = targetLocation - transform.position;
						float newAnticipatedFramesToTarget = newDistanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime * 2);
						targetLocation = (UtilityFunctions.UseUnitZPosition (Target, targetsFightingTarget.position) - targetLocation).normalized * newAnticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
						UtilityFunctions.DebugMessage ("Anticipated Frames to target: " + newAnticipatedFramesToTarget);		
				}
				GuessedTargetLocation = targetLocation;
		
				UtilityFunctions.DebugMessage ("Guessed Target Location: " + targetLocation);
				UtilityFunctions.DebugMessage ("Projectile Location: " + transform.position);
		
				// adjust direction on new location
				distanceToTarget = targetLocation - transform.position;
		
				MidPoint = distanceToTarget * .5f + transform.position;
				MidPointDistance = Vector3.Distance (targetLocation, transform.position) / 2f;
				UtilityFunctions.DebugMessage ("Target: " + distanceToTarget);
				UtilityFunctions.DebugMessage ("Distance To Target: " + Vector3.Distance (targetLocation, transform.position));
				UtilityFunctions.DebugMessage ("Mid Point: " + MidPoint);
				UtilityFunctions.DebugMessage ("Mid Point Distance: " + MidPointDistance);
		
				UtilityFunctions.DebugMessage ("Arching Projectile velocity: " + (targetLocation - transform.position).normalized * ProjectileSpeed);
				transform.GetComponent<Rigidbody2D> ().velocity = (targetLocation - transform.position).normalized * ProjectileSpeed;
				UtilityFunctions.DebugMessage ("Velocity: " + transform.GetComponent<Rigidbody2D> ().velocity.magnitude);
				UtilityFunctions.DebugMessage ("Anticipated Time to target: " + distanceToTarget.magnitude / transform.GetComponent<Rigidbody2D> ().velocity.magnitude);
				IsMoving = true;
				NeedToCalculateProjectilePath = false;
		}
}
