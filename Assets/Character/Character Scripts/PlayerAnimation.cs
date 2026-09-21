using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Walking")]
    public Sprite[] walkUp;
    public Sprite[] walkDown;
    public Sprite[] walkLeft;

    [Header("Attacking")]
    public Sprite[] attackUp;
    public Sprite[] attackDown;
    public Sprite[] attackLeft;
    public float attackSpeed = 0.1f;

    [Header("Blocking")]
    public Sprite blockUp;
    public Sprite blockDown;
    public Sprite blockLeft;

    [Header("Input")]
    public InputActionReference attackAction;
    public InputActionReference blockAction;

    private PlayerFacing facing;
    private TopDownController movement;

    private int currentFrame = 0;
    private float timer = 0f;

    private bool attacking = false;
    private int attackFrame = 0;
    private float attackTimer = 0f;

    void Start()
    {
        facing = GetComponent<PlayerFacing>();
        movement = GetComponent<TopDownController>();
    }

    void OnEnable()
    {
        attackAction.action.Enable();
        blockAction.action.Enable();
    }

    void OnDisable()
    {
        attackAction.action.Disable();
        blockAction.action.Disable();
    }

    void Update()
    {
        // Start attack
        if (attackAction.action.WasPressedThisFrame() && !attacking)
        {
            attacking = true;
            attackFrame = 0;
            attackTimer = 0f;
        }

        // Attack has highest priority
        if (attacking)
        {
            PlayAttack();
            return;
        }

        // Block has second priority
        if (blockAction.action.IsPressed())
        {
            PlayBlock();
            return;
        }

        // Otherwise play walking / idle
        Vector2 move = movement.GetMovement();

        if (move != Vector2.zero)
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
        Sprite[] animation = GetAttackSprites();

        if (animation == null || animation.Length == 0)
            return;

        spriteRenderer.sprite = animation[attackFrame];

        SetFlip();

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackSpeed)
        {
            attackTimer = 0f;
            attackFrame++;

            if (attackFrame >= animation.Length)
            {
                attackFrame = 0;
                attacking = false;
            }
        }
    }

    Sprite[] GetAttackSprites()
    {
        if (facing.FacingDirection == Vector2.up)
            return attackUp;

        if (facing.FacingDirection == Vector2.down)
            return attackDown;

        if (facing.FacingDirection == Vector2.left)
            return attackLeft;

        return attackLeft;
    }

    // -------------------------
    // BLOCK
    // -------------------------

    void PlayBlock()
    {
        if (facing.FacingDirection == Vector2.up)
        {
            spriteRenderer.sprite = blockUp;
        }
        else if (facing.FacingDirection == Vector2.down)
        {
            spriteRenderer.sprite = blockDown;
        }
        else if (facing.FacingDirection == Vector2.left)
        {
            spriteRenderer.sprite = blockLeft;
        }
        else
        {
            spriteRenderer.sprite = blockLeft;
        }

        SetFlip();
    }

    // -------------------------
    // WALK
    // -------------------------

    void PlayWalk()
    {
        timer += Time.deltaTime;

        if (timer >= 0.12f)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= 4)
                currentFrame = 0;
        }

        if (facing.FacingDirection == Vector2.up)
        {
            spriteRenderer.sprite = walkUp[currentFrame];
        }
        else if (facing.FacingDirection == Vector2.down)
        {
            spriteRenderer.sprite = walkDown[currentFrame];
        }
        else
        {
            spriteRenderer.sprite = walkLeft[currentFrame];
        }

        SetFlip();
    }

    // -------------------------
    // IDLE
    // -------------------------

    void PlayIdle()
    {
        currentFrame = 0;
        timer = 0f;

        if (facing.FacingDirection == Vector2.up)
        {
            spriteRenderer.sprite = walkUp[0];
        }
        else if (facing.FacingDirection == Vector2.down)
        {
            spriteRenderer.sprite = walkDown[0];
        }
        else
        {
            spriteRenderer.sprite = walkLeft[0];
        }

        SetFlip();
    }

    // -------------------------
    // FLIP LEFT → RIGHT
    // -------------------------

    void SetFlip()
    {
        spriteRenderer.flipX =
            facing.FacingDirection == Vector2.right;
    }
}