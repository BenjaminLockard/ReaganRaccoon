
using UnityEngine;

public class InteractionPromptTrigger : MonoBehaviour
{
    [Header("Prompt Message")]
    public string promptMessage = "Press E to Interact";

    [Header("Optional Dialogue Manager")]
    public DialogueManager dialogueManager;

    private bool playerNearby;

    void OnEnable()
    {
        playerNearby = false;
    }

    void Start()
    {
        Debug.Log("PROMPT SCRIPT ACTIVE ON: " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            "PROMPT COLLISION: " + gameObject.name +
            " detected " + other.gameObject.name
        );

        if (other.CompareTag("Reagan"))
        {
            Debug.Log("REAGAN DETECTED BY PROMPT: " + gameObject.name);

            playerNearby = true;
            UpdatePrompt();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            playerNearby = true;
            UpdatePrompt();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            Debug.Log("REAGAN LEFT PROMPT: " + gameObject.name);

            playerNearby = false;
            HidePrompt();
        }
    }

    void OnDisable()
    {
        HidePrompt();
        playerNearby = false;
    }

    private void UpdatePrompt()
    {
        if (InteractionPromptManager.Instance == null)
            return;

        bool dialogueOpen =
            dialogueManager != null &&
            dialogueManager.IsDialogueActive();

        if (playerNearby && !dialogueOpen)
        {
            InteractionPromptManager.Instance.ShowPrompt(
                gameObject,
                promptMessage
            );
        }
        else
        {
            HidePrompt();
        }
    }

    private void HidePrompt()
    {
        if (InteractionPromptManager.Instance != null)
        {
            InteractionPromptManager.Instance.HidePrompt(gameObject);
        }
    }
}


