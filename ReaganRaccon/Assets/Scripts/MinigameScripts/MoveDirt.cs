using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveDirt : MonoBehaviour
{
    [SerializeField] private bool isDragging = false;
    private Vector3 cursorWorldPosition;
    private Vector2 cursorInput;
    
   private Rigidbody2D rb;
   private void Start() {
    rb = GetComponent<Rigidbody2D>();
   }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update() {
        if (isDragging)
        {
            rb.transform.position = GetMousePositionInWorldSpace();
        }
    }
    
    private void OnMouseDrag(InputValue value) {
        if(value.isPressed){
        rb.transform.position = GetMousePositionInWorldSpace();
        }
    }
    private void OnMouseDown(InputValue value) {
        if (value.isPressed)
        {
            isDragging = true;
        }
    }
    private void OnMouseUp()
    {
        isDragging = false;
    }
    
    public Vector3 GetMousePositionInWorldSpace()
    {
       // Vector2 p = Camera.main.ScreenToWorldPoint(mousePosition);
        cursorWorldPosition = 
        Camera.main.ScreenToWorldPoint(
        new Vector3(
        cursorInput.x, cursorInput.y, -Camera.main.transform.position.z)
        );
        return cursorWorldPosition;
    }
}
