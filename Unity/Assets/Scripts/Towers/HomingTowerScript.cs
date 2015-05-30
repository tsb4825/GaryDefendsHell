using UnityEngine;
using System.Collections;

public class HomingTowerScript : Tower
{
    public Transform Projectile;
    public float Force;


    public override void Fire()
    {
        UtilityFunctions.DebugMessage("Fire");
        var cannonPosition = transform.FindChild("ProjectileCannon").position;
        cannonPosition.z = 0;
        Transform bullet = (Transform)Instantiate(Projectile, cannonPosition, Quaternion.identity);
        bullet.GetComponent<HomingProjectileScript>().Target = Target;
        bullet.GetComponent<HomingProjectileScript>().TowerFiredFrom = transform;
        bullet.LookAt(Target);
        bullet.GetComponent<Rigidbody2D>().AddForce((Target.position - bullet.position).normalized * Force);
    }
}
