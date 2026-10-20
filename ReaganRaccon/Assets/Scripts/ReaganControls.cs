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

    [Header("Animation")]
    private Animator animator;
    private Vector2 lastMoveDirection = Vector2.down;

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
    [SerializeField] private float bounceMultiplier = 0.5f;
    [SerializeField] private float dizzyDuration = 3f;
    [SerializeField] private float crashThreshold = 1.25f;
    [SerializeField] private float hazardKnockbackForce = 30f;

    private float dizzyTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // DASH

    public void OnDash(InputValue value)
    {
        if (!value.isPressed || dizzy)
            return;

        if (isDashing || dashCooldownTimer > 0f)
            return;

        if (moveInput.sqrMagnitude > 0f)
        {
            dashDirection = moveInput.normalized;
        }
        else if (moveToCursor)
        {
            Vector2 direction =
                (Vector2)cursorWorldPosition -
                (Vector2)transform.position;

            if (direction.sqrMagnitude > 0.01f)
                dashDirection = direction.normalized;
            else
                return;
        }
        else
        {
            return;
        }

        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        currentVelocity = dashDirection * dashSpeed;
    }

    // COLLISION / CRASH STUN

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
            hazardKnockback(collision);
        else
        {
            crashStun(collision);
        }
    }

    private void crashStun(Collision2D collision)
    {
        float speed = currentVelocity.magnitude;

        if (speed > (maxSpeed * crashThreshold) && !dizzy)
        {
            dizzy = true;
            dizzyTimer = dizzyDuration;
        }

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
        currentVelocity =
            bounceDirection * impactSpeed * bounceMultiplier;
    }

    public void hazardKnockback(Collision2D collision)
    {
        //dizzy = true;
        //dizzyTimer = dizzyDuration;

        //isDashing = false;
        //dashTimer = 0f;

        Vector2 collisionPoint = collision.GetContact(0).point;

        Vector2 launchDirection =
            ((Vector2)transform.position - collisionPoint).normalized;

        // Reverse the player's momentum and scale the bounce.
        currentVelocity =
            launchDirection * hazardKnockbackForce;
    }


    // RESET MOVEMENT

    public void ResetMovement()
    {
        moveInput = Vector2.zero;
        cursorInput = Vector2.zero;
        moveToCursor = false;
    }

    // INPUT

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
        // Mouse movement is handled in Update.
    }

    // ANIMATION

    private void UpdateAnimation()
    {
        if (animator == null)
            return;

        bool isWalking =
            moveInput.sqrMagnitude > 0.01f ||
            currentVelocity.sqrMagnitude > 0.01f;

        animator.SetBool("isWalking", isWalking);

        animator.SetFloat("InputX", currentVelocity.x);
        animator.SetFloat("InputY", currentVelocity.y);

        // Remember the last nonzero movement direction.
        if (currentVelocity.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = currentVelocity.normalized;
        }

        if (!isWalking)
        {
            animator.SetFloat("LastInputX", lastMoveDirection.x);
            animator.SetFloat("LastInputY", lastMoveDirection.y);
        }
    }

    // UPDATE

    private void Update()
    {
        moveToCursor =
            Mouse.current != null &&
            Mouse.current.leftButton.isPressed &&
            moveInput.sqrMagnitude == 0f;

        // Update mouse world position.
        if (moveToCursor && Camera.main != null)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            cursorWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

            cursorWorldPosition.z = transform.position.z;
        }

        // Dizzy timer.
        if (dizzy)
        {
            dizzyTimer -= Time.deltaTime;

            if (dizzyTimer <= 0f)
                dizzy = false;
        }

        // Dash cooldown.
        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        // Active dash.
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            currentVelocity = dashDirection * dashSpeed;

            transform.position += new Vector3(
                currentVelocity.x,
                currentVelocity.y,
                0f
            ) * Time.deltaTime;

            UpdateAnimation();

            if (dashTimer <= 0f)
                isDashing = false;

            return;
        }

        // Normal movement.
        Vector2 desiredVelocity = Vector2.zero;

        // WASD movement.
        if (moveInput.sqrMagnitude > 0f)
        {
            Vector2 inputDirection = moveInput.normalized;

            desiredVelocity = inputDirection * maxSpeed;

            moveToCursor = false;
        }
        // Mouse movement.
        else if (moveToCursor)
        {
            Vector2 direction =
                (Vector2)cursorWorldPosition -
                (Vector2)transform.position;

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

        // Acceleration / deceleration.
        float accelerationRate =
            desiredVelocity.sqrMagnitude > 0f
                ? acceleration
                : deceleration;

        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            desiredVelocity,
            accelerationRate * Time.deltaTime
        );

        // Move player.
        transform.position += new Vector3(
            currentVelocity.x,
            currentVelocity.y,
            0f
        ) * Time.deltaTime;

        UpdateAnimation();
    }

    // RESET SCENE

    public void OnReset(InputValue input)
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}