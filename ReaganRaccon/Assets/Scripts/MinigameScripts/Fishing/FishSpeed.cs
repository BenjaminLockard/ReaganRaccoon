using UnityEngine;

public class FishSpeed : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public bool flip = false;
    private LoadNext loadNext;
    public float speedMultiplier;
    private float timer;
    public float killTimer;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadNext = GetComponent<LoadNext>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {   
        killTimer += Time.deltaTime;
        timer += Time.deltaTime;
        speedMultiplier += Time.deltaTime * 0.1f;
        speed = Random.Range(5f,10f);
        
        if(flip){
        rb.linearVelocity = Vector2.left * (speed + speedMultiplier);
        }
        else
        {
        rb.linearVelocity = Vector2.right * (speed + speedMultiplier);
        }
        if(killTimer > 6 )
        {
            Destroy(gameObject);
        }
    }
}
