using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StunDrainTowerScript :  Tower {
    public int NumberOfProjectiles;
    public Transform Projectile;
    public float StunTime;
    public float DrainSpeed;
    public float NextDrainTime;
    public IEnumerable<Transform> DrainTargets;
    public float DrainDamage;

    public override void Fire()
    {
        IEnumerable<Transform> targets = FindClosestTargets(NumberOfProjectiles);
        DrainTargets = targets;
        foreach(Transform target in targets)
        {
            // draw tenacle
            target.GetComponent<CreepScript>().AddAffliction(AfflictionTypes.Stun, StunTime, 0);
        }
        NextDrainTime = Time.time;
    }

    public override void Update()
    {
        if (Time.time >= NextFireTime)
        {
            Fire();
            NextFireTime = Time.time + AttackCooldown;
        }
        if (DrainTargets != null && NextDrainTime < Time.time)
        {
            foreach( Transform target in DrainTargets)
            {
                target.GetComponent<CreepScript>().TakeDamage(DrainDamage);
            }
        }
        base.Update();
    }

    private IEnumerable<Transform> FindClosestTargets(int targets)
    {
        if (targets >= Targets.Count)
            return Targets;
        else
        {
            IEnumerable<Creep> creeps = Targets.Select(x =>
                new Creep
                {
                    Transform = x,
                    Distance = (UtilityFunctions.UseUnitZPosition(x, x.transform.position) - UtilityFunctions.UseUnitZPosition(x, transform.position)).sqrMagnitude
                });
            return creeps.OrderBy(x => x.Distance).Take(targets).Select(x => x.Transform);
        }
    }

    private class Creep
    {
        public Transform Transform {get;set;}
        public float Distance { get; set; }
    }
}
