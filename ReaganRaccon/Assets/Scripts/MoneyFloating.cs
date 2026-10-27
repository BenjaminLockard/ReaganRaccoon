using UnityEngine;

public class MoneyFloating : MonoBehaviour
{
    public float value;
    public bool active = true;

    public MoneyManager moneyManager;
    public ReaganControls playerControls;

    [SerializeField] private float deceleration = 15f;

    private SpriteRenderer spriteRenderer;
    private Vector2 currentVelocity;

    public void Initialize(
        MoneyManager moneyManager,
        ReaganControls playerControls)
    {
        this.moneyManager = moneyManager;
        this.playerControls = playerControls;
    }

    public void Launch(Vector2 direction, float force)
    {
        currentVelocity = direction.normalized * force;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active &&
            collision.CompareTag("Reagan") &&
            !playerControls.iFrames)
        {
            moneyManager.changeMoney(value);

            active = false;
            spriteRenderer.enabled = false;
        }
    }

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