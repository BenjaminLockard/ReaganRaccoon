using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class interaction : MonoBehaviour

{
    public ReaganControls playerControls;
    public LoadNext loadNext;
    private bool canInteract = false;
    private bool isCollidingWithObject;
    public string SceneName;
    public string type;
    public GameObject removed;
    private float transitionTime = 1.7f; 
    public Animator transition;
    public Animator stopMoving;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public AudioClip minigameSound;
    public AudioSource audioSource;
    
    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        loadNext = GetComponent<LoadNext>();
    }

    void OnTriggerStay2D(Collider2D other){
            if(other.gameObject.CompareTag("Reagan")){
                canInteract = true;
                //Debug.Log("Can Interact");
            }
            else{
                canInteract = false;
            }
    }
    
    void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.CompareTag("Reagan")){
            canInteract = false;
            //Debug.Log("Can't Interact");
        }
    }
    
    
    void OnInteract(InputValue value){
        if(canInteract){
            //pause
            //Debug.Log("Loading Game");
            //IFRAME BUGFIX

            audioSource.PlayOneShot(minigameSound, 0.8f);
            transition.SetTrigger("tvSwitchScenes");
            playerControls.resetIFrame();
            Invoke("buffer", 2.50f);
           
            
        }
    }

    public void buffer()
    {
        loadNext.switchScene(type, SceneName);
    }

}
