using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    // 10 minutes in seconds
    [SerializeField] private float timerDuration = 600f;
    // Reference to a UI Text element to display the timer
    public TextMeshProUGUI timerText;
    // Public Reference to this CountdownTimer, for use in other scripts
    public static CountdownTimer instance; 

    // Current time on timer in seconds
    private float currentTime;
    // Whether the timer is actively counting down (true) or paused (false)
    private bool isRunning = false;

    private void Start()
    {
        // Create reference for other scripts
        instance = this;
    }

    private void Update()
    {
        // If timer is counting down, then continue counting down and update the timer's UI
        if (isRunning && !GameManager.instance.freezeGame)
        {
            currentTime -= Time.deltaTime;

            // If timer is finished counting down, stop it and end the game
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;
                TimerFinished();
            }

            UpdateTimerText();
        }
    }

    // If timer has an non-positve currentTime, then timer is reset. Regardless, always unpauses the timer when called
    public void StartTimer()
    {
        // Ensure the timer starts with a valid duration
        if (currentTime <= 0f) 
        {
            ResetTimer();
        }

        Debug.Log("timer started");
        // timer is now counting down
        isRunning = true;
    }

    // Pause timer's count down
    public void StopTimer()
    {
        Debug.Log("timer stopped");

        // timer is now paused
        isRunning = false;
    }

    // Sets timer to its starting state, where currentTime = timerDuration and the timer does not start running 
    public void ResetTimer()
    {
        currentTime = timerDuration;
        UpdateTimerText();
        isRunning = false;
    }

    // Updates the UI timer the player sees
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Timer counts down to zero, so win the game
    private void TimerFinished()
    {
        Debug.Log("Timer Finished!");
        GameManager.instance.WinGame();
    }
}
