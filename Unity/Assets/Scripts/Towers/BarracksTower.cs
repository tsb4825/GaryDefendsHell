using UnityEngine;
using System.Collections;
using System.Linq;

public class BarracksTower : Tower
{
    public Transform Attacker;
    private GameObject[] SpawnLocations;
    private int? LastTargetedLocation;

    void Start()
    {
        SpawnLocations = GameObject.FindGameObjectsWithTag("Spawn");
    }

    public override void Fire()
    {
        UtilityFunctions.DebugMessage("Fire");
        Transform unit = (Transform)Instantiate(Attacker, transform.position, Quaternion.identity);
        unit.GetComponent<CreepScript>().Target = GetSpawnLocation();
    }

    private Transform GetSpawnLocation()
    {
        if (LastTargetedLocation == null || LastTargetedLocation.GetValueOrDefault() + 1 >= SpawnLocations.Count())
        {
            LastTargetedLocation = 0;
        }
        else
        {
            LastTargetedLocation++;
        }
        UtilityFunctions.DebugMessage("Spawn Location: " + SpawnLocations[LastTargetedLocation.GetValueOrDefault()].name);
        return SpawnLocations[LastTargetedLocation.GetValueOrDefault()].transform;
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
