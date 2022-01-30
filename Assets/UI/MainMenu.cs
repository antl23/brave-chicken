using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject controls;
    public GameObject credits;

    public void StartGame()
    {
        SceneManager.LoadScene("Farm");
        Time.timeScale = 1f;
    }

    public void Controls()
    {
        menu.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        menu.SetActive(true);
        controls.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        menu.SetActive(true);
        credits.SetActive(false);
    }
}
