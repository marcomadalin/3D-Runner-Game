using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuTerminalController : Terminal
{
    [SerializeField] private MenuController menu = null;
    private void Start()
    {
        numCheats = 3;
        cheats = new string[3]  {"Unlock_levels", "Caserio", "Default_music" };
        cheatsDescription = new string[3] {"Unlocks all levels", "Cahnge all music to to ANUEL AA || BZRP Music Sessions #46", "Restores all music to default"};
    }
    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame) showConsole = !showConsole;

        if (showConsole && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (input == "Unlock_levels")
            {
                PlayerPrefs.SetInt("cheats", 1);
                PlayerPrefs.SetInt("levelsCheat", 1);
                menu.UnlockAllLevelsCheat();
            }
            else if (input == "Caserio")
            {
                PlayerPrefs.SetInt("caserio", 1);
                menu.CheatMusic();
            }
            else if (input == "Default_music")
            {
                PlayerPrefs.SetInt("caserio", 0);
                menu.DefaultMusic();
            }
            input = "";
        }
    }
}
