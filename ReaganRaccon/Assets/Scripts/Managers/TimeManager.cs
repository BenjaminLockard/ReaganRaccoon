
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Values")]
    [SerializeField] private float baseTime = 60;
    [SerializeField] private float currentTime = 0;
    [SerializeField] private bool ticking = false;

    [Header("EndGame")]

    public TMP_Text timerText;

    IEnumerator Type(TMP_Text dest, string text, float typeSpeed)
    {
        dest.text = "";

        foreach (char letter in text)
        {
            dest.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
    }

    public void ChangeTime(float time)
    {
        currentTime += time;

        if (currentTime < 0f)
            currentTime = 0f;

        DisplayTime(currentTime);
    }

    public void StartTimer()
    {
        if (currentTime <= 0f)
            return;

        ticking = true;
    }

    public void PauseTimer()
    {
        ticking = false;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timerText == null)
            return;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format(
            "{0:00}:{1:00}",
            minutes,
            seconds
        );
    }

    void Start()
    {
        currentTime = baseTime;

        // Timer waits until the ranger dialogue ends.
        ticking = false;

        DisplayTime(currentTime);
    }

    void Update()
    {
        if (!ticking)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            ticking = false;

            if (timerText != null)
            {
                timerText.text =
                    "0:00 TIME'S UP";
            }

            StartCoroutine(EndGame());
            return;
        }

        DisplayTime(currentTime);
    }

    public IEnumerator EndGame()
    {
        
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
