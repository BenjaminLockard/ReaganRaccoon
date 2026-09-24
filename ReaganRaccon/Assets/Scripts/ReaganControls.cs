using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ReaganControls : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 25f;

    private Vector2 moveInput;
    private Vector2 cursorInput;
    private Vector3 cursorWorldPosition;
    private bool moveToCursor;

    private Vector2 currentVelocity;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector2 dashDirection;
    
    private bool dizzy;

    [Header("Crash Stun")]
    //[SerializeField] private float bounceSpeed = 8f;
    [SerializeField] private float bounceMultiplier = 0.5f;
    [SerializeField] private float dizzyDuration = 3f;

    private float dizzyTimer;

    public void OnDash(InputValue value)
    {
        if (!value.isPressed || dizzy)
            return;

        // Can't dash while already dashing or on cooldown.
        if (isDashing || dashCooldownTimer > 0f)
            return;

        // WASD takes priority.
        if (moveInput.sqrMagnitude > 0f)
        {
            dashDirection = moveInput.normalized;
        }
        // Otherwise dash toward the mouse.
        else if (moveToCursor)
        {
            Vector2 direction = (Vector2)cursorWorldPosition - (Vector2)transform.position;

            if (direction.sqrMagnitude > 0.01f)
            {
                dashDirection = direction.normalized;
            }
            else
            {
                return;
            }
        }
        else
        {
            // No movement direction available.
            return;
        }

        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        // Immediately give the player dash velocity.
        currentVelocity = dashDirection * dashSpeed;
    }

    //to allow slip-through, add to conditional w/ tags or something
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float speed = currentVelocity.magnitude;

        if (speed > (maxSpeed * 1.25) && !dizzy)
        {
            crashStun(collision);
        }
    }

    private void crashStun(Collision2D collision)
    {
        dizzy = true;
        dizzyTimer = dizzyDuration;

        // Cancel the dash.
        isDashing = false;
        dashTimer = 0f;

        // Get the player's speed at the moment of impact.
        float impactSpeed = currentVelocity.magnitude;

        // Find the direction away from the collision.
        Vector2 collisionPoint = collision.GetContact(0).point;

        Vector2 bounceDirection =
            ((Vector2)transform.position - collisionPoint).normalized;

        // Reverse the player's momentum and scale the bounce.
        currentVelocity = bounceDirection * impactSpeed * bounceMultiplier;
    }


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnCursor(InputValue value)
    {
        cursorInput = value.Get<Vector2>();
    }

    public void OnMoveToCursor(InputValue value)
    {
        if (value.isPressed)
        {
            cursorWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    cursorInput.x,
                    cursorInput.y,
                    -Camera.main.transform.position.z
                )
            );

            // Keep from changing Z axis
            cursorWorldPosition.z = transform.position.z;

            moveToCursor = true;
        }
    }

    private void Update()
    {
        // -------------------------
        // DIZZY TIMER
        // -------------------------

        if (dizzy)
        {
            dizzyTimer -= Time.deltaTime;

            if (dizzyTimer <= 0f)
            {
                dizzy = false;
            }
        }


        // -------------------------
        // DASH COOLDOWN
        // -------------------------

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }


        // -------------------------
        // DASH
        // -------------------------

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            currentVelocity = dashDirection * dashSpeed;

            transform.position += new Vector3(
                currentVelocity.x,
                currentVelocity.y,
                0f
            ) * Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }

            return;
        }


        // -------------------------
        // NORMAL MOVEMENT
        // -------------------------

        Vector2 desiredVelocity = Vector2.zero;

        if (moveInput.sqrMagnitude > 0f)
        {
            Vector2 inputDirection = moveInput.normalized;

            desiredVelocity = inputDirection * maxSpeed;

            moveToCursor = false;
        }
        else if (moveToCursor)
        {
            Vector2 direction = cursorWorldPosition - transform.position;

            if (direction.sqrMagnitude < 0.01f)
            {
                desiredVelocity = Vector2.zero;
                moveToCursor = false;
            }
            else
            {
                float distance = direction.magnitude;
                float slowdownDistance = 3.5f;

                float speedMultiplier = Mathf.Clamp01(
                    distance / slowdownDistance
                );

                desiredVelocity =
                    direction.normalized *
                    maxSpeed *
                    speedMultiplier;
            }
        }

        float accelerationRate = desiredVelocity.sqrMagnitude > 0f
            ? acceleration
            : deceleration;

        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            desiredVelocity,
            accelerationRate * Time.deltaTime
        );

        transform.position += new Vector3(
            currentVelocity.x,
            currentVelocity.y,
            0f
        ) * Time.deltaTime;
    }


    public void OnReset(InputValue input) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
