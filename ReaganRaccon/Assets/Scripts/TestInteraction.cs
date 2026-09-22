using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("SUCCESS! Reagan interacted with Square 2!");
    }
}