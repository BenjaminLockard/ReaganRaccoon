
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MinigameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 60f;
    public TMP_Text timerText;

    [Header("When Time Runs Out")]
    public UnityEvent onTimeUp;

    private float timeRemaining;
    private bool timerRunning;
    private bool gameFinished;

    void Start()
    {
        timeRemaining = timeLimit;
        timerRunning = true;
        gameFinished = false;

        UpdateTimerText();
    }

    void Update()
    {
        if (!timerRunning || gameFinished)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;

            UpdateTimerText();
            TimeIsUp();
            return;
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (timerText == null)
            return;

        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = "Time: " + seconds;
    }

    void TimeIsUp()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        Debug.Log("TIME IS UP! PLAYER LOST!");

        onTimeUp?.Invoke();
    }

    // Call this when the player wins.
    // Prevents the timer from triggering a loss afterward.
    public void StopTimer()
    {
        timerRunning = false;
        gameFinished = true;
    }
}
