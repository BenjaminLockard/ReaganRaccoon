using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    public GameObject pauseMenu;

    public void OnPause(InputValue input)
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    } 
    
    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    

    public void SceneReset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
