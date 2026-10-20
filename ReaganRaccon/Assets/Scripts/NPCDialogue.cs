
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    public DialogueManager.DialogueLine[] lines;

    [Header("References")]
    public DialogueManager dialogueManager;

    private bool playerNearby = false;
    private bool waitForKeyRelease = false;
    private bool activeConversation = false;

    void Update()
    {
        if (Keyboard.current == null || dialogueManager == null)
            return;

        // Reset when the conversation ends
        if (activeConversation &&
            !dialogueManager.IsDialogueActive())
        {
            activeConversation = false;
        }

        // Prevent the first E press from
        // immediately skipping the first line
        if (waitForKeyRelease)
        {
            if (!Keyboard.current.eKey.isPressed)
            {
                waitForKeyRelease = false;
            }

            return;
        }

        // Only respond when E is pressed
        if (!Keyboard.current.eKey.wasPressedThisFrame)
            return;

        // Advance an existing conversation
        if (dialogueManager.IsDialogueActive())
        {
            // Only the NPC that started the
            // conversation can advance it
            if (activeConversation)
            {
                dialogueManager.NextLine();
            }

            return;
        }

        // Start a conversation when Reagan is nearby
        if (playerNearby)
        {
            dialogueManager.StartDialogue(lines);

            activeConversation =
                dialogueManager.IsDialogueActive();

            waitForKeyRelease = activeConversation;
        }
    }

    // Detect when Reagan approaches the NPC
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            playerNearby = true;
        }
    }

    // Detect when Reagan walks away
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Reagan"))
        {
            playerNearby = false;
        }
    }
}

