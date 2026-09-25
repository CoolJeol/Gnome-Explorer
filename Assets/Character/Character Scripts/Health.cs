using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public bool isPlayer = false;

    [Header("Respawn")]
    public bool respawnEnemy = true;
    public float respawnDelay = 10f;
    public float playerRespawnDelay = 2f;

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

    private Collider2D col;
    private Rigidbody2D rb;
    private TopDownEnemy enemy;
    private EnemyAnimation enemyAnimation;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        if (!isPlayer)
        {
            enemy = GetComponent<TopDownEnemy>();
            enemyAnimation = GetComponent<EnemyAnimation>();
        }
    }

    void Update()
    {
        // Only the player regenerates
        if (!isPlayer)
            return;

        // Don't regenerate at full health
        if (currentHealth >= maxHealth)
            return;

        timeSinceDamage += Time.deltaTime;

        // Wait until the player hasn't taken damage
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
        // -------------------------
        // PLAYER BLOCK
        // -------------------------

        if (isPlayer)
        {
            PlayerBlock block = GetComponent<PlayerBlock>();

            if (block != null && block.IsBlocking)
            {
                Debug.Log("Player blocked the attack!");

                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayPlayerBlock();

                return;
            }
        }

        // -------------------------
        // DEAL DAMAGE
        // -------------------------

        currentHealth -= damage;

        // Reset regeneration timer
        timeSinceDamage = 0f;
        regenTimer = 0f;

        Debug.Log(
            gameObject.name +
            " took " +
            damage +
            " damage. Health: " +
            currentHealth
        );

        // -------------------------
        // HURT SOUND
        // -------------------------

        if (AudioManager.Instance != null)
        {
            if (isPlayer)
                AudioManager.Instance.PlayPlayerHurt();
            else
                AudioManager.Instance.PlayEnemyHurt();
        }

        // -------------------------
        // DAMAGE BLINK
        // -------------------------

        if (spriteRenderer != null)
            StartCoroutine(DamageBlink());

        // -------------------------
        // DEATH
        // -------------------------

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageBlink()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(blinkTime);

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(blinkTime);
        }
    }

    void Die()
    {
        // -------------------------
        // PLAYER DEATH
        // -------------------------

        if (isPlayer)
        {
            StartCoroutine(PlayerRespawn());
            return;
        }

        // -------------------------
        // ENEMY DEATH
        // -------------------------

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayEnemyDeath();

        if (respawnEnemy)
        {
            StartCoroutine(EnemyRespawn());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator PlayerRespawn()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (col != null)
            col.enabled = false;

        yield return new WaitForSeconds(playerRespawnDelay);

        transform.position = startPosition;
        currentHealth = maxHealth;

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

    IEnumerator EnemyRespawn()
    {
        // Stop enemy movement
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Disable enemy AI
        if (enemy != null)
            enemy.enabled = false;

        // Disable enemy animation
        if (enemyAnimation != null)
            enemyAnimation.enabled = false;

        // Hide enemy
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        // Disable collider
        if (col != null)
            col.enabled = false;

        Debug.Log(
            gameObject.name +
            " will respawn in " +
            respawnDelay +
            " seconds."
        );

        yield return new WaitForSeconds(respawnDelay);

        // Move back to starting position
        transform.position = startPosition;

        // Restore health
        currentHealth = maxHealth;

        timeSinceDamage = 0f;
        regenTimer = 0f;

        // Reset sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        // Enable collider
        if (col != null)
            col.enabled = true;

        // Enable enemy AI
        if (enemy != null)
            enemy.enabled = true;

        // Enable enemy animation
        if (enemyAnimation != null)
            enemyAnimation.enabled = true;

        Debug.Log(gameObject.name + " respawned!");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}