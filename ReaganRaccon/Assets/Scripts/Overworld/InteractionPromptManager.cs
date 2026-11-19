
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;

    [Header("Interaction Prompt UI")]
    public TMP_Text promptText;

    private Dictionary<GameObject, string> activePrompts =
        new Dictionary<GameObject, string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(
                "Duplicate InteractionPromptManager detected. " +
                "Keeping the original manager.",
                this
            );

            return;
        }

        Instance = this;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning(
                "InteractionPromptManager: Prompt Text is not assigned!",
                this
            );
        }
    }

    void Update()
    {
        if (Instance == this)
        {
            RefreshPrompt();
        }
    }

    public void ShowPrompt(GameObject source, string message)
    {
        if (Instance != this)
        {
            if (Instance != null)
                Instance.ShowPrompt(source, message);

            return;
        }

        if (source == null || promptText == null)
            return;

        activePrompts[source] = message;

        RefreshPrompt();
    }

    public void HidePrompt(GameObject source = null)
    {
        if (Instance != this)
        {
            if (Instance != null)
                Instance.HidePrompt(source);

            return;
        }

        if (source == null)
        {
            activePrompts.Clear();
        }
        else
        {
            activePrompts.Remove(source);
        }

        RefreshPrompt();
    }

    private void RefreshPrompt()
    {
        if (promptText == null)
            return;

        GameObject selectedSource = null;
        string selectedMessage = "";

        foreach (var prompt in activePrompts)
        {
            if (prompt.Key != null &&
                prompt.Key.activeInHierarchy)
            {
                selectedSource = prompt.Key;
                selectedMessage = prompt.Value;
                break;
            }
        }

        if (selectedSource == null)
        {
            if (promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(false);
            }

            return;
        }

        promptText.text = selectedMessage;

        if (!promptText.gameObject.activeSelf)
        {
            promptText.gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}


