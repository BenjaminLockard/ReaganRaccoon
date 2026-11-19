
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "SprintSceneB";

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");
        Application.Quit();
    }
}
