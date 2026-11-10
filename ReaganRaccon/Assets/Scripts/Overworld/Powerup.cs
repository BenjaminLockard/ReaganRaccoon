using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Powerup : MonoBehaviour
{
    public string id; //all lowercase, food name;
    public bool active = true;

    public GameObject reaganBody;
    public ReaganControls reaganControls;
    public MoneyManager moneyManager;
    public TimeManager timeManager;
    public UIManager uiManager;

    [SerializeField] private float deceleration = 10f;
    private SpriteRenderer spriteRenderer;
    private Vector2 currentVelocity;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active &&
            collision.CompareTag("Reagan") &&
            !reaganControls.iFrames)
        {
            powerupBehavior();

            active = false;
            spriteRenderer.enabled = false;
        }
    }


    public void powerupBehavior()
    {
        switch (id) {
            case "kibble":
                StartCoroutine(behaviorKibble());
                uiManager.PowerupDisplay("Kibble", "Dogs will ignore you", 15f);
                break;
            // dogs ignore ----------------------------------
            case "sandwich":

                break;
            // pickup radius----------------------------------
            case "caramel":
                StartCoroutine(behaviorCaramel());
                uiManager.PowerupDisplay("Caramel Apple", "Drop less money when hit", 30f);
                break;
            // lose less money ----------------------------------
            case "cotton":
                StartCoroutine(reaganControls.behaviorCotton());
                uiManager.PowerupDisplay("Cotton Candy", "Turbo mode", 14f);
                break;

            // sort-lived superboost w/ crash ----------------------------------
            case "wine":
                StartCoroutine(behaviorWine());
                uiManager.PowerupDisplay("Fine Wine", "Money Multiplier", 30f);
                break;
            // money multiplier ----------------------------------
            case "coffee":
                StartCoroutine(reaganControls.behaviorCoffee());
                uiManager.PowerupDisplay("Black Coffee", "Faster movement", 30f);
                break;
            // move & dash speed ----------------------------------
            case "tea":
                behaviorTea();
                uiManager.PowerupDisplay("Herbal Tea", "+15 seconds time", 5f);
                break;
            // extra time ----------------------------------
            case "soda":
                StartCoroutine(reaganControls.behaviorSoda());
                uiManager.PowerupDisplay("Grape Soda", "Slow movement, better dahses", 15f);
                break;
            // dash frequency & no dizzy but slow ----------------------------------
            case "pills":
                //int randomInt = Random.Range(1, 11);
                break;
                // random from existing ----------------------------------

        }
    }

    public IEnumerator behaviorKibble()
    {
        reaganControls.kibble = true;
        yield return new WaitForSeconds(15f);
        reaganControls.kibble = false;
    }

    public IEnumerator behaviorCaramel() { 
        reaganControls.caramel = true;
        yield return new WaitForSeconds(30f);
        reaganControls.caramel = false;
    }

    public IEnumerator behaviorWine()
    {
        moneyManager.wine = true;
        yield return new WaitForSeconds(30f);
        moneyManager.wine = false;
    }

    public void behaviorTea()
    {
        timeManager.ChangeTime(30);
    }


    //public void Initialize(
    //    MoneyManager moneyManager,
    //    ReaganControls reaganControls)
    //{
    //    this.moneyManager = moneyManager;
    //    this.reaganControls = reaganControls;
    //}

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = active;
    }

    

    private void Update()
    {
        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            Vector2.zero,
            deceleration * Time.deltaTime
        );

        transform.position += new Vector3(
            currentVelocity.x,
            currentVelocity.y,
            0f
        ) * Time.deltaTime;
    }
}
