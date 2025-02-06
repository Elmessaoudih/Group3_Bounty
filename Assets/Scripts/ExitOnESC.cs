using UnityEngine;

public class ExitGameOnEscape : MonoBehaviour
{
    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    // Quits the game
    void QuitGame()
    {
            Application.Quit();
    }
}
