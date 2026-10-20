using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed; 
    private Vector2 currentVelocity;
    private Rigidbody2D rb; 
    private Vector2 moveInput;
    public float jumpStrength = 10f;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform startingPoint; 
    
    public float coyoteTime = 0.05f;
    private float coyoteTimeCounter;

    void Start(){
    rb = GetComponent<Rigidbody2D>();

    }

   //moves the player left and right
    private void OnMove(InputValue value){
         moveInput = value.Get<Vector2>();
    }

    void Update(){
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        if(transform.position.y <= -8){
            transform.position = startingPoint.transform.position;
        }
        if(isGrounded){
            coyoteTimeCounter = coyoteTime;
        }
        else coyoteTimeCounter -= Time.deltaTime;
    }
 

    //if space is pressed Jump if the player is standing on something
    private void OnJump(InputValue value){
        if(coyoteTimeCounter > 0f && value.isPressed){
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpStrength);
            coyoteTimeCounter = 0f;
        }
    }

    private void update(){
        transform.position += new Vector3(currentVelocity.x, currentVelocity.y, 0f) * Time.deltaTime;
    }

    private void FixedUpdate(){
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
