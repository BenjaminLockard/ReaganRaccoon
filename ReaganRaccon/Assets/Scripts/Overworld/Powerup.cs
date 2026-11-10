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
                break;
            // dogs ignore ----------------------------------
            case "sandwich":

                break;
            // pickup radius----------------------------------
            case "caramel":
                StartCoroutine(behaviorCaramel());
                break;
            // lose less money ----------------------------------
            case "cotton":
                StartCoroutine(reaganControls.behaviorCotton());
                break;

            // sort-lived superboost w/ crash ----------------------------------
            case "wine":
                StartCoroutine(behaviorWine());
                break;
            // money multiplier ----------------------------------
            case "coffee":
                StartCoroutine(reaganControls.behaviorCoffee());
                break;
            // move & dash speed ----------------------------------
            case "tea":
                behaviorTea();
                break;
            // extra time ----------------------------------
            case "soda":
                StartCoroutine(reaganControls.behaviorSoda());
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
