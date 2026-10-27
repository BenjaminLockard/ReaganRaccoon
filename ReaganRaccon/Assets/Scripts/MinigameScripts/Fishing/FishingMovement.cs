using UnityEngine;
using UnityEngine.InputSystem;

public class FishingMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector2 moveInput;
    public float moveSpeed; 
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private LoadNext loadNext;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        loadNext = GetComponent<LoadNext>();
    }
    private void OnMove(InputValue value){
        moveInput = value.Get<Vector2>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Hazard"))
        {
            loadNext.switchScene("FishingMinigame","switchback");
        }
    }
    // Update is called once per frame
    void Update()
    {//
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        rb.position = new Vector2(Mathf.Clamp(rb.position.x,-52.54f,-24.23f),Mathf.Clamp(rb.position.y,-4.44f,11.33f));
    }
}
