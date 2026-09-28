using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MoveDirt : MonoBehaviour
{
    //[SerializeField] private bool isDragging = false;
    private Vector3 cursorWorldPosition;
    private Vector2 cursorInput;
    private Vector2 RightClickInput;
    Camera cam;
    private GameObject c;
    private Rigidbody2D rb;

    private void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<GameObject>();
    }
    public void OnLeftClick(InputValue value)
    {

        //Just takes in mouse position and prints to screen.
        if (value.isPressed)
        {
            Debug.Log("there was a click at " + cursorWorldPosition);

            RaycastHit2D hit;

            Ray ray = cam.ScreenPointToRay(cursorWorldPosition);

            hit = Physics2D.GetRayIntersection(ray, 20);

            //Test hit exists, otherwise you get an error.
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Prize"))
            {
                hit.collider.gameObject.transform.position = cursorWorldPosition;
            }
        }
    }

    public void OnRightClick(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("right click");//clicks multiple times
        }
    }

    public void OnCursor(InputValue value)
    {
        cursorInput = value.Get<Vector2>();//location?
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update()
    {

        cursorWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(cursorInput.x, cursorInput.y, -Camera.main.transform.position.z));
        Ray ray = cam.ScreenPointToRay(cursorWorldPosition);

        //Remember that ScreenPointToRay doesn't draw from origin of camera, it's a straight line from the screenpoint of the mouse 'straight down the lens' into the game.

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);

        /*if (isDragging)
        {
            rb.transform.position = cursorWorldPosition;
        }
        */
    }
}
