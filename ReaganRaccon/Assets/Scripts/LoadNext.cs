using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNext : MonoBehaviour
{
    //if object tag has "tag" minigame and pressing action causes the new scene to load

    private bool isCollidingWithObject;
    public string miniGameSceneName;
    private void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Collision");
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("WE HIT EACH OTTER");
        if(collision.gameObject.CompareTag("Regan")){
            Debug.Log("WE HIT EACH OTTER");
            isCollidingWithObject = true;
            SceneManager.LoadScene("SampleScene");
        }
        else{
            isCollidingWithObject = false;
        }
        //sets that regan is colliding with the object
        //allows the player to press the interaction button
        //within controls
    }

    private void Update(){
        if(isCollidingWithObject) // && [interact] is pressed 
        //transition scene should be loaded as soon as you enter a level
        //while in the background of that the minigame is being loaded 
        SceneManager.LoadScene(miniGameSceneName);
    }
}
