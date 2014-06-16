using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreepScript : MonoBehaviour
{
		public float MaxHitPoints;
		public float CurrentHitPoints;
		public Transform Target;
		public Transform WayPointTarget;
		public float UnitSpeed;
		public int LivesCost;
		public int GoldAwarded;
		public bool DebugMode;
		public bool IsControlledByPlayer;
		public bool IsFighting;
		public bool IsMovingTowardFighter;
		public Transform FightingTarget;
		public float AttackDamage;
		public float AttackCooldown;
		public float NextAttackTime;
		public const float DistanceBetweenMeleeFighters = .25f;
		public Vector2 size = new Vector2 (30, 5);
		public Texture2D healthBarEmpty;
		public Texture2D healthBarFull;
		public GUIStyle HealthBar;
		public List<Affliction> Afflictions;

		void Awake ()
		{
				Afflictions = new List<Affliction> ();
		}

		void OnGUI ()
		{
				HealthBar.alignment = TextAnchor.MiddleCenter;
				Vector3 point = Camera.main.WorldToScreenPoint (transform.position + new Vector3 (0, .5f));
				float guiY = Screen.height - point.y;
				// draw the background:
				GUI.BeginGroup (new Rect (point.x - (size.x / 2), guiY, size.x, size.y), HealthBar);
				GUI.Box (new Rect (0, 0, size.x, size.y), healthBarEmpty, HealthBar);
		
				// draw the filled-in part:
				GUI.BeginGroup (new Rect (0, 0, size.x * (CurrentHitPoints / MaxHitPoints), size.y), HealthBar);
				GUI.Box (new Rect (0, 0, size.x, size.y), healthBarFull, HealthBar);
				GUI.EndGroup ();
		
				GUI.EndGroup ();
		
		} 

		// Update is called once per frame
		void Update ()
		{
				if (WayPointTarget == null) {
						UtilityFunctions.DebugMessage ("Finding waypoint through update");
						WayPointTarget = UtilityFunctions.FindClosestWayPointToSelfAndTarget (transform, WayPointTarget, Target);
						if (WayPointTarget == null) {
								WayPointTarget = Target;
						}
				}
				if (IsFighting && FightingTarget != null) {
						if ((UtilityFunctions.UseUnitZPosition (transform, transform.position) - UtilityFunctions.UseUnitZPosition (transform, FightingTarget.position)).sqrMagnitude > DistanceBetweenMeleeFighters) {
								UtilityFunctions.DebugMessage ("Moving towards melee target.");
								IsMovingTowardFighter = true;
								float newUnitSpeed = Afflictions.Any (x => x.AfflictionType == AfflictionTypes.SpeedBoost) 
									? Afflictions.Where (x => x.AfflictionType == AfflictionTypes.SpeedBoost).Max (x => x.AffectAmount)
						: 1f;
				newUnitSpeed *= UnitSpeed;
								transform.position = Vector3.MoveTowards (transform.position, UseUnitZPosition (FightingTarget.position), Time.deltaTime * newUnitSpeed);
						} else {
								IsMovingTowardFighter = false;
								Attack ();
						}
				} else if (IsFighting) {
						IsMovingTowardFighter = false;
						IsFighting = false;
				} else {
						float newUnitSpeed = Afflictions.Any (x => x.AfflictionType == AfflictionTypes.SpeedBoost) 
						? Afflictions.Where (x => x.AfflictionType == AfflictionTypes.SpeedBoost).Max (x => x.AffectAmount)
						: 1f;
			newUnitSpeed *= UnitSpeed;
						if (newUnitSpeed != UnitSpeed)
				UtilityFunctions.DebugMessage ("Speed Boost - " + newUnitSpeed);
						transform.position = Vector3.MoveTowards (transform.position, UseUnitZPosition (WayPointTarget.transform.position), Time.deltaTime * newUnitSpeed);
				}

				Afflictions.RemoveAll (x => x.EndTime >= Time.time);
		}

		private void Attack ()
		{
				if (Time.time >= NextAttackTime) {
						UtilityFunctions.DebugMessage ("Attacking...");
						FightingTarget.GetComponent<CreepScript> ().TakeDamage (AttackDamage);
						NextAttackTime = Time.time + AttackCooldown;
				}
		}

		void OnTriggerEnter2D (Collider2D collider)
		{
				if (collider.tag == "Spawn" && IsControlledByPlayer) {
						Destroy (this.gameObject);
				}
				if (FightingTarget == null 
						&& collider is CircleCollider2D 
						&& collider.GetComponent<CreepScript> () != null  
						&& (!collider.GetComponent<CreepScript> ().IsFighting || collider.GetComponent<CreepScript> ().FightingTarget == transform)
						&& collider.GetComponent<CreepScript> ().IsControlledByPlayer != IsControlledByPlayer) {
						FightingTarget = collider.transform;
						IsFighting = true;
				}
		}

		void OnTriggerExit2D (Collider2D collider)
		{
				if (collider is CircleCollider2D && collider.transform == FightingTarget) {
						IsFighting = false;
						FightingTarget = null;
				}
		}

		Vector3 UseUnitZPosition (Vector3 position)
		{
				position.z = transform.position.z;
				return position;
		}

		public void TakeDamage (float damage)
		{
				CurrentHitPoints -= damage;
				if (CurrentHitPoints <= 0) {
						Destroy (transform.gameObject);
						UtilityFunctions.DebugMessage ("Enemy Killed");
						GameObject.FindObjectOfType<PlayerScript> ().AddGold (GoldAwarded);
				}
		}

		public void Heal (float hitPoints)
		{
				CurrentHitPoints += hitPoints;
				if (CurrentHitPoints > MaxHitPoints) {
						CurrentHitPoints = MaxHitPoints;
				}
		}

		public void AddAffliction (AfflictionTypes afflictionType, float duration, float affectAmount)
		{
				Afflictions.Add (new Affliction { AfflictionType = afflictionType, EndTime = Time.time + duration, AffectAmount = affectAmount});
		}
}
