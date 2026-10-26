using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyFloating : MonoBehaviour
{
    public float value;
    public bool active = true;

    public MoneyManager moneyManager;
    SpriteRenderer spriteRenderer;
    public ReaganControls playerControls;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && collision.gameObject.tag == "Reagan" && !playerControls.iFrames)
        {
            moneyManager.changeMoney(value);
            active = false;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = active;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
