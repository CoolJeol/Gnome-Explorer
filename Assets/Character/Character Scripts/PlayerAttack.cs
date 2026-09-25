using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public InputActionReference attackAction;

    public float attackRange = 1.5f;
    public int attackDamage = 25;
    public float attackCooldown = 0.5f;

    private float attackTimer;
    private PlayerFacing facing;


    void Start()
    {
        facing = GetComponent<PlayerFacing>();
    }

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
            AudioManager.Instance.PlayPlayerAttack();
        }
    }

    void Attack()
    {
        Vector2 attackDirection = facing.FacingDirection;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Vector2 direction =
                (hit.transform.position - transform.position).normalized;

            float angle = Vector2.Angle(attackDirection, direction);

            // 180 degree attack area
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
}