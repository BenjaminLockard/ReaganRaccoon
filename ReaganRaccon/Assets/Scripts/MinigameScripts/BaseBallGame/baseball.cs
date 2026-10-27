using UnityEngine;

public class baseballscript : MonoBehaviour
{
    private Rigidbody2D rb;
    private LoadNext loadNext;
    private Vector2 force;
    //public int x;
    //private int y = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadNext = GetComponent<LoadNext>();
        rb = GetComponent<Rigidbody2D>();
        Invoke("throwBall", Random.Range(5,10));
        force = new Vector2(Random.Range(-20,-10), 0);
    }

    // Update is called once per frame
    private void throwBall()
    {
        rb.AddForce(force,ForceMode2D.Impulse);//change Vector 2     
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Win")){
            //trigger win item
            //load scene
            Debug.Log("YOU WIN!!");
            loadNext.switchScene("BaseBallMinigame","switchback");
        }else if(other.gameObject.CompareTag("Lose")){
            //trigger lose 
            //load scene
            loadNext.switchScene("BaseBallMinigame","switchback");
            Debug.Log("FUCK YOU FUCK YOU AHHHHHH FUCK YOU");
        }

    }
}
