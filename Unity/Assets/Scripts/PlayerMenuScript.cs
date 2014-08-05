﻿using UnityEngine;
using System.Collections;

public class PlayerMenuScript : MonoBehaviour
{
    public Transform Angel;
    private const int ButtonWidth = 84;
    private const int ButtonHeight = 30;
    private float ButtonX = Screen.width * .8f + (Screen.width * .2f / 2f) - ButtonWidth;
    private GameState GameState;
    private bool IsGameStarted;

    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width * .8f, 0.0f, Screen.width * .2f, Screen.height), "");

        if (!FindObjectOfType<WaveScript>().AreAllCreepsReleased() && GameState == GameState.Running && !IsGameStarted)
        {
            if (GUI.Button(new Rect(ButtonX,30f - (ButtonHeight / 2f),ButtonWidth,ButtonHeight),"Start Waves"))
            {
                FindObjectsOfType<WaveScript>()[0].ReleaseNextWave();
                IsGameStarted = true;
            }
        }

        if (GUI.Button(new Rect(ButtonX,80f - (ButtonHeight / 2f),ButtonWidth,ButtonHeight),"Send Creep"))
        {
            Transform enemy = (Transform)Instantiate(Resources.Load<Transform>("Angel"), GameObject.FindGameObjectWithTag("Spawn").transform.position,
                                          Quaternion.identity);
            enemy.GetComponent<CreepScript>().Target = GameObject.FindGameObjectWithTag("Base").transform;
        }

        if (GUI.Button(new Rect(ButtonX,130f - (ButtonHeight / 2f),ButtonWidth,ButtonHeight),"Send Player Creep"))
        {
            Transform unit = (Transform)Instantiate(Resources.Load<Transform>("PlayerAngel"), GameObject.FindGameObjectWithTag("Base").transform.position,
                                          Quaternion.identity);
            unit.GetComponent<CreepScript>().Target = GameObject.FindGameObjectWithTag("Spawn").transform;
        }

        if (IsGameStarted)
        {
            if (GUI.Button(new Rect(ButtonX,180f - (ButtonHeight / 2f),ButtonWidth,ButtonHeight),"Pause"))
            {
                GameState = GameState == GameState.Running ? GameState.Paused : GameState.Running;
                Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            }
        }

        if (!FindObjectOfType<WaveScript>().AreAllCreepsReleased() && GameState == GameState.Running)
        {
            if (GUI.Button(new Rect(ButtonX,230f - (ButtonHeight / 2f),ButtonWidth,ButtonHeight),"Next Wave"))
            {
                FindObjectsOfType<WaveScript>()[0].ReleaseNextWave();
            }
        }
    }
}

public enum GameState
{
    Running,
    Paused
}