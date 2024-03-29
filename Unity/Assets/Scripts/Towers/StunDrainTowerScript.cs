﻿using UnityEngine;
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
        DrainTimeCount = 0;
        DrainTargets = (List<Transform>)FindClosestTargetsToBase(NumberOfAttackers);
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
            DrainTargets.RemoveAll(x => x == null);
            foreach( Transform target in DrainTargets)
            {
                target.GetComponent<CreepScript>().TakeDamage(DrainDamage);
            }
            NextDrainTime = Time.time + DrainSpeed;
            DrainTimeCount += 1;
        }
        base.Update();
    }
}