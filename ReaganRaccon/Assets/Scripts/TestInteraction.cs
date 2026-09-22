using UnityEngine;

public class TestInteraction : MonoBehaviour, IInteractable
{
    private LoadNext loadNext;
    private ReaganInteractions reaganInteractions;
    void Start(){
        loadNext = GetComponent<LoadNext>;
        reaganInteractions = GetComponent<ReaganInteractions>;
    }
    public void Interact()
    {
        loadNext.switchScene(SceneType, SceneName);
        Debug.Log("SUCCESS! Reagan interacted with Square 2!");
    }
}