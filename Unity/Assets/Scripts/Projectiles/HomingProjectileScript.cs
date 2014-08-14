using UnityEngine;
using System.Collections;

public class HomingProjectileScript : ProjectileScript
{
    public Transform Target;
    public Vector3 LastTargetPosition;
    public bool HasTargetChanged;

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position += (Target.position - transform.position).normalized * Time.deltaTime * ProjectileSpeed;
            LastTargetPosition = Target.position;
            HasTargetChanged = true;
        }
        else
        {
            Target = TowerFiredFrom.GetComponent<HomingTowerScript>().Target;

            if (Target == null && HasTargetChanged)
            {
                UtilityFunctions.DebugMessage("Force Added. " + (LastTargetPosition - transform.position).normalized * 50 * ProjectileSpeed);
                transform.rigidbody2D.AddForce((LastTargetPosition - transform.position).normalized * 50 * ProjectileSpeed);
                HasTargetChanged = false;
            }
            else if (Target != null)
            {
                HasTargetChanged = true;
            }
        }
    }
}
