using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadNext : MonoBehaviour
{
    //if object tag has "tag" minigame and pressing action causes the new scene to load

    private bool isCollidingWithObject;
    public string SceneName;
    public string type;
    //[SerializeField] GameObject Root; 
    public GameObject removed;
    private bool canInteract = false;

    void OnTriggerStay2D(Collider2D other){
            if(other.gameObject.CompareTag("Reagan")){
                canInteract = true;
                Debug.Log("Can Interact");
            }
            else{
                canInteract = false;
            }
    }
    
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Reagan")){
            canInteract = false;
            Debug.Log("Can't Interact");
        }
    }
    
    
    void OnInteract(InputValue value){
        if(canInteract){
            Debug.Log("Loading Game");
            switchScene(type, SceneName);
        }
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("WE HIT EACH OTTER");
        if (collision.gameObject.CompareTag("Reagan"))
        {
            Debug.Log("WE HIT EACH OTTER");
            //isCollidingWithObject = true;
            switchScene(type, SceneName);
  
            //adding additive causes the scene to load on top of each other 
            //which should allow the main game to be active 
            //however switching
        }
        
        else
        {
            isCollidingWithObject = false;
        }
        
        //sets that regan is colliding with the object
        //allows the player to press the interaction button
        //within controls
        //}
    */

    public void switchScene(string type, string SceneName)
    {
        if (type == "minigame")
        {
            Destroy(removed);
            GameManager.Instance.setRootFalse();
            //disable root object here from the main world
            
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            //Loads the minigame additively
            //Main gets disabled

            //rooty.SetActive(false)
            //SceneManager.SetActiveScene(SceneName);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
            
        }                    
        else if (type == "switchback")
{
    Destroy(removed);

    // Unload the completed minigame
    SceneManager.UnloadSceneAsync(SceneName);

    // Keep the main game disabled
    GameManager.Instance.setRootFalse();

    // Find the reward manager outside Root
    RewardManager rewardManager =
        FindFirstObjectByType<RewardManager>();

    if (rewardManager != null)
    {
        rewardManager.ShowRewardWindow();
    }
    else
    {
        Debug.LogWarning("RewardManager not found!");

        // Fallback so the player is not stuck
        GameManager.Instance.setRootTrue();
    }

            //reloads the root scene 
        }
        else
        {
            Destroy(removed);
            GameManager.Instance.setRootTrue();
            //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName));
            
            //DontDestroyOnLoad(root);
            
            //SceneManager.LoadScene(SceneName);

        }
        //adding additive causes the scene to load on top of each other 
        //which should allow the main game to be active 
        //however switching to the main game should destory the minigame to not waste data
        //this can be called 
    }
/*
    //https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html

    public void loadMinigame(string name)
    {
        StartCorotutine(LoadSceneNext());
    }

    IEnumerator LoadLevel()
    {
    //animation plays here
    SceneManager.LoadSceneAsync(name);
    //the main world will not be loaded unfortunately
    //however im keeping it incase we need to load something in the background
    //LIKE TRANSITIONING TO AREAS
    }
*/

    /*private void Update(){
        if(isCollidingWithObject) // && [interact] is pressed 
        //transition scene should be loaded as soon as you enter a level
        //while in the background of that the minigame is being loaded 
        SceneManager.LoadScene(miniGameSceneName);
    }
    */
}
