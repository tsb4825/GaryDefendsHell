﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CreepLoadout
{
    public Transform Creep;
    public Transform SpawnLocation;
}

public class Swarm
{
    public CreepLoadout CreepLoadOut;
    public int Quantity;
    public float TimeInBetweenCreeps;
    public float TimeOfNextCreep;
    public float TimeToNextSwarm;
    public float TimeOfNextSwarm;
    public bool ShowUnitDescription;
    public string UnitDescription;
}

public class Wave
{
    public Queue<Swarm> Swarms;
    public int WaveNumber;
    public float TimeToStartWave;
    public float TimeInBetweenNextWave;
    public bool IsActive;
    public float GetWaveDuration()
    {
        return Swarms.Sum(x => (x.Quantity - 1) * x.TimeInBetweenCreeps)
                + Swarms.Sum(x => x.TimeToNextSwarm)
                + TimeInBetweenNextWave;
    }
}