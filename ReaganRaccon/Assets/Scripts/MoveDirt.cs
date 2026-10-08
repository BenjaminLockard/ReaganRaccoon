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
    private Camera cam;
    //private GameObject c;
    //private Rigidbody2D rb;
    private RaycastHit2D hit;
    private Ray ray;

    private void Start()
    {
        cam = Camera.main;
       // rb = GetComponent<Rigidbody2D>();
       // c = GetComponent<GameObject>();
    }
     public void OnCursor(InputValue value)
    {
        cursorInput = value.Get<Vector2>();//location?

    }
    private void Update()
    {
        cursorWorldPosition = cam.ScreenToWorldPoint(new Vector3(cursorInput.x, cursorInput.y, 0f));
        ray = cam.ScreenPointToRay(cursorWorldPosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
    }
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void OnLeftClick(InputValue value)
    {
        //Just takes in mouse position and prints to screen.
        if (value.isPressed)
        {   
            Debug.Log("there was a click at " + cursorWorldPosition);
            
            //Ray ray = cam.ScreenPointToRay(cursorWorldPosition);

            hit = Physics2D.GetRayIntersection(ray, 20);

            //Test hit exists, otherwise you get an error.
            if(hit.collider != null && hit.collider.gameObject.CompareTag("Prize"))
            {
                Debug.Log("Got I FUCKING YOU");
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

    
}
