using UnityEngine;

[CreateAssetMenu(fileName = "GameManager", menuName = "GameManager", order = 1)]
public class GameManager : ScriptableObject
{
    public bool gameIsPaused;
    public GameObject MainMenu;
    public GameObject Player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        Debug.Log(Cursor.visible);
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;
        if (gameIsPaused)
        {
            Debug.Log("handle pause");
            Player.SetActive(false);
            MainMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Player.SetActive(false);
            MainMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
