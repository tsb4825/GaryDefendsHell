using UnityEngine;
using System.Collections;
using System.Linq;

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

    void Update()
    {
        if (NeedToCalculateProjectilePath)
        {
            CalculateProjectilePath();
        }
        ActualFramesToTarget += 1;

        Vector3 unitPositionZeroZ = new Vector3(transform.position.x, transform.position.y, 0);
        if (IsMoving && (GuessedTargetLocation - unitPositionZeroZ).sqrMagnitude >= MidPointDistance)
        {
            float distanceToMidPoint = Vector3.Distance(MidPoint, unitPositionZeroZ);
            float newScale = 1f + ((MidPointDistance - distanceToMidPoint) / MidPointDistance * MaxAdditionalScale);
            transform.localScale = new Vector3(newScale, newScale, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, newScale);
        }
        // going down
        else if (IsMoving)
        {
            float distanceAwayFromMidPoint = Vector3.Distance(unitPositionZeroZ, MidPoint);
            float newScale = 1f + ((MidPointDistance - distanceAwayFromMidPoint) / MidPointDistance * MaxAdditionalScale);
            transform.localScale = new Vector3(newScale, newScale, 0);
            transform.position = new Vector3(transform.position.x, transform.position.y, newScale);
            if (newScale <= 1)
            {
                //UtilityFunctions.DebugMessage("Actual Target Location: " + Target != null ? Target.position.ToString() : "");
                UtilityFunctions.DebugMessage("Actual Frames to Target: " + ActualFramesToTarget);
                IsMoving = false;
                transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                RemoveProjectileTime = Time.time + ProjectileAliveTime;
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                UtilityFunctions.DebugMessage("Actual projectile hit: " + transform.position);

                // hit anything?
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.zero);
                if (hits != null)
                {
                    foreach (RaycastHit2D hit in hits.Where(x => x.transform.tag == "Enemy"))
                    {
                        hit.transform.GetComponent<CreepScript>().TakeDamage(Damage);
                    }
                }
                Destroy(gameObject);
            }
        }
        else
        {
            if (Time.time >= RemoveProjectileTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void CalculateProjectilePath()
    {
        Vector3 targetLocation = Target.position;
        Vector3 distanceToTarget = targetLocation - transform.position;

        // Guess location when reaching target
        float anticipatedFramesToTarget = distanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime * 2);
        Transform targetsTarget = Target.GetComponent<CreepScript>().WayPointTarget;
        bool IsFighting = Target.GetComponent<CreepScript>().IsFighting;
        bool IsMovingTowardUnit = Target.GetComponent<CreepScript>().IsMovingTowardFighter;
        float TargetSpeed = Target.GetComponent<CreepScript>().UnitSpeed;
        Transform targetsFightingTarget = Target.GetComponent<CreepScript>().FightingTarget;

        if (!IsFighting)
        {
            targetLocation = (UtilityFunctions.UseUnitZPosition(Target, targetsTarget.position) - targetLocation).normalized * anticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
            // adjust by time off due to not knowing location of target after time
            Vector3 newDistanceToTarget = targetLocation - transform.position;
            float newAnticipatedFramesToTarget = newDistanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime * 2);
            targetLocation = (UtilityFunctions.UseUnitZPosition(Target, targetsTarget.position) - targetLocation).normalized * newAnticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
            UtilityFunctions.DebugMessage("Anticipated Frames to target: " + newAnticipatedFramesToTarget);
        }
        else if (IsFighting && IsMovingTowardUnit)
        {
            targetLocation = (UtilityFunctions.UseUnitZPosition(Target, targetsFightingTarget.position) - targetLocation).normalized * anticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
            // adjust by time off due to not knowing location of target after time
            Vector3 newDistanceToTarget = targetLocation - transform.position;
            float newAnticipatedFramesToTarget = newDistanceToTarget.magnitude / (ProjectileSpeed * Time.deltaTime * 2);
            targetLocation = (UtilityFunctions.UseUnitZPosition(Target, targetsFightingTarget.position) - targetLocation).normalized * newAnticipatedFramesToTarget * TargetSpeed * Time.deltaTime + targetLocation;
            UtilityFunctions.DebugMessage("Anticipated Frames to target: " + newAnticipatedFramesToTarget);
        }
        GuessedTargetLocation = targetLocation;

        UtilityFunctions.DebugMessage("Guessed Target Location: " + targetLocation);

        // adjust direction on new location
        distanceToTarget = targetLocation - transform.position;

        MidPoint = distanceToTarget * .5f + transform.position;
        MidPointDistance = Vector3.Distance(targetLocation, transform.position) / 2f;

        transform.GetComponent<Rigidbody2D>().velocity = (targetLocation - transform.position).normalized * ProjectileSpeed;
        IsMoving = true;
        NeedToCalculateProjectilePath = false;
    }
}
