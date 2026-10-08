using UnityEngine;
using UnityEngine.InputSystem;

public class ReaganInteractions : MonoBehaviour
{
    private IInteractable currentInteractable;

    public GameObject interactionPrompt;

    void Update()
    {
        if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            interactionPrompt.SetActive(true);

            Debug.Log("Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) &&
            currentInteractable == interactable)
        {
            currentInteractable = null;
            interactionPrompt.SetActive(false);
        }
    }
}