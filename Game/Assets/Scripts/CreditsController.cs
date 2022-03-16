using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField] AudioSource music = null;
    [SerializeField] AudioSource click = null;

    private void Start()
    {
        music.Play();
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasReleasedThisFrame)
        {
            click.Play();
            music.Stop();
            SceneManager.LoadScene("Menu");
        }
    }
}
