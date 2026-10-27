using UnityEngine;
using UnityEngine.InputSystem;

public class Swing : MonoBehaviour
{   
    public float timer = 0f;
    public float Recovery = 0.9f;
    public Animator transition;
    
    void Update(){
        if(timer > 0f){ //if the timer is at 0 then you can swing again
            //Debug.Log("Time: " + timer);
            timer -= Time.deltaTime;
        }
        else
        {
            Debug.Log("HOLD ON");
        }
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnSpace(InputValue space){
        if(space.isPressed && timer <= 0){
            //if space is press and the timer is at 0 then you can swing
            //else it does nothing
            timer = Recovery;
            transition.SetTrigger("isSwing");
            Debug.Log("SWING");
        } 
        else{
            Debug.Log("SWING AND A MISS");
        }
    }
}
