using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Walk")]
    public Sprite[] walkLeft;
    public float walkSpeed = 0.12f;

    [Header("Attack")]
    public Sprite[] attack;
    public float attackSpeed = 0.1f;

    private TopDownEnemy enemy;
    private Rigidbody2D rb;

    private int walkFrame = 0;
    private float walkTimer = 0f;

    private int attackFrame = 0;
    private float attackTimer = 0f;

    void Start()
    {
        enemy = GetComponent<TopDownEnemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Attack has priority
        if (enemy.IsAttacking)
        {
            PlayAttack();
            return;
        }

        // Walking
        if (rb.linearVelocity != Vector2.zero)
        {
            PlayWalk();
        }
        else
        {
            PlayIdle();
        }
    }

    // -------------------------
    // ATTACK
    // -------------------------

    void PlayAttack()
    {
        if (attack.Length == 0)
            return;

        spriteRenderer.sprite = attack[attackFrame];

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackSpeed)
        {
            attackTimer = 0f;
            attackFrame++;

            if (attackFrame >= attack.Length)
            {
                attackFrame = 0;

                // Tell enemy the animation is finished
                enemy.FinishAttack();
            }
        }
    }

    // -------------------------
    // WALK
    // -------------------------

    void PlayWalk()
    {
        if (walkLeft.Length == 0)
            return;

        walkTimer += Time.deltaTime;

        if (walkTimer >= walkSpeed)
        {
            walkTimer = 0f;
            walkFrame++;

            if (walkFrame >= walkLeft.Length)
                walkFrame = 0;
        }

        spriteRenderer.sprite = walkLeft[walkFrame];

        // Moving right → flip the left animation
        if (rb.linearVelocity.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    // -------------------------
    // IDLE
    // -------------------------

    void PlayIdle()
    {
        if (walkLeft.Length == 0)
            return;

        walkFrame = 0;
        walkTimer = 0f;

        spriteRenderer.sprite = walkLeft[0];
    }
}