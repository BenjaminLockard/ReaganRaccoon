
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueManager.DialogueLine[] lines;

    [Header("References")]
    public DialogueManager dialogueManager;

    [Header("Interaction Prompt")]
    public GameObject interactionPrompt;

    private bool playerNearby;
    private bool waitForKeyRelease;
    private bool activeConversation;

    void Start()
    {
        UpdatePrompt();
    }

    void Update()
    {
        if (dialogueManager == null)
            return;

        if (activeConversation &&
            !dialogueManager.IsDialogueActive())
        {
            activeConversation = false;
        }

        UpdatePrompt();

        if (Keyboard.current == null)
            return;

        if (waitForKeyRelease)
        {
            if (!Keyboard.current.eKey.isPressed)
                waitForKeyRelease = false;

            return;
        }

        if (!Keyboard.current.eKey.wasPressedThisFrame)
            return;

        if (dialogueManager.IsDialogueActive())
        {
            if (activeConversation &&
                !dialogueManager.IsShowingChoices())
            {
                dialogueManager.AdvanceDialogue();
            }

            return;
        }

        if (playerNearby)
        {
            dialogueManager.StartDialogue(lines);

            activeConversation =
                dialogueManager.IsDialogueActive();

            waitForKeyRelease = activeConversation;

            UpdatePrompt();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
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
            UpdatePrompt();
        }
    }

    void UpdatePrompt()
    {
        if (interactionPrompt == null)
            return;

        bool showPrompt =
            playerNearby &&
            dialogueManager != null &&
            !dialogueManager.IsDialogueActive();

        interactionPrompt.SetActive(showPrompt);
    }
}
