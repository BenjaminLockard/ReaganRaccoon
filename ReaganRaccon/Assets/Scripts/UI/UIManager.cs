using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    
    public GameObject pauseMenu, powerupPopup;
    public TMP_Text powerupName, powerupDesc, powerupDuration;
    private float powerupTime = 0f;
    private bool powerupTicking = false;

    public void OnPause(InputValue input)
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else
        {
            Unpause();
        }
    } 
    
    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    

    public void SceneReset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void DisplayTime(float timeToDisplay)
    {
        float seconds = Mathf.FloorToInt(timeToDisplay);

        powerupDuration.text = string.Format("{0:00}", seconds);
    }

    public void PowerupDisplay(string name, string desc, float duration)
    {
        powerupPopup.SetActive(true);
        powerupName.text = name;
        powerupDesc.text = desc;

        powerupTime = duration;
        powerupTicking = true;
    }

    private void Update()
    {
        if (powerupTicking && powerupTime <= 1)
        {
            powerupTicking = false;
            powerupPopup.SetActive(false);
        }
        else
        {
            DisplayTime(powerupTime);
            powerupTime -= Time.deltaTime;
        }
    }

    }
