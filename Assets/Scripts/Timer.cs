using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI timerText;
    // Change this number to change how much time you have.
    float remainingTime = 30f;

    // Update is called once per frame
    void Update()
    {
        // This is a timer to count down how long you have left.

        remainingTime -= Time.deltaTime;

        if (remainingTime < 1)
        {
            remainingTime = 0;
            LoseScreen();
            enabled = false;
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float amount)
    {
        remainingTime += amount;
    }

    void LoseScreen()
    {
        Time.timeScale = 0f;

        // The code for losing goes here.

        // inventory.gameObject.SetActive(false);
        // loseImage.SetActive(true);
    }
}