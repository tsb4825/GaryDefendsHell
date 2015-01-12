using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MultiConstantAttackTowerScript : Tower
{
    public int NumberOfProjectiles;
    public Transform Projectile;
    public float TowerRange;

    public float PathWidth;

    public override void Fire()
    {
        // Get all paths
        var paths = PathingScript.BuildAIPaths();

        // Find all intersections of paths, remove duplicates
        List<Vector3> collisionPaths = new List<Vector3>();
        foreach(var path in paths)
        {
            for (var index = 0; index < path.Count; index++ )
            {
                if (index != 0)
                {
                    Vector3 nodePath = UtilityFunctions.UseUnitZPosition(transform, path[index - 1]) - UtilityFunctions.UseUnitZPosition(transform, path[index]);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(UtilityFunctions.UseUnitZPosition(transform, path[index]), nodePath, nodePath.magnitude);
                    
                    if (hits.Any(x => x.collider == transform.collider2D))
                    {
                        if (!collisionPaths.Any(x => x == nodePath))
                        {
                            collisionPaths.Add(nodePath);
                        }
                    }
                }
            }
        }

        // select random path

        // find random spot in collider and path collision to fire node with path width random

        // Fire constant attack projectiles in at random paths
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
