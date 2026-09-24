using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    private LoadNext loadNext;
    private ReaganInteractions reaganInteractions;
    public string type;
    public string name;
    void Start(){
        loadNext = GetComponent<LoadNext>();
        reaganInteractions = GetComponent<ReaganInteractions>();
    }
    public void Interact()
    {
        loadNext.switchScene(type, name);
        Debug.Log("SUCCESS! Reagan interacted with Square 2!");
    }
}