using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject menu = null;
    [SerializeField] private playerController player = null;
    public static bool inMenu = false;

    private void Awake()
    {
        inMenu = false;
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !LevelController.inLevelComplete) ChangePaused();
    }

    public void ResumeGame()
    {
        ChangePaused();
    }

    public void MainMenu()
     {
        ResumeTime();
        SceneManager.LoadScene("Menu");
     }

    public void RestartLevel()
    {
        ResumeTime();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("cheats", 0);
        PlayerPrefs.SetInt("caserio", 0);
        PlayerPrefs.SetInt("levelsCheat", 0);
    }

    private void ChangePaused()
    {
        inMenu = !inMenu;

        if (inMenu)
        {
            Time.timeScale = 0;
            player.PauseWalkSound();
        }
        else
        {
            Time.timeScale = 1;
            player.ResumeWalkSound();
        }

        menu.SetActive(inMenu);
    }

    private void ResumeTime()
    {
        inMenu = false;
        player.ResumeWalkSound();
        Time.timeScale = 1;
    }
}
