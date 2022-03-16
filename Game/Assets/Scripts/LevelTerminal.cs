using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelTerminal : Terminal
{
    [SerializeField] private LevelController levelController = null;
    [SerializeField] private insectFather insectController = null;
    [SerializeField] private playerController playerController = null;
    [SerializeField] private obstaclesCheat ocb = null;
    [SerializeField] private obstaclesCheat oce = null;
    [SerializeField] private obstaclesCheat oct = null;


    private void Start()
    {
        numCheats = 6;
        cheats = new string[6] {"NoCollision","Collision", "Invincible", "Set_aliens X", "Caserio", "Default_music"};
        cheatsDescription = new string[6] {"You can go through the obstacles", "Collisions are activated again", "Boss is always defeated", "Set the number of aliens to X", "Cahnge all music to to ANUEL AA || BZRP Music Sessions #46", "Restores all music to default" };
    }
    private void Update()
    {
        if (Keyboard.current.f1Key.wasPressedThisFrame) showConsole = !showConsole;

        if (showConsole && Keyboard.current.enterKey.wasPressedThisFrame)
        {
 
            if (input == "Caserio")
            {
                PlayerPrefs.SetInt("caserio", 1);
                levelController.CheatMusic();
            }
            else if (input == "Default_music")
            {
                PlayerPrefs.SetInt("caserio", 0);
                levelController.DefaultMusic();
            }
            else if (input == "Invincible")
            {
                PlayerPrefs.SetInt("cheats", 1);
                playerController.invincible = true;
            }
            else if (input.Split(' ')[0] == "Set_aliens")
            {
                PlayerPrefs.SetInt("cheats", 1);
                int numAliens = int.Parse(input.Split(' ')[1]);
                if (numAliens > 0 && numAliens < 91) insectController.setInsects(numAliens);
            }
            else if (input == "NoCollision")
            {
                PlayerPrefs.SetInt("cheats", 1);
                if (ocb != null) ocb.invencible();
                if (oce != null) oce.invencible();
                if (oct != null) oct.invencible();
            }
            else if (input == "Collision")
            {
                if (ocb != null) ocb.vencible();
                if (oce != null) oce.vencible();
                if (oct != null) oct.vencible();
            }
            input = "";
        }
    }
}
