using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3.5f;

    [Header("Combat")]
    public float attackCooldown = 1.0f;

    private Transform playerTarget;
    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool playerInAttackRange = false;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle attack cooldown
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Try to attack
        if (playerInAttackRange && cooldownTimer <= 0)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && !isAttacking)
        {
            // 1. Calculate direction to the player
            Vector2 direction = ((Vector2)playerTarget.position - rb.position).normalized;

            // 2. Move towards the player
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

            // 3. Rotate the entire enemy to face the player
            // Assumes your visual indicator points along the positive Y-axis (Up).
            // Change 'Vector2.up' to 'Vector2.right' if your indicator points Right.
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Smoothly rotate toward the target angle
            float smoothedAngle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, 360f * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedAngle);
        }
    }

    // --- Core Logic Called By Trigger Scripts ---

    public void SetPlayerTarget(Transform player)
    {
        playerTarget = player;
    }

    public void SetPlayerInAttackRange(bool inRange)
    {
        playerInAttackRange = inRange;
    }

    private void Attack()
    {
        cooldownTimer = attackCooldown;
        Debug.Log("Enemy attacks the player!");
    }
}
