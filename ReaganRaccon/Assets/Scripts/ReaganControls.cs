using UnityEngine;
using UnityEngine.InputSystem;

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
        Vector2 desiredVelocity = Vector2.zero;

        // WASD movement
        if (moveInput.sqrMagnitude > 0f)
        {
            Vector2 inputDirection = moveInput.normalized;

            desiredVelocity = inputDirection * maxSpeed;

            // Manual input takes priority over cursor movement
            moveToCursor = false;
        }

        // Mouse movement
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

                float slowdownDistance = 4f;

                float speedMultiplier = Mathf.Clamp01(
                    distance / slowdownDistance
                );

                desiredVelocity = direction.normalized * maxSpeed * speedMultiplier;
            }
        }

        // Accelerate toward desired velocity
        float accelerationRate = desiredVelocity.sqrMagnitude > 0f
            ? acceleration
            : deceleration;

        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            desiredVelocity,
            accelerationRate * Time.deltaTime
        );

        // Move the character
        transform.position += new Vector3(
            currentVelocity.x,
            currentVelocity.y,
            0f
        ) * Time.deltaTime;
    }
}