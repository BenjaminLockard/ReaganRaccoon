
using UnityEngine;

public class InteractionPromptTrigger : MonoBehaviour
{
    [Header("Prompt Message")]
    public string promptMessage = "Press E to Interact";

    [Header("Optional Dialogue Manager")]
    public DialogueManager dialogueManager;

    private bool playerNearby;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            playerNearby = true;
            ShowPrompt();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            playerNearby = false;

            if (InteractionPromptManager.Instance != null)
                InteractionPromptManager.Instance.HidePrompt(gameObject);
        }
    }

    void Update()
    {
        if (!playerNearby)
            return;

        if (InteractionPromptManager.Instance == null)
            return;

        if (dialogueManager != null &&
            dialogueManager.IsDialogueActive())
        {
            InteractionPromptManager.Instance.HidePrompt(gameObject);
        }
        else
        {
            ShowPrompt();
        }
    }

    private void ShowPrompt()
    {
        if (InteractionPromptManager.Instance != null)
        {
            InteractionPromptManager.Instance.ShowPrompt(
                gameObject,
                promptMessage);
        }
    }
}

