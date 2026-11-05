
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;

    [Header("Prompt UI")]
    public TMP_Text promptText;

    private Dictionary<GameObject, string> activePrompts =
        new Dictionary<GameObject, string>();

    private GameObject currentSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(
                "Duplicate InteractionPromptManager found!",
                this
            );

            enabled = false;
            return;
        }

        Instance = this;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }

    public void ShowPrompt(GameObject source, string message)
    {
        if (promptText == null || source == null)
            return;

        if (activePrompts.TryGetValue(source, out string oldMessage)
            && oldMessage == message)
        {
            return;
        }

        activePrompts[source] = message;
        RefreshPrompt();
    }

    public void HidePrompt(GameObject source = null)
    {
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
            if (prompt.Key != null && prompt.Key.activeInHierarchy)
            {
                selectedSource = prompt.Key;
                selectedMessage = prompt.Value;
                break;
            }
        }

        currentSource = selectedSource;

        if (currentSource == null)
        {
            promptText.gameObject.SetActive(false);
            return;
        }

        if (promptText.text != selectedMessage)
            promptText.text = selectedMessage;

        if (!promptText.gameObject.activeSelf)
            promptText.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
