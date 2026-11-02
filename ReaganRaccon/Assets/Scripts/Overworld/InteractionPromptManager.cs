
using UnityEngine;
using TMPro;

public class InteractionPromptManager : MonoBehaviour
{
    public static InteractionPromptManager Instance;

    [Header("Prompt UI")]
    public TMP_Text promptText;

    private GameObject currentSource;
    private string currentMessage;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HidePrompt();
    }

    public void ShowPrompt(GameObject source, string message)
    {
        if (promptText == null || source == null)
            return;

        currentSource = source;
        currentMessage = message;

        promptText.text = currentMessage;
        promptText.gameObject.SetActive(true);
    }

    public void HidePrompt(GameObject source = null)
    {
        if (source != null && currentSource != source)
            return;

        currentSource = null;

        if (promptText != null)
            promptText.gameObject.SetActive(false);
    }
}

