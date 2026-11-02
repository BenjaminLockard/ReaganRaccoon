using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    
    public GameObject pauseMenu;

    public void OnPause(InputValue input)
    {
        if (!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else
        {
            Unpause();
        }
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
