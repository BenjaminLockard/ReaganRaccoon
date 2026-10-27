using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReaganControls : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public MoneyManager moneyManager;

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

    [Header("Hazard Handling")]
    [SerializeField] public bool iFrames = false;
    [SerializeField] private float hazardKnockbackForce = 20f;
    [SerializeField] private float iFrameDuration = 2f;
    [SerializeField] private GameObject grandPrefab;
    [SerializeField] private GameObject largePrefab;
    [SerializeField] private GameObject smallPrefab;
    [SerializeField] private float coinLaunchForceMin = 8f;
    [SerializeField] private float coinLaunchForceMax = 12f;


    private float dizzyTimer;

    Vector2 desiredVelocity = Vector2.zero;

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
        if (collision.gameObject.CompareTag("Hazard") && !iFrames)
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

        StartCoroutine(justHit());
    }

    public IEnumerator justHit()
    {
        iFrames = true;
        dropMoney();

        for (float i = iFrameDuration; i > 0f; i -= 0.15f)
        {
            SetOpacity(0.5f);
            yield return new WaitForSeconds(0.075f);
            SetOpacity(1f);
            yield return new WaitForSeconds(0.075f);
        }

        iFrames = false;
    }

    public void dropMoney()
    {
        float moneyToLose = moneyManager.getCurrentMoney() / 5f;
        if (moneyToLose > 25f)
            moneyToLose = 25f;

        moneyManager.changeMoney(moneyToLose * -1f);

        for (int i = 0; i < 4; i++)
        {
            if (moneyToLose >= 5)
            {
                GameObject grandCoin = Instantiate(
                    grandPrefab,
                    transform.position,
                    transform.rotation
                );

                MoneyFloating coin =
                    grandCoin.GetComponent<MoneyFloating>();

                // Give the coin the references it needs.
                coin.Initialize(moneyManager, this);

                // Launch the coin in a random direction.
                Vector2 randomDirection =
                    Random.insideUnitCircle.normalized;

                float launchForce = Random.Range(
                        coinLaunchForceMin,
                        coinLaunchForceMax
                );

                coin.Launch(randomDirection, launchForce);

                moneyToLose -= 5;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (moneyToLose >= 1)
            {
                GameObject largeCoin = Instantiate(
                    largePrefab,
                    transform.position,
                    transform.rotation
                );

                MoneyFloating coin =
                    largeCoin.GetComponent<MoneyFloating>();

                // Give the coin the references it needs.
                coin.Initialize(moneyManager, this);

                // Launch the coin in a random direction.
                Vector2 randomDirection =
                    Random.insideUnitCircle.normalized;

                float launchForce = Random.Range(
                        coinLaunchForceMin,
                        coinLaunchForceMax
                );

                coin.Launch(randomDirection, launchForce);

                moneyToLose -= 1;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            if (moneyToLose >= 0.25f)
            {
                GameObject smallCoin = Instantiate(
                    smallPrefab,
                    transform.position,
                    transform.rotation
                );

                MoneyFloating coin =
                    smallCoin.GetComponent<MoneyFloating>();

                // Give the coin the references it needs.
                coin.Initialize(moneyManager, this);

                // Launch the coin in a random direction.
                Vector2 randomDirection =
                    Random.insideUnitCircle.normalized;

                float launchForce = Random.Range(
                        coinLaunchForceMin,
                        coinLaunchForceMax
                );

                coin.Launch(randomDirection, launchForce);

                moneyToLose -= 0.25f;
            }
        }
    }

    //KNOWN BUG - iFrames last forever if minigame entered during iFrames
    //FIX - Call function below upon returning to main scene.
    public void resetIFrame()
    {
        iFrames = false;
        SetOpacity(1f);
    }

    public void SetOpacity(float alphaValue)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = alphaValue;
        spriteRenderer.color = currentColor;
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
            desiredVelocity.sqrMagnitude > 0.01f;

        animator.SetBool("isWalking", isWalking);

        animator.SetFloat("InputX", desiredVelocity.x);
        animator.SetFloat("InputY", desiredVelocity.y);

        // Remember the last nonzero movement direction.
        if (desiredVelocity.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = desiredVelocity.normalized;
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
        desiredVelocity = Vector2.zero;

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
    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


        // RESET SCENE

        //public void OnReset(InputValue input)
        //{
        //    SceneManager.LoadScene(
        //        SceneManager.GetActiveScene().name
        //    );
        //}
    }