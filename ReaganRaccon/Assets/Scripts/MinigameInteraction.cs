using UnityEngine;

public class MinigameInteraction : MonoBehaviour, IInteractable
{
    public LoadNext loadNext;
    public string sceneName;
    public string type = "minigame";

    public void Interact()
    {
        loadNext.switchScene(type, sceneName);
    }
}