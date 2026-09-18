using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public bool isPlayer = false;

    public float respawnDelay = 2f;

    public float blinkTime = 0.1f;
    public int blinkCount = 2;

    private int currentHealth;
    private Vector3 startPosition;

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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
        // Stop the player from moving while dead
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Hide player
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Disable collider
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        // Reset position and health
        transform.position = startPosition;
        currentHealth = maxHealth;

        // Show player
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        // Enable collider
        if (col != null)
            col.enabled = true;

        Debug.Log("Player respawned!");
    }
}