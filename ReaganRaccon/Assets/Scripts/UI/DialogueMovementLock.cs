
using UnityEngine;

public class DialogueMovementLock : MonoBehaviour
{
    [Header("References")]
    public DialogueManager dialogueManager;
    public ReaganControls reaganControls;

    private bool wasInDialogue = false;

    void LateUpdate()
    {
        if (dialogueManager == null ||
            reaganControls == null)
            return;

        bool inDialogue =
            dialogueManager.IsDialogueActive();

        if (inDialogue && !wasInDialogue)
        {
            // Clear movement and mouse input.
            reaganControls.ResetMovement();

            // Disable Reagan's movement controls.
            reaganControls.enabled = false;
        }
        else if (!inDialogue && wasInDialogue)
        {
            // Restore controls after dialogue.
            reaganControls.ResetMovement();
            reaganControls.enabled = true;
        }

        wasInDialogue = inDialogue;
    }

    void OnDisable()
    {
        // Restore movement if this component is disabled.
        if (reaganControls != null &&
            wasInDialogue)
        {
            reaganControls.ResetMovement();
            reaganControls.enabled = true;
        }
    }
}

