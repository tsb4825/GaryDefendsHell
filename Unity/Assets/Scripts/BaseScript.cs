using UnityEngine;
using System.Collections;

public class BaseScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.tag == "Enemy" && collider is BoxCollider2D) {
			DestroyObject (collider.gameObject);
			GameObject.FindObjectOfType<PlayerScript>().LoseLives(collider.GetComponent<CreepScript>().LivesCost);
		}
	}
}
