using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileTowerScript : Tower
{
    public Transform Projectile;

    public override void Fire()
    {
        UtilityFunctions.DebugMessage("Projectile fire");
        var cannonPosition = transform.FindChild("ProjectileCannon").position;
        cannonPosition.z = 0;
        Transform bullet = (Transform)Instantiate(Projectile, cannonPosition, Quaternion.identity);
        bullet.rigidbody2D.velocity = (Target.position - bullet.position).normalized * bullet.GetComponent<ProjectileScript>().ProjectileSpeed;
        UtilityFunctions.DebugMessage("Projectile velocity: " + (Target.position - bullet.position).normalized * bullet.GetComponent<ProjectileScript>().ProjectileSpeed);
    }

}
