using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RandomLocationHorizontal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.transform.position = new Vector2(rb.transform.position.x + Random.Range(-7,7), rb.transform.position.y);
    }


}
