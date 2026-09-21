using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public bool isPlayer = false;

    [Header("Respawn")]
    public float respawnDelay = 2f;

    [Header("Damage Blink")]
    public float blinkTime = 0.1f;
    public int blinkCount = 2;

    [Header("Passive Regeneration")]
    public float regenDelay = 5f;
    public float regenInterval = 1f;
    public int regenAmount = 5;

    private int currentHealth;
    private Vector3 startPosition;

    private float timeSinceDamage;
    private float regenTimer;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Only the player regenerates
        if (!isPlayer)
            return;

        // Already at full health
        if (currentHealth >= maxHealth)
            return;

        timeSinceDamage += Time.deltaTime;

        // Wait until player has not been hurt for 5 seconds
        if (timeSinceDamage >= regenDelay)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                regenTimer = 0f;
                currentHealth += regenAmount;

                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;

                Debug.Log("Player regenerated! Health: " + currentHealth);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        // Check if player is blocking
        if (isPlayer)
        {
            PlayerBlock block = GetComponent<PlayerBlock>();

            if (block != null && block.IsBlocking)
            {
                Debug.Log("Player blocked the attack!");
                return;
            }
        }

        currentHealth -= damage;

        // Reset regeneration timer
        timeSinceDamage = 0f;
        regenTimer = 0f;

        Debug.Log(gameObject.name + " took " + damage +
                  " damage. Health: " + currentHealth);

        if (spriteRenderer != null)
            StartCoroutine(DamageBlink());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageBlink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkTime);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkTime);
        }
    }

    void Die()
    {
        if (isPlayer)
        {
            StartCoroutine(Respawn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Respawn()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        transform.position = startPosition;
        currentHealth = maxHealth;

        // Reset regeneration
        timeSinceDamage = 0f;
        regenTimer = 0f;

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        if (col != null)
            col.enabled = true;

        Debug.Log("Player respawned!");
    }
}