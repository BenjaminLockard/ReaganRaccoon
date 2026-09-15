
using UnityEngine;
using UnityEngine.InputSystem;

public class ReaganControls : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 moveInput;
    private Vector2 cursorInput;
    private Vector3 cursorWorldPosition;
    private bool moveToCursor;

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
        // Take a snapshot of mouse position
        if (value.isPressed)
        {
            cursorWorldPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(cursorInput.x, cursorInput.y,
                    -Camera.main.transform.position.z));

            //keep from changing z axis
            cursorWorldPosition.z = transform.position.z;

            moveToCursor = true;
        }
    }

    private void Update()
    {
        Vector3 direction = Vector3.zero;

        if (moveInput.sqrMagnitude > 0f)
        {
            direction = new Vector3(moveInput.x, moveInput.y, 0f);

            moveToCursor = false;
        }

        else if (moveToCursor)
        {
            direction = cursorWorldPosition - transform.position;

            if (direction.sqrMagnitude < 0.01f)
            {
                direction = Vector3.zero;
                moveToCursor = false;
            }
        }

        // Prevent diagonal movement from being too fast
        if (direction.sqrMagnitude > 1f)
            direction.Normalize();

        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
