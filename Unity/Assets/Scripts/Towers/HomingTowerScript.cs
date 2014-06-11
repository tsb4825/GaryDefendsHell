using UnityEngine;
using System.Collections;

public class HomingTowerScript : Tower {

	public Transform Projectile;
	
	public override void Fire ()
	{
		UtilityFunctions.DebugMessage ("Fire");
		var cannonPosition = transform.FindChild ("ProjectileCannon").position;
		cannonPosition.z = 0;
		Transform bullet = (Transform)Instantiate (Projectile, cannonPosition, Quaternion.identity);
		bullet.GetComponent<HomingProjectileScript> ().Target = Target;
		bullet.GetComponent<HomingProjectileScript> ().TowerFiredFrom = transform;
	}
}
