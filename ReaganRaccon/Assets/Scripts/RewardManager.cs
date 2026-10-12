
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardManager : MonoBehaviour
{
    // Assign these in the Inspector
    public GameObject rewardWindow;
    public GameObject rewardCamera;

    // Checks for Enter while the reward window is open
    void Update()
    {
        if (rewardWindow != null &&
            rewardWindow.activeInHierarchy &&
            Keyboard.current != null &&
            (Keyboard.current.enterKey.wasPressedThisFrame ||
             Keyboard.current.numpadEnterKey.wasPressedThisFrame))
        {
            ContinueToMainGame();
        }
    }

    // Called when the minigame finishes
    public void ShowRewardWindow()
    {
        rewardCamera.SetActive(true);
        rewardWindow.SetActive(true);
    }

    // Called when Continue is clicked or Enter is pressed
    public void ContinueToMainGame()
    {
        Debug.Log("CONTINUE BUTTON CLICKED!");

        // Reactivate the main game
        GameManager.Instance.setRootTrue();

        // Hide the reward screen and its camera
        rewardWindow.SetActive(false);
        rewardCamera.SetActive(false);
    }
}
