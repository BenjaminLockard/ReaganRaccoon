
using UnityEngine;

public class baseballscript : MonoBehaviour
{
    private Rigidbody2D rb;
    private LoadNext loadNext;

    private Vector2 force;
    private bool gameEnded = false;

    void Start()
    {
        loadNext = GetComponent<LoadNext>();
        rb = GetComponent<Rigidbody2D>();

        Invoke("throwBall", Random.Range(5f, 10f));

        force = new Vector2(
            Random.Range(-20f, -10f),
            0f
        );
    }

    private void throwBall()
    {
        if (gameEnded)
            return;

        if (rb != null)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded)
            return;

        bool won = other.CompareTag("Win");
        bool lost = other.CompareTag("Lose");

        if (!won && !lost)
            return;

        gameEnded = true;

        RewardManager rewardManager =
            FindFirstObjectByType<RewardManager>();

        if (rewardManager != null)
        {
            if (won)
            {
                rewardManager.ShowRewardWindow();
            }
            else
            {
                rewardManager.ShowLoseWindow();
            }
        }
        else
        {
            Debug.LogWarning(
                "Baseball: RewardManager not found!"
            );
        }

        if (loadNext != null)
        {
            loadNext.switchScene(
                "BaseBallMinigame",
                "switchback"
            );
        }
        else
        {
            Debug.LogError(
                "Baseball: LoadNext component missing!"
            );
        }
    }
}

