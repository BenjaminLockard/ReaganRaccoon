using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;

public class UIManager : MonoBehaviour
{
    
    public GameObject pauseMenu, powerupPopup;
    public TMP_Text powerupName, powerupDesc, powerupDuration;
    private float powerupTime = 0f;
    private bool powerupTicking = false;
    private Vector2 restingPlace;

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

    public IEnumerator PowerupDisplay(string name, string desc, float duration)
    {
        powerupPopup.SetActive(true);
        powerupName.text = name;
        powerupDesc.text = desc;


        if (powerupPopup.GetComponent<RectTransform>().anchoredPosition != restingPlace)
        {
            powerupPopup.GetComponent<RectTransform>().anchoredPosition = (restingPlace + new Vector2(-300f, 0f));
            for (int i = 0; i < 30; i++)
            {
                powerupPopup.GetComponent<RectTransform>().anchoredPosition += new Vector2(10f, 0f);
                yield return new WaitForSeconds(0.001f);
            }
        }

        powerupPopup.GetComponent<RectTransform>().anchoredPosition = restingPlace;
        powerupTime = duration;
        powerupTicking = true;
    }

    public IEnumerator PowerupHide()
    {
        powerupTicking = false;

        for (int i = 0; i < 30; i++)
            {
                powerupPopup.GetComponent<RectTransform>().anchoredPosition += new Vector2(-10f, 0f);
                yield return new WaitForSeconds(0.001f);
        }

        powerupPopup.SetActive(false);
    }


    private void Start()
    {
        restingPlace = powerupPopup.GetComponent<RectTransform>().anchoredPosition;
        powerupPopup.GetComponent<RectTransform>().anchoredPosition = (restingPlace + new Vector2(-300f, 0f));
    }

    private void Update()
    {
        if (powerupTicking && powerupTime <= 1)
        {
            StartCoroutine(PowerupHide());
        }
        else if (powerupTicking)
        {
            DisplayTime(powerupTime);
            powerupTime -= Time.deltaTime;
        }
    }

    }
