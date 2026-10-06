using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ClawMinigame : MonoBehaviour
{
    
    public float speed = 40f; 
    
    private float flip = 1f;
    private bool failed = false;
    public Rigidbody2D rb;
    private bool isSpacePressed = false; 
    private LoadNext loadNext;
    void Start(){
        loadNext = GetComponent<LoadNext>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if(rb.transform.position.x >= 8.5)
        {
            flip = -1;
        }else if (rb.transform.position.x <= -8.5)//possibly condense this with an || 
        {
            flip = 1;
        }
        //this is the boundary of how far the box can go out before flipping
    }

    private void FixedUpdate() {
        //fixed updates on a constant loop
        if(isSpacePressed){
            //moves down
            if (failed)//is triggered by the collider
            {
            rb.MovePosition(rb.position + new Vector2(0f , speed) * Time.fixedDeltaTime);
            if(rb.transform.position.y >= 3)
                {
                    //claw goes up then goes back to going left and right
                    failed = false;
                    isSpacePressed = false;
                }
            }else{
                //claw goes straight down 
            rb.MovePosition(rb.position + new Vector2(0f , speed * -1) * Time.fixedDeltaTime);
            }
        }
        else
        {
            rb.MovePosition(rb.position + new Vector2(speed * flip, 0f) * Time.fixedDeltaTime);
            //goes left and right
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Prize"))
        {
            //hitting the triangle causes you to win
            //Debug.Log("WINNER");
            //loadNext.switchScene("NULL","TestSceneB");
            //Jason
            loadNext.switchScene("switchback","ClawMinigame");
            //The name of this script needs to be put in
            //then the script knows which one to destroy
        }
        else{
            //runs the return sequence where the claw goes back up and you try again
            failed = true;
        }
    }
    public void OnSpace(InputValue space){
        //Debug.Log("Press Space Pressed");
        isSpacePressed = true;
        //this causes the claw to go down and to run the next section of the code
    }
}
