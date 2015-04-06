using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class CreditsScript : MonoBehaviour {
    private List<string> creditsArray;
    public float creditSpeed;
    private List<Rect> creditsRectangle = new List<Rect>();
    public GUIStyle creditsStyle;
    public TextAsset creditsText;

	void Start () {
        var creditsText = GetCreditsText();
        Debug.Log(creditsText);
        creditsArray = creditsText;
        for(var index = 0; index < creditsArray.Count; index++)
        {
            creditsRectangle.Add(new Rect(new Rect(Screen.width / 2 - 150, Screen.height + (30 * index), 300, 100)));
        }
	}

    void OnGUI()
    {
        for (int i = 0; i < creditsArray.Count; i++)
        {
            GUI.Label(creditsRectangle[i], creditsArray[i], creditsStyle);
            Rect tempRect = creditsRectangle[i];
            tempRect.y = tempRect.y - creditSpeed;
            creditsRectangle[i] = tempRect;
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Application.LoadLevel("MainScreen");
        }
    }

    private List<string> GetCreditsText()
    {
        List<string> credits = new List<string>();
        string text = creditsText.text;
        foreach (var line in text.Split('\n'))
        {
            credits.Add(line);
        }
        return credits;
    }
}
