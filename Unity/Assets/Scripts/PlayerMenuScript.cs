using UnityEngine;
using System.Collections;

public class PlayerMenuScript : MonoBehaviour {
	public Transform Angel;

	private const int ButtonWidth = 84;
	private const int ButtonHeight = 30;
	private float ButtonX = Screen.width * .8f + (Screen.width * .2f / 2f) - ButtonWidth;

	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width * .8f, 0.0f, Screen.width * .2f, Screen.height), "");

		if (
			GUI.Button(
			new Rect(
			ButtonX,
			30f - (ButtonHeight / 2f),
			ButtonWidth,
			ButtonHeight
			),
			"Start Waves"
			)
			)
		{
			FindObjectsOfType<WaveScript>()[0].ReleaseNextWave ();
		}

		if (
			GUI.Button(
			new Rect(
			ButtonX,
			80f - (ButtonHeight / 2f),
			ButtonWidth,
			ButtonHeight
			),
			"Send Creep"
			)
			)
		{
			Transform enemy = (Transform)Instantiate (Resources.Load<Transform> ("Angel"), GameObject.FindGameObjectWithTag ("Spawn").transform.position, 
			                                          Quaternion.identity);
			enemy.GetComponent <CreepScript> ().Target = GameObject.FindGameObjectWithTag ("Base").transform;
		}

		if (
			GUI.Button(
			new Rect(
			ButtonX,
			130f - (ButtonHeight / 2f),
			ButtonWidth,
			ButtonHeight
			),
			"Send Player Creep"
			)
			)
		{
			Transform unit = (Transform)Instantiate (Resources.Load<Transform> ("PlayerAngel"), GameObject.FindGameObjectWithTag ("Base").transform.position, 
			                                          Quaternion.identity);
			unit.GetComponent <CreepScript> ().Target = GameObject.FindGameObjectWithTag ("Spawn").transform;
		}

		if (
			GUI.Button(
			new Rect(
			ButtonX,
			180f - (ButtonHeight / 2f),
			ButtonWidth,
			ButtonHeight
			),
			"Pause"
			)
			)
		{
			Time.timeScale = Time.timeScale == 0 ? 1: 0;
		}
	}
}
