using UnityEngine;
using UnityEngine.InputSystem; 
public class ReganInteract
{
    
    private LoadNext loadNext;
    private bool canInteract = false;

    void OnTriggerStay2D(Collider2D other){
            if(other.gameObject.CompareTag("Reagan")){
                canInteract = true;
            }
            else{
                canInteract = false;
            }
    }
    
    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Reagan")){
                canInteract = false;
        }
    }
    
    
    void OnInteract(InputValue value){
        if(canInteract){
            
        }
    }
}
