using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private AudioSource music = null;

    private void Awake()
    {
        music.Play();
    }
    public void RestartLastLevel()
    {
        music.Stop();
        SceneManager.LoadScene("Level" + PlayerPrefs.GetInt("lastLevel").ToString());
    }

    public void LoadMainMenu()
    {
        music.Stop();
        SceneManager.LoadScene("Menu");
    }
}
