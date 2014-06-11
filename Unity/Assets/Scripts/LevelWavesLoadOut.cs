using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

public class LevelWavesLoadOut: MonoBehaviour
{
		public TextAsset LevelWaveConfiguration;
		public Transform SpawnPoint1;
		public Transform SpawnPoint2;

		public Queue<Wave> GetLevelWaves ()
		{
				var waves = new Queue<Wave> ();
				XDocument levelXml = XDocument.Parse (LevelWaveConfiguration.text);
				GameObject.FindObjectOfType<PlayerScript> ().Gold = int.Parse (levelXml.Element ("Level").Attribute ("StartingGold").Value);
				IEnumerable<XElement> waveElements = levelXml.Descendants ("Level").Descendants ("Waves").Descendants ("Wave");
				int waveNumber = 0;
				foreach (XElement waveNode in waveElements) {
						Wave wave = new Wave ();
						wave.Swarms = new Queue<Swarm> ();
						IEnumerable<XElement> swarms = waveNode.Descendants ("Swarms").Descendants ("Swarm");
						UtilityFunctions.DebugMessage (swarms.Count () + " of swarms in xml");
						foreach (XElement swarmNode in swarms) {
								string creepType = swarmNode.Descendants ("Creep").First ().Value;
								Transform creep;
								try {
										creep = Resources.Load<Transform> (creepType);
								} catch {
										throw new Exception ("Unable to find prefab - " + creepType);
								}
								int quantity = int.Parse (swarmNode.Descendants ("Quantity").First ().Value);
								float timeInBetweenCreeps = float.Parse (swarmNode.Descendants ("TimeInBetweenCreeps").First ().Value);
								int spawnLocation = int.Parse (swarmNode.Descendants ("SpawnLocation").First ().Value);
								float timeToNextSwarm = float.Parse (swarmNode.Descendants ("TimeToNextSwarm").First ().Value);

								wave.Swarms.Enqueue (new Swarm 
					                    { 
					CreepLoadOut = new CreepLoadout 
						{ 
							Creep = creep, 
							SpawnLocation = GetSpawnPoint(spawnLocation) 
						}, 
						Quantity = quantity, 
						TimeInBetweenCreeps = timeInBetweenCreeps,
					TimeToNextSwarm = timeToNextSwarm
					});
						}
						wave.WaveNumber = waveNumber;
						waveNumber++;
						wave.TimeInBetweenNextWave = float.Parse (waveNode.Descendants ("TimeInBetweenNextWave").First ().Value);
						waves.Enqueue (wave);
				}

				return waves;
		}

		private Transform GetSpawnPoint (int spawnLocation)
		{
				switch (spawnLocation) {
				case 1:
						return SpawnPoint1;
				case 2:
						return SpawnPoint2;
				default:
						throw new Exception ("Spawn Point not supported");
				}
		}
}