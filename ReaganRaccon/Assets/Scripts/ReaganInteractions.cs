using UnityEngine;
using UnityEngine.InputSystem;

public class ReaganInteractions : MonoBehaviour
{
    //public string sceneName;
    //public string SceneType; 
    private IInteractable currentInteractable;

    void Update()
    {
        if (currentInteractable != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            Debug.Log("Press E to interact");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable) &&
            currentInteractable == interactable)
        {
            currentInteractable = null;
        }
    }
}