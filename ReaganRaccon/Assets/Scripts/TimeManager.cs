using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [Header("Time Values")]
    [SerializeField] private float baseTime = 180;
    [SerializeField] private float currentTime = 0;
    [SerializeField] private bool ticking = true;

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

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Start()
    {
        ticking = true;
        currentTime = baseTime;
    }

    void Update()
    {
        if (currentTime <= 0)
        {
            ticking = false;
            timerText.text = "0:00 TIME UP - Press R To Replay";
        }
        else if (ticking)
        {
            DisplayTime(currentTime);
            currentTime -= Time.deltaTime;
        }
    }
}
