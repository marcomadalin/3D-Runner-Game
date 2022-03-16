using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    protected Vector2 scroll = new Vector2(0, 0);
    protected bool showConsole = false;
    protected string input = string.Empty;
    protected int numCheats = 0;
    protected string[] cheats = null;
    protected string[] cheatsDescription = null;

    protected void OnGUI()
    {
        if (!showConsole) return;
        float y = 0f;

        GUI.Box(new Rect(0, y, Screen.width, 100), "");

        Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * numCheats);

        scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

        for (int i = 0; i < numCheats; ++i)
        {

            string label = cheats[i] + " - " + cheatsDescription[i];
            Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
            GUI.Label(labelRect, label);
        }

        GUI.EndScrollView();

        y += 100f;

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);

    }
}
