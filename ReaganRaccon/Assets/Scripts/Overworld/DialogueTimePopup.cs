
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueTimePopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;
    public Image popupBackground;
    public TMP_Text popupText;

    [Header("Settings")]
    public float displayDuration = 1.5f;

    [Header("Popup Colors")]
    public Color addedColor =
        new Color32(54, 228, 122, 255);

    public Color removedColor =
        new Color32(255, 82, 82, 255);

    public Color textColor = Color.white;

    private Coroutine popupCoroutine;

    void Start()
    {
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    public void ShowTimeChange(float seconds)
    {
        if (seconds == 0f)
            return;

        if (popupCoroutine != null)
            StopCoroutine(popupCoroutine);

        popupCoroutine = StartCoroutine(
            DisplayPopup(seconds));
    }

    IEnumerator DisplayPopup(float seconds)
    {
        if (popupPanel == null ||
            popupBackground == null ||
            popupText == null)
        {
            yield break;
        }

        bool added = seconds > 0f;

        // Change the entire box color.
        popupBackground.color =
            added ? addedColor : removedColor;

        // Keep the text white.
        popupText.color = textColor;

        // Change the message.
        popupText.text = added
            ? "+" + seconds.ToString("0") + " SECONDS!"
            : seconds.ToString("0") + " SECONDS!";

        popupPanel.SetActive(true);

        yield return new WaitForSecondsRealtime(
            displayDuration);

        popupPanel.SetActive(false);
        popupCoroutine = null;
    }
}

