using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreepLoadout {
	public Transform Creep;
	public Transform SpawnLocation;
}

public class Swarm {
	public CreepLoadout CreepLoadOut;
	public int Quantity;
	public float TimeInBetweenCreeps;
	public float TimeOfNextCreep;
	public float TimeToNextSwarm;
	public float TimeOfNextSwarm;
}

public class Wave {
	public Queue<Swarm> Swarms;
	public int WaveNumber;
	public float TimeToStartWave;
	public float TimeInBetweenNextWave;
	public bool IsActive;
}