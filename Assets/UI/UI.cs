using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static bool paused = false;
    public GameObject overlay;
    public Player player;
    public GameObject controls;
    public Image muteButton;
    public Slider slider;

    private void Start()
    {
        HideCursor();
    }

    private void Update()
    {
        if (Time.timeScale == 1 && player.finished == false && player.health > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) {
                Resume();
            } else
            {
                Pause();
            }
        }
        if (player.finished)
        {
            ShowCursor();
        }
        if (slider != null) AudioListener.volume = slider.value;
        if (muteButton != null)  muteButton.color = AudioListener.volume > 0 ? Color.white : Color.red;
    }

    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Resume()
    {
        overlay.SetActive(false);
        player.canMove = true;
        Time.timeScale = 1f;
        paused = false;
        HideCursor();
    }

    public void Pause()
    {
        player.canMove = false;
        overlay.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ShowCursor();
    }

    public void Controls()
    {
        overlay.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        overlay.SetActive(true);
        controls.SetActive(false);
    }

    public void Restart()
    {
        player.canMove = true;
        Time.timeScale = 1f;
        paused = false;
        HideCursor();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToMainMenu()
    {
        if (overlay != null) overlay.SetActive(false);
        player.canMove = true;
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void toggleMute()
    {
        slider.value = slider.value == 0 ? 1 : 0;
    }
}
