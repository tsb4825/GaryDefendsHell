using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class MultiConstantAttackTowerScript : Tower
{
    public int NumberOfProjectiles;
    public Transform Projectile;

    public float PathWidth;

    public List<ActualVector> CollisionPaths;

    void Start()
    {
        // Get all paths
        var paths = PathingScript.BuildAIPaths();
        // Find all intersections of paths, remove duplicates
        List<ActualVector> collisionPaths = new List<ActualVector>();
        foreach (var path in paths)
        {
            for (var index = 0; index < path.Count; index++)
            {
                if (index != 0)
                {
                    Vector3 nodePath = UtilityFunctions.UseUnitZPosition(transform, path[index]) - UtilityFunctions.UseUnitZPosition(transform, path[index - 1]);
                    RaycastHit2D[] hits = Physics2D.RaycastAll(UtilityFunctions.UseUnitZPosition(transform, path[index - 1]), nodePath, nodePath.magnitude);
                    if (hits.Any(x => x.collider == transform.GetComponent<Collider2D>()) || transform.GetComponent<Collider2D>().OverlapPoint(path[index - 1]))
                    {
                        var collisionPoint1 = hits.Any(x => x.collider == transform.GetComponent<Collider2D>()) ? hits.First(y => y.collider == transform.GetComponent<Collider2D>()).point : (Vector2?)null;
                        if (!collisionPaths.Any(x => x.IsEqual(path[index - 1], nodePath, nodePath.magnitude, collisionPoint1)))
                        {
                            collisionPaths.Add(new ActualVector
                            {
                                Origin = path[index - 1],
                                Direction = nodePath,
                                Magnitude = nodePath.magnitude,
                                CollisionPoint1 = collisionPoint1,
                                StartsInsideCollider = transform.GetComponent<Collider2D>().OverlapPoint(path[index - 1]),
                                EndsInsideCollider = transform.GetComponent<Collider2D>().OverlapPoint(path[index])
                            });
                        }
                    }
                }
            }
        }

        // refine vectors to only inside the collider
        for (var pathIndex = 0; pathIndex < collisionPaths.Count; pathIndex++)
        {
            // if outside collider, set origin to collider, shorten magnitude
            if (!collisionPaths[pathIndex].StartsInsideCollider)
            {
                var newVector = collisionPaths[pathIndex].Direction - (collisionPaths[pathIndex].CollisionPoint1.GetValueOrDefault() - collisionPaths[pathIndex].Origin);
                collisionPaths[pathIndex].Origin = collisionPaths[pathIndex].CollisionPoint1.GetValueOrDefault();
                collisionPaths[pathIndex].Direction = newVector;
                collisionPaths[pathIndex].Magnitude = newVector.magnitude;
            }
            // if vector leaving collider, cut off
            if (!collisionPaths[pathIndex].EndsInsideCollider)
            {
                var endPoint = collisionPaths[pathIndex].Direction + collisionPaths[pathIndex].Origin;
                var oppositeDirectection = collisionPaths[pathIndex].Direction * -1;
                RaycastHit2D[] hits = Physics2D.RaycastAll(UtilityFunctions.UseUnitZPosition(transform, endPoint), oppositeDirectection, oppositeDirectection.magnitude);

                var collision = hits.First(x => x.collider == transform.GetComponent<Collider2D>()).point;
                collisionPaths[pathIndex].Direction = collision - collisionPaths[pathIndex].Origin;
                collisionPaths[pathIndex].Magnitude = (collision - collisionPaths[pathIndex].Origin).magnitude;
            }
        }

        CollisionPaths = collisionPaths;
    }

    public override void Fire()
    {
        // Fire constant attack projectiles on the randomly chosen vector
        for (int index = 0; index < NumberOfProjectiles; index++)
        {
            // select random path based on how long the vectors are within the collider
            var randomValue = Random.Range(0, 100);
            var sumMagnitudes = CollisionPaths.Sum(x => x.Magnitude);
            var percentageTotalRunThrough = 0f;
            ActualVector selectedPath = null;
            for (var pathIndex = 0; pathIndex < CollisionPaths.Count() && selectedPath == null; pathIndex++)
            {
                var currentPercentage = CollisionPaths[pathIndex].Magnitude / sumMagnitudes * 100;
                if (randomValue <= (currentPercentage + percentageTotalRunThrough))
                {
                    selectedPath = CollisionPaths[pathIndex];
                }
                percentageTotalRunThrough += currentPercentage;
            }
            Debug.DrawRay(selectedPath.Origin, selectedPath.Direction, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));

            Vector3 endpoint = GetRandomEndPoint(selectedPath);
            Transform projectile = (Transform)Instantiate(Projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<ConstantAttackProjectileScript>().TargetLocation = endpoint;
        }
    }

    private Vector2 GetRandomEndPoint(ActualVector selectedVector)
    {
        var endPoint = selectedVector.Origin + selectedVector.Direction;
        var randomPoint = Vector2.Lerp(selectedVector.Origin, endPoint,Random.Range(0f,1f));
        randomPoint.x += Random.Range(-.15f, .15f);
        randomPoint.y += Random.Range(-.15f, .15f);
        return randomPoint;
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
    public bool StartsInsideCollider { get; set; }
    public bool EndsInsideCollider { get; set; }
    public Vector2? CollisionPoint1 { get; set; }

    public bool IsEqual(Vector2 origin, Vector2 direction, float magnitude, Vector2? collisionPoint1)
    {
        return origin == Origin && direction == Direction && magnitude == Magnitude && collisionPoint1 == CollisionPoint1;
    }
}