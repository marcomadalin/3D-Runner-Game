using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private playerController playerController = null;
    [SerializeField] private AudioSource music = null;
    [SerializeField] private AudioSource caserio = null;
    [SerializeField] private AudioSource defaultMusic = null;
    [SerializeField] private GameObject levelCompletePopUp = null;
    [SerializeField] private AudioSource levelCompleteSound = null;
    public static bool inLevelComplete = false;
    private int currentLevel = 0;
    private const int credits = 6;

    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");

        if (PlayerPrefs.GetInt("caserio") == 1) CheatMusic();
        else DefaultMusic();

        string actScene = SceneManager.GetActiveScene().name;
        char level = actScene[actScene.Length - 1];
        currentLevel = (int)(level - '0');
        playerController.PlayerInput.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("rebinds", string.Empty));
        inLevelComplete = false;
    }

    public void LevelComplete()
    {
        music.Stop();
        levelCompleteSound.Play();

        if (PlayerPrefs.GetInt("cheats") == 0)
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
            if (currentLevel + 1 != credits) PlayerPrefs.SetInt("Level" + (currentLevel + 1).ToString(), 1);
        }

        if (currentLevel + 1 == credits) SceneManager.LoadScene("Credits");
        else
        {
            levelCompletePopUp.SetActive(true);
            inLevelComplete = true;
        }
    }

    public void LoadNextLevel()
    {
        inLevelComplete = false;
        SceneManager.LoadScene("Level" + (currentLevel + 1).ToString());
    }

    public void CheatMusic()
    {
        music.Stop();
        music = caserio;
        music.Play();
    }

    public void DefaultMusic()
    {
        music.Stop();
        music = defaultMusic;
        music.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GameOverScreen()
    {
        StartCoroutine(WaitGameOver());
    }

    public void SaveLastLevel()
    {
        PlayerPrefs.SetInt("lastLevel", currentLevel);
    }

    IEnumerator WaitGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        PlayerPrefs.SetInt("lastLevel", currentLevel);
        music.Stop();
        SceneManager.LoadScene("GameOver");
    }

}
