using UnityEngine;
using UnityEngine.InputSystem;

public class HideOnInteract : MonoBehaviour
{
    public GameObject thisPanel;
    
    private void OnInteract()
    {
        thisPanel.SetActive(false);
    }

    //private void Start()
    //{
    //    thisPanel.SetActive(true);
    //}
}
