using UnityEngine;
using System.Collections;

public class ComicCelScript : MonoBehaviour {
	public float ZoomSpeed = 4;
	public Vector3 Position1 = new Vector3(-11.7f, 5.0f, -10f);
	public bool WaitingOnMouseClick;

	void OnGUI()
	{
		GUI.color = Color.black;
		if (WaitingOnMouseClick) {
			GUI.Label (new Rect (Screen.width / 2 - 100, Screen.height - 100, 200, 100), "Click To Continue");
				}
	}

	// Update is called once per frame
	void Update () {
		if (!WaitingOnMouseClick) {
						var cameraPosition = GameObject.FindGameObjectWithTag ("MainCamera").transform.position;
						GameObject.FindGameObjectWithTag ("MainCamera").transform.position += (Position1 - cameraPosition).normalized * ZoomSpeed * Time.deltaTime;
				}
		if (Position1.IsCloseEnough(GameObject.FindGameObjectWithTag ("MainCamera").transform.position)) {
			UtilityFunctions.DebugMessage ("Got here");
			WaitingOnMouseClick = true;
				}
	}
}
