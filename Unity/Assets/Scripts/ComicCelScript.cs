using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComicCelScript : MonoBehaviour
{
    public float ZoomSpeed = 4;
    public List<Vector3> ScreenPositions;
    public bool WaitingOnMouseClick;
    private int positionIndex;

    void Start()
    {
        positionIndex = 0;
        transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("StoryBoardExample");
        ScreenPositions = new List<Vector3>{ 
			new Vector3(-11.7f, 5.0f, -10f), 
			new Vector3(11.4f, 5.0f, -10f), 
			new Vector3(-11.7f, -5.0f, -10f), 
			new Vector3(11.4f, -5.0f, -10f)
		};
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        if (WaitingOnMouseClick)
        {
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 100, 200, 100), "Click To Continue");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            WaitingOnMouseClick = false;
            if (positionIndex >= 3)
            {
                // go to level
            }
            else
            {
                positionIndex++;
            }
        }
        if (!WaitingOnMouseClick)
        {
            var cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position += (ScreenPositions[positionIndex] - cameraPosition).normalized * ZoomSpeed * Time.deltaTime;
        }
        if (ScreenPositions[positionIndex].CameraIsCloseEnough(GameObject.FindGameObjectWithTag("MainCamera").transform.position))
        {
            WaitingOnMouseClick = true;
        }
    }
}
