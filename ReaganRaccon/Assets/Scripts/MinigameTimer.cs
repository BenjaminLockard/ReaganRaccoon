using UnityEngine;
using TMPro;

public class MinigameTimer : MonoBehaviour
{
    public float timeLimit = 60f;
    public TMP_Text timerText;

    private float timeRemaining;
    private bool timerRunning;

    void Start()
    {
        timeRemaining = timeLimit;
        timerRunning = true;
        UpdateTimerText();
    }

    void Update()
    {
        if (timerRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                timerRunning = false;
                TimeIsUp();
            }

            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time: " + seconds;
    }

    void TimeIsUp()
    {
        Debug.Log("TIME IS UP!");
    }
}
