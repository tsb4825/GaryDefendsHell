using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GuiDisplayScript
{
    public static void ConfirmModal(string text, Action confirm, Action decline)
    {
        GUI.Label(new Rect(50, 50, 300, 30), text);
        if (GUI.Button(new Rect(140, 80, 60, 30), "Yes"))
        {
            confirm();
        }
        if (GUI.Button(new Rect(210, 80, 60, 30), "No"))
        {
            decline();
        }
    }
}
