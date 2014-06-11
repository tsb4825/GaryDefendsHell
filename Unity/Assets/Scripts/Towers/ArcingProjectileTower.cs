using UnityEngine;
using System.Collections;

public class ArcingProjectileTower : Tower {
	public Transform Projectile;
	public int ProjectilesPerFire;

	public override void Fire ()
	{
		UtilityFunctions.DebugMessage ("Arcing Projectile fire");
		var cannonPosition = transform.FindChild ("ProjectileCannon").position;
		cannonPosition.z = 0;
		Transform bullet = (Transform)Instantiate (Projectile, cannonPosition, Quaternion.identity);
		bullet.GetComponent<ArcingProjectile> ().Target = Target;
	}
}
