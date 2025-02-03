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
#if UNITY_EDITOR
        // If in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // If in a built application, quit the game
            Application.Quit();
#endif
    }
}
