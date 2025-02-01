using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        // Check for Space Bar input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogError("pauseMenuUI is not assigned in the Inspector!");
            return;
        }

        Debug.Log("Resuming Game");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resume game time
        GameIsPaused = false;
    }

    void Pause()
    {
        if (pauseMenuUI == null)
        {
            Debug.LogError("pauseMenuUI is not assigned in the Inspector!");
            return;
        }

        Debug.Log("Pausing Game");
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}