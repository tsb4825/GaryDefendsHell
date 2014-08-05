using UnityEngine;
using System.Collections;

public class AOETowerScript : Tower
{
    public float AttackDamage;

    public override void Fire()
    {
        UtilityFunctions.DebugMessage("Fire");
        foreach (var target in Targets)
        {
            target.GetComponent<CreepScript>().TakeDamage(AttackDamage);
        }
    }
}
