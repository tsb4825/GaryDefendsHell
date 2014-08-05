using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class CreditsScript : MonoBehaviour {
    private List<string> creditsArray;
    public float creditSpeed;
    private List<Rect> creditsRectangle = new List<Rect>();

	void Start () {
        var creditsText = GetCreditsText();
        Debug.Log(creditsText);
        creditsArray = creditsText;
        int index = 0;
        foreach(var credit in creditsArray)
        {
            creditsRectangle.Add(new Rect(new Rect(Screen.width / 2 - 150, Screen.height + (30 * index), 300, 100)));
            index++;
        }
	}

    void OnGUI()
    {
        for (int i = 0; i < creditsArray.Count; i++)
        {
            GUI.Label(creditsRectangle[i], creditsArray[i]);
            Rect tempRect = creditsRectangle[i];
            tempRect.y = tempRect.y - creditSpeed;
            creditsRectangle[i] = tempRect;
        }
    }

    private List<string> GetCreditsText()
    {
        List<string> credits = new List<string>();
        credits.Add("Dedicated to");
        credits.Add("Mr. Logan S. Byars Esquire");
        credits.Add(string.Empty);
        credits.Add("A Game By");
        credits.Add("Tim Byars");
        credits.Add(string.Empty);
        credits.Add("Concept By");
        credits.Add("Tim Byars");
        credits.Add(string.Empty);
        credits.Add("Programming");
        credits.Add("Tim Byars");
        credits.Add(string.Empty);
        credits.Add("QA (The \"Real\" Work)");
        credits.Add("Tim Byars");
        credits.Add(string.Empty);
        credits.Add("Coffee Getter");
        credits.Add("Tim Byars");
        credits.Add(string.Empty);
        credits.Add("Special Thanks");
        credits.Add("A true brony - Joe H.");
        credits.Add("Unity");
        credits.Add("Unity Forums");
        credits.Add("Google Searches");
        credits.Add("All the people smarter than me that code");
        credits.Add("Coffee");
        credits.Add(string.Empty);
        credits.Add("Not Special Thanks");
        credits.Add("My Job");
        credits.Add("Childhood diseases I get as an adult");
        credits.Add("Diarrhea");
        credits.Add(string.Empty);

        return credits;
    }
}
