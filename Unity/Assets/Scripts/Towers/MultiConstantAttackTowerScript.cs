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
        List<ActualVector> collisionPaths = new List<ActualVector>();
        foreach(var path in paths)
        {
            for (var index = 0; index < path.Count; index++ )
            {
                if (index != 0)
                {
                    Vector3 nodePath = UtilityFunctions.UseUnitZPosition(transform, path[index]) - UtilityFunctions.UseUnitZPosition(transform, path[index - 1]);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(UtilityFunctions.UseUnitZPosition(transform, path[index - 1]), nodePath, nodePath.magnitude);
                    if (hits.Any(x => x.collider == transform.GetComponent<Collider2D>()) || transform.GetComponent<Collider2D>().OverlapPoint(path[index - 1]))
                    {
                        var collisionPoint1 = hits.Any(x => x.collider == transform.GetComponent<Collider2D>()) ? hits.First(y => y.collider == transform.GetComponent<Collider2D>()).point : (Vector2?)null;
                        var collisionPoint2 = hits.Count(z => z.collider == transform.GetComponent<Collider2D>()) == 2 
                            ? hits.Where(z => z.collider == transform.GetComponent<Collider2D>()).Skip(1).First().point : (Vector3?)null;
                        if (!collisionPaths.Any(x => x.IsEqual(path[index - 1], nodePath, nodePath.magnitude, collisionPoint1, collisionPoint2)))
                        {
                            collisionPaths.Add(new ActualVector
                            {
                                Origin = path[index - 1], 
                                Direction = nodePath, 
                                Magnitude = nodePath.magnitude, 
                                CollisionPoint1 = collisionPoint1, 
                                CollisionPoint2 = collisionPoint2
                            });
                            Debug.Log("Origin: " + path[index - 1] + "Path: " + nodePath);
                            //Debug.DrawRay(path[index - 1], nodePath, new Color(Random.Range(0f, 1f), Random.Range(0f,1f),Random.Range(0f,1f)));
                        }
                    }
                }
            }
        }

        Debug.Break();

        // refine vectors to only inside the collider
        for(var pathIndex = 0; pathIndex < collisionPaths.Count; pathIndex++)
        {
            var didVectorStartInside = true;
            // if outside collider, set origin to collider, shorten magnitude
            if (!transform.GetComponent<Collider2D>().OverlapPoint(collisionPaths[pathIndex].Origin))
            {
                var newVector = collisionPaths[pathIndex].Direction - (collisionPaths[pathIndex].CollisionPoint1.GetValueOrDefault() - collisionPaths[pathIndex].Origin);
                collisionPaths[pathIndex].Origin = collisionPaths[pathIndex].CollisionPoint1.GetValueOrDefault();
                collisionPaths[pathIndex].Direction = newVector;
                collisionPaths[pathIndex].Magnitude = newVector.magnitude;
                didVectorStartInside = false;
            }
            // if vector leaving collider, cut off
            if ((!didVectorStartInside && collisionPaths[pathIndex].CollisionPoint2 != null) || (didVectorStartInside && collisionPaths[pathIndex].CollisionPoint1 != null))
            {
                // use first collision point if started inside collider
                var newVector = (didVectorStartInside ? collisionPaths[pathIndex].CollisionPoint1.GetValueOrDefault() : collisionPaths[pathIndex].CollisionPoint2.GetValueOrDefault()) - collisionPaths[pathIndex].Origin;
                 collisionPaths[pathIndex].Direction = newVector;
                 collisionPaths[pathIndex].Magnitude = newVector.magnitude;
            }
            Debug.Log("Origin: " + collisionPaths[pathIndex].Origin + "Path: " + collisionPaths[pathIndex].Direction + "Magnitude: " + collisionPaths[pathIndex].Magnitude);
            Debug.DrawRay(collisionPaths[pathIndex].Origin, collisionPaths[pathIndex].Direction, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        }
        Debug.Break();
        // if magnitude hits collider, cap magnitude at collider

        // select random path based on how long the vectors are within the collider
        ActualVector randomPath = collisionPaths[Random.Range(0, collisionPaths.Count() - 1)];
        //Debug.DrawRay(randomPath.Origin, randomPath.Direction, Color.red);

        // Fire constant attack projectiles on the randomly chosen vector
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

public class ActualVector
{
    public Vector2 Origin { get; set; }
    public Vector2 Direction { get; set; }
    public float Magnitude { get; set; }
    public Vector2? CollisionPoint1 { get; set; }
    public Vector2? CollisionPoint2 { get; set; }

    public bool IsEqual(Vector2 origin, Vector2 direction, float magnitude, Vector2? collisionPoint1, Vector2? collisionPoint2)
    {
        return origin == Origin && direction == Direction && magnitude == Magnitude && collisionPoint1 == CollisionPoint1 && collisionPoint2 == CollisionPoint2;
    }
}