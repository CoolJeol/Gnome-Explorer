using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public InputActionReference attackAction;

    public float attackRange = 1.5f;
    public int attackDamage = 25;
    public float attackCooldown = 0.5f;
    public float attackStep = 0.3f;

    private float attackTimer;

    void OnEnable()
    {
        attackAction.action.Enable();
    }

    void OnDisable()
    {
        attackAction.action.Disable();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackAction.action.WasPressedThisFrame() && attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
        }
    }

    void Attack()
    {
        // Small step forward
        transform.position += transform.up * attackStep;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange
        );

        foreach (Collider2D hit in hits)
        {
            // Don't hit yourself
            if (hit.gameObject == gameObject)
                continue;

            Vector2 direction =
                (hit.transform.position - transform.position).normalized;

            // Player's facing direction
            Vector2 forward = transform.up;

            // 180 degree attack area
            float angle = Vector2.Angle(forward, direction);

            if (angle <= 90f)
            {
                Health enemyHealth = hit.GetComponent<Health>();

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }
}