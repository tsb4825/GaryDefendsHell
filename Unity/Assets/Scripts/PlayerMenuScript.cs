using UnityEngine;
using System.Collections;

public class PlayerMenuScript : MonoBehaviour {
	public Transform Angel;

	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 30;
		
		GUI.Box(new Rect(Screen.width * .8f, 0.0f, Screen.width * .2f, Screen.height), "");
		
		// Draw a button to start the game
		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width * .8f + (Screen.width * .2f / 2f) - buttonWidth,
			30f - (buttonHeight / 2f),
			buttonWidth,
			buttonHeight
			),
			"Start Waves"
			)
			)
		{
			FindObjectsOfType<WaveScript>()[0].ReleaseNextWave ();
		}

		if (
			GUI.Button(
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width * .8f + (Screen.width * .2f / 2f) - buttonWidth,
			80f - (buttonHeight / 2f),
			buttonWidth,
			buttonHeight
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
			// Center in X, 2/3 of the height in Y
			new Rect(
			Screen.width * .8f + (Screen.width * .2f / 2f) - buttonWidth,
			130f - (buttonHeight / 2f),
			buttonWidth,
			buttonHeight
			),
			"Send Player Creep"
			)
			)
		{
			Transform unit = (Transform)Instantiate (Resources.Load<Transform> ("PlayerAngel"), GameObject.FindGameObjectWithTag ("Base").transform.position, 
			                                          Quaternion.identity);
			unit.GetComponent <CreepScript> ().Target = GameObject.FindGameObjectWithTag ("Spawn").transform;
		}
	}
}
