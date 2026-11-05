
using UnityEngine;

public class CampEntranceGate : MonoBehaviour
{
    [Header("Dialogue Reference")]
    public DialogueManager dialogueManager;

    [Header("Gate Colliders")]
    public Collider2D northGate;
    public Collider2D westGate;

    void Start()
    {
        UpdateGates();
    }

    void Update()
    {
        UpdateGates();
    }

    void UpdateGates()
    {
        if (dialogueManager == null)
            return;

        bool shouldBlock =
            !dialogueManager.IsEntryAllowed() ||
            dialogueManager.IsEntryDenied();

        if (northGate != null)
            northGate.enabled = shouldBlock;

        if (westGate != null)
            westGate.enabled = shouldBlock;
    }
}

