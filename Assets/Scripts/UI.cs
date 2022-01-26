using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static bool paused = false;
    public GameObject overlay;
    public Player player;

    private void Start()
    {
        HideCursor();
    }

    private void Update()
    {
        if (player.health > 0 && Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) {
                Resume();
            } else
            {
                Pause();
            }
        }
        if (player.health == 0)
        {
            ShowCursor();
        }
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

    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
