using UnityEngine;

public class TopDownEnemy : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float attackRange = 1.2f;
    public float moveSpeed = 2f;

    public int damage = 10;
    public float attackCooldown = 1f;

    public bool IsAttacking { get; private set; }

    private Transform player;
    private Rigidbody2D rb;
    private float attackTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void Update()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        attackTimer -= Time.deltaTime;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // Outside detection range
        if (distance > detectionRadius)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Currently attacking
        if (IsAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // In attack range
        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }

            return;
        }

        // Chase player
        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    void Attack()
    {
        IsAttacking = true;

        Debug.Log("ENEMY ATTACK!");

        Health playerHealth = player.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("Player has no Health component!");
        }
    }

    public void FinishAttack()
    {
        IsAttacking = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRadius
        );

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}