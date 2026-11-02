
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueManager.DialogueLine[] lines;

    [Header("References")]
    public DialogueManager dialogueManager;

    private bool playerNearby;
    private bool waitForKeyRelease;
    private bool activeConversation;

    void Update()
    {
        if (Keyboard.current == null ||
            dialogueManager == null)
            return;

        if (activeConversation &&
            !dialogueManager.IsDialogueActive())
        {
            activeConversation = false;
        }

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
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
            playerNearby = false;
    }
}


