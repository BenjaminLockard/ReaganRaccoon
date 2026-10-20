using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardDog : MonoBehaviour
{
    [Header("Patrol")]
    public GameObject[] patrolPoints;
    private int currentPPIndex = 0;

    [Header("Movement")]
    public float speed = 3f;
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float aggroRange;
    public float chaseRange;

    public enum EnemyState { Patrolling, Chasing }
    public EnemyState currentState = EnemyState.Patrolling;

    public GameObject target;
    private GameObject player;

    private Rigidbody2D rb;

    // Current movement velocity.
    private Vector2 currentVelocity;


    // --------------------------------------------------
    // PLAYER RANGE
    // --------------------------------------------------

    bool PlayerInRange()
    {
        if (player == null)
        {
            Debug.LogError("Player DNE");
            return false;
        }

        if (currentState == EnemyState.Chasing) {
            return Vector2.Distance(
            transform.position,
            player.transform.position
            ) <= chaseRange;
        } else
        {
            return Vector2.Distance(
            transform.position,
            player.transform.position
            ) <= aggroRange;
        }
    }


    // --------------------------------------------------
    // STATE
    // --------------------------------------------------

    void UpdateState()
    {
        if (PlayerInRange() && currentState == EnemyState.Patrolling)
        {
            currentState = EnemyState.Chasing;
        }
        else if (!PlayerInRange() && currentState == EnemyState.Chasing)
        {
            currentState = EnemyState.Patrolling;
        }
    }


    // --------------------------------------------------
    // MOVEMENT
    // --------------------------------------------------

    void MoveTowardTarget()
    {
        Vector2 direction =
            target.transform.position - transform.position;

        if (direction.sqrMagnitude > 0.01f)
        {
            direction.Normalize();

            Vector2 desiredVelocity = direction * speed;

            // Accelerate toward the desired velocity.
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                desiredVelocity,
                acceleration * Time.deltaTime
            );
        }
        else
        {
            // If we have reached the target, decelerate.
            currentVelocity = Vector2.MoveTowards(
                currentVelocity,
                Vector2.zero,
                deceleration * Time.deltaTime
            );
        }

        rb.linearVelocity = currentVelocity;
    }


    // --------------------------------------------------
    // PATROL
    // --------------------------------------------------

    void Patrol()
    {
        if (Vector2.Distance(
                transform.position,
                target.transform.position
            ) <= 0.5f)
        {
            currentPPIndex =
                (currentPPIndex + 1) % patrolPoints.Length;
        }

        target = patrolPoints[currentPPIndex];

        MoveTowardTarget();
    }


    // --------------------------------------------------
    // CHASE
    // --------------------------------------------------

    void Chase()
    {
        target = player;

        MoveTowardTarget();
    }


    // --------------------------------------------------
    // GIZMOS
    // --------------------------------------------------

    private void OnDrawGizmos()
    {
        if (patrolPoints != null)
        {
            Gizmos.color = Color.green;

            foreach (GameObject point in patrolPoints)
            {
                Gizmos.DrawWireSphere(
                    point.transform.position,
                    0.5f
                );
            }
        }
    }


    // --------------------------------------------------
    // START
    // --------------------------------------------------

    void Start()
    {
        player = GameObject.FindWithTag("Reagan");

        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("Patrol Points DNE");
            return;
        }

        target = patrolPoints[currentPPIndex];
    }


    // --------------------------------------------------
    // UPDATE
    // --------------------------------------------------

    void Update()
    {
        UpdateState();

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;

            case EnemyState.Chasing:
                Chase();
                break;
        }

        if (target != null)
        {
            Debug.DrawLine(
                transform.position,
                target.transform.position,
                Color.red
            );
        }
    }
}