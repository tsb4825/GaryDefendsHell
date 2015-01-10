using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SlowTower : Tower
{
    public float SlowTime;
    public float ReductionPercentage;

    public override void Fire()
    {
        foreach (var target in Targets)
        {
            target.GetComponent<CreepScript>().AddAffliction(AfflictionTypes.Slow, SlowTime, ReductionPercentage);
        }
    }
}

