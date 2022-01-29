using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject controls;

    public void StartGame()
    {
        SceneManager.LoadScene("Main");
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

}
