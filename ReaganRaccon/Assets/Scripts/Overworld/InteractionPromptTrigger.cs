
using UnityEngine;

public class InteractionPromptTrigger : MonoBehaviour
{
    [Header("Prompt Message")]
    public string promptMessage = "Press E to Interact";

    [Header("Optional Dialogue Manager")]
    public DialogueManager dialogueManager;

    private bool playerNearby = false;
    private bool promptShowing = false;

    void OnEnable()
    {
        playerNearby = false;
        promptShowing = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
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
            playerNearby = false;
            HidePrompt();
        }
    }

    void Update()
    {
        if (playerNearby)
        {
            UpdatePrompt();
        }
    }

    private void UpdatePrompt()
    {
        if (InteractionPromptManager.Instance == null)
            return;

        bool dialogueOpen =
            dialogueManager != null &&
            dialogueManager.IsDialogueActive();

        if (dialogueManager != null)
        {

            if (playerNearby && !dialogueOpen && !dialogueManager.IsEntryAllowed())
            {
                InteractionPromptManager.Instance.ShowPrompt(
                    gameObject,
                    promptMessage
                );

                promptShowing = true;
            }
            else
            {
                HidePrompt();
            }
        } else
        {

            if (playerNearby)
            {
                InteractionPromptManager.Instance.ShowPrompt(
                    gameObject,
                    promptMessage
                );

                promptShowing = true;
            }
            else
            {
                HidePrompt();
            }
        }
    }

    private void HidePrompt()
    {
        if (!promptShowing)
            return;

        if (InteractionPromptManager.Instance != null)
        {
            InteractionPromptManager.Instance.HidePrompt(
                gameObject
            );
        }

        promptShowing = false;
    }

    void OnDisable()
    {
        HidePrompt();
        playerNearby = false;
    }
}
