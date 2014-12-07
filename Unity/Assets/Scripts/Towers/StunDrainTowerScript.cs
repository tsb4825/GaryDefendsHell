using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StunDrainTowerScript :  Tower {
    public int NumberOfAttackers;
    public Transform Projectile;
    public float StunTime;
    public float DrainSpeed;
    public float NextDrainTime;
    public List<Transform> DrainTargets;
    public float DrainDamage;
    public int DrainTimes;
    public int DrainTimeCount;

    public StunDrainTowerScript()
    {
        DrainTargets = new List<Transform>();
    }

    public override void Fire()
    {
        Debug.Log("Firing");
        DrainTimeCount = 0;
        DrainTargets = FindClosestTargets(NumberOfAttackers);
        Debug.Log("Targets stunning: " + DrainTargets.Count);
        DrainTargets.RemoveAll(x => x == null);
        foreach (Transform target in DrainTargets)
        {
            // draw tenacle
            target.GetComponent<CreepScript>().AddAffliction(AfflictionTypes.Stun, StunTime, 0);
        }
        NextDrainTime = Time.time;
    }

    public override void Update()
    {
        if (DrainTargets != null && DrainTargets.Count > 0 && Time.time >= NextDrainTime && DrainTimeCount < DrainTimes)
        {
            Debug.Log("Drain Count: " + DrainTimeCount);
            Debug.Log("DrainTargets Count: " + DrainTargets.Count);
            DrainTargets.RemoveAll(x => x == null);
            foreach( Transform target in DrainTargets)
            {
                target.GetComponent<CreepScript>().TakeDamage(DrainDamage);
            }
            NextDrainTime = Time.time + DrainSpeed;
            DrainTimeCount = DrainTimeCount += 1;
        }
        base.Update();
    }

    private List<Transform> FindClosestTargets(int targets)
    {
        Debug.Log("Targets Count: " + Targets.Count);
        if (targets >= Targets.Count)
            return Targets.ToList();
        else
        {
            List<Creep> creeps = Targets.Select(x =>
                new Creep
                {
                    Transform = x,
                    Distance = (UtilityFunctions.UseUnitZPosition(x, x.transform.position) - UtilityFunctions.UseUnitZPosition(x, transform.position)).sqrMagnitude
                }).ToList();
            return creeps.OrderBy(x => x.Distance).Take(targets).Select(x => x.Transform).ToList();
        }
    }

    private class Creep
    {
        public Transform Transform {get;set;}
        public float Distance { get; set; }
    }
}
