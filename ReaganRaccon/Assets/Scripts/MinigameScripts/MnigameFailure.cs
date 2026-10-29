
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameFailure : MonoBehaviour
{
    [Header("Minigame Scene")]
    [Tooltip("Leave blank to use this object's scene.")]
    public string minigameSceneName;

    private bool failureTriggered;

    public void TriggerFailure()
    {
        if (failureTriggered)
            return;

        failureTriggered = true;

        string sceneName = string.IsNullOrEmpty(minigameSceneName)
            ? gameObject.scene.name
            : minigameSceneName;

        Debug.Log("MINIGAME LOST: " + sceneName);

        RewardManager rewardManager =
            FindFirstObjectByType<RewardManager>();

        if (rewardManager != null)
        {
            // Show the lose window, never the reward window.
            rewardManager.ShowLoseWindow();
        }
        else
        {
            Debug.LogWarning("RewardManager not found!");
            GameManager.Instance.setRootTrue();
        }

        // Remove the failed minigame scene.
        SceneManager.UnloadSceneAsync(sceneName);
    }
}


