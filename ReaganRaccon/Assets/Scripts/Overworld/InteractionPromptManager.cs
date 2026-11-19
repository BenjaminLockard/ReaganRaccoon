
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;

    public TMP_Text promptText;

    private Dictionary<GameObject, string> activePrompts =
        new Dictionary<GameObject, string>();

    private GameObject currentSource;

    void Awake()
    {
        // Do not disable this script if another manager exists.
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning(
                "Duplicate InteractionPromptManager detected. " +
                "Using the latest instance.",
                this
            );
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
        // Forward requests to the active manager.
        if (Instance != this)
        {
            if (Instance != null)
            {
                Instance.ShowPrompt(source, message);
            }

            return;
        }

        if (source == null || promptText == null)
        {
            return;
        }

        if (!activePrompts.TryGetValue(source, out string oldMessage)
            || oldMessage != message)
        {
            activePrompts[source] = message;

            Debug.Log(
                "Prompt requested by: " + source.name,
                source
            );
        }

        RefreshPrompt();
    }

    public void HidePrompt(GameObject source = null)
    {
        // Forward requests to the active manager.
        if (Instance != this)
        {
            if (Instance != null)
            {
                Instance.HidePrompt(source);
            }

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
        {
            return;
        }

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

        currentSource = selectedSource;

        if (currentSource == null)
        {
            if (promptText.gameObject.activeSelf)
            {
                promptText.gameObject.SetActive(false);
            }

            return;
        }

        if (promptText.text != selectedMessage)
        {
            promptText.text = selectedMessage;
        }

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
