using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNext : MonoBehaviour
{
    //if object tag has "tag" minigame and pressing action causes the new scene to load

    private bool isCollidingWithObject;
    public string SceneName;
    public string type;
/*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("WE HIT EACH OTTER");
        if (collision.gameObject.CompareTag("Reagan"))
        {
            Debug.Log("WE HIT EACH OTTER");
            isCollidingWithObject = true;
            if (type == "minigame")
            {
                SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
            }
            else
            {
                SceneName.LoadScene(SceneName);
            }
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
    }
*/
    public void switchScene(string type, string SceneName)
    {
        if (type == "minigame")
        {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadScene(SceneName);
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
