using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WaveScript : MonoBehaviour
{
    private Queue<Wave> Waves;
    public bool IsLevelStarted;
    public Transform Base;
    public bool ShowUnitDescriptionModal;
    public string UnitDescription;
    public Transform Creep;

    void Start()
    {
        Waves = this.GetComponent<LevelWavesLoadOut>().GetLevelWaves();
        this.GetComponent<PlayerScript>().TotalWaves = Waves.Count;
    }

    void Update()
    {
        if (IsLevelStarted)
        {
            CheckToReleaseWave();
            AddCreepsWaiting();
        }
    }

    void OnGUI()
    {
        if (ShowUnitDescriptionModal)
        {
            DrawModalWindow();
        }
    }
    
    private void DrawModalWindow()
    {
        GUI.ModalWindow(0, new Rect(Screen.width / 2 - 200, Screen.height / 2 - 75, 400, 200), ShowUnitDescription, "Super Sweet New Unit!");
    }

    void ShowUnitDescription(int windowID)
    {
        var texture = Creep.GetComponent<SpriteRenderer>().sprite.texture;
        GUI.DrawTexture(new Rect(200 - texture.width / 2, 30, texture.width, texture.height), texture, ScaleMode.StretchToFill, true, 10.0F); 
        GUI.Label(new Rect(50, 120, 300, 30), UnitDescription);
        if (GUI.Button(new Rect(170, 150, 60, 30), "Ok"))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            ShowUnitDescriptionModal = false;
        }
    }

    public void ReleaseNextWave()
    {
        IsLevelStarted = true;
        if (!AreAllWavesReleased())
        {
            var wave = Waves.Where(w => !w.IsActive).First();
            wave.IsActive = true;
            this.GetComponent<PlayerScript>().WaveNumber++;
            this.GetComponent<PlayerScript>().TimeOfNextWave = Time.time + wave.GetWaveDuration();
            if (Waves.Count(x => !x.IsActive) > 0)
            {
                Waves.Where(w => !w.IsActive).First().TimeToStartWave = Time.time + wave.GetWaveDuration(); ;
            }
        }
    }

    public bool AreAllWavesReleased()
    {
        var areAllWavesReleased = Waves.Where(x => !x.IsActive).Count() <= 0;
        if (areAllWavesReleased)
        {
            this.GetComponent<PlayerScript>().AreAllWavesReleased = true;
        }
        return areAllWavesReleased;
    }

    public bool AreAllCreepsReleased()
    {
        var areAllCreepsReleased = Waves.Count() <= 0;
        if (areAllCreepsReleased)
        {
            this.GetComponent<PlayerScript>().AreAllCreepsReleased = true;
        }
        return areAllCreepsReleased;
    }

    //add ability to have multiple swarms active
    private void AddCreepsWaiting()
    {
        if (Waves.Count > 0)
        {
            foreach (var wave in Waves.Where(x => x.IsActive))
            {
                if (wave.Swarms.Count > 0)
                {
                    Swarm swarm = wave.Swarms.Peek();
                    if (Time.time >= swarm.TimeOfNextCreep && Time.time > swarm.TimeOfNextSwarm)
                    {
                        swarm.TimeOfNextCreep = Time.time + swarm.TimeInBetweenCreeps;
                        if (swarm.ShowUnitDescription)
                        {
                            ShowUnitDescriptionModal = true;
                            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
                            UnitDescription = swarm.UnitDescription;
                            Creep = swarm.CreepLoadOut.Creep;
                            swarm.ShowUnitDescription = false;
                        }
                        Creep = (Transform)Instantiate(swarm.CreepLoadOut.Creep, swarm.CreepLoadOut.SpawnLocation.position,
                                          Quaternion.identity);
                        Creep.GetComponent<CreepScript>().Target = Base;
                        swarm.Quantity -= 1;
                        if (swarm.Quantity <= 0)
                        {
                            Swarm completedSwarm = wave.Swarms.Dequeue();
                            if (wave.Swarms.Count > 0)
                            {
                                wave.Swarms.Peek().TimeOfNextCreep = Time.time + completedSwarm.TimeToNextSwarm;
                            }
                        }
                    }
                }
            }
            Waves = new Queue<Wave>(Waves.Where(x => x.Swarms.Count > 0).OrderBy(x => x.WaveNumber));
            AreAllWavesReleased();
        }
    }

    private void CheckToReleaseWave()
    {
        if (Waves.Count(x => !x.IsActive) > 0)
        {
            var nextWave = Waves.Where(x => !x.IsActive).First();
            if (Time.time >= nextWave.TimeToStartWave)
            {
                ReleaseNextWave();
            }
        }
    }
}
