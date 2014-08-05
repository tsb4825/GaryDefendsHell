using UnityEngine;
using System.Collections;

public class MultiConstantAttackTowerScript : Tower
{
    public int NumberOfProjectiles;
    public Transform Projectile;
    public float TowerRange;

    public override void Fire()
    {
        // Fire constant attack projectiles in random directions
        for (int index = 0; index < NumberOfProjectiles; index++)
        {
            Vector3 endpoint = GetRandomEndPoint();
            Transform projectile = (Transform)Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<ConstantAttackProjectileScript>().TargetLocation = endpoint;
        }
    }

    private Vector3 GetRandomEndPoint()
    {
        return new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(TowerRange * -1, TowerRange), 0f);
    }

    public override void Update()
    {
        if (Time.time >= NextFireTime)
        {
            Fire();
            NextFireTime = Time.time + AttackCooldown;
        }
        base.Update();
    }
}
