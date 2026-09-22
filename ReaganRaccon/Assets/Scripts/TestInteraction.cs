using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    private LoadNext loadNext;
    private ReaganInteractions reaganInteractions;
    private string type;
    private string name;
    void Start(){
        loadNext = GetComponent<LoadNext>();
        reaganInteractions = GetComponent<ReaganInteractions>();
    }
    public void Interact()
    {
        type = ReaganInteractions.GetComponent<string>()["sceneType"];
        name = ReaganInteractions.GetComponent<string>()["sceneName"];
        loadNext.switchScene(type, name);
        Debug.Log("SUCCESS! Reagan interacted with Square 2!");
    }
}