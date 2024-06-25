using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : Health
{
    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;
    public UnityEvent onDamageTaken;
    public UnityEvent onPlayerDied;

    private bool isInvincible = false;
    private SpriteRenderer playerRenderer;
    private Color originalColor;
    private PlayerMovement movementScript;

    protected override void Start()
    {
        base.Start();
        playerRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
        movementScript = GetComponent<PlayerMovement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null && !isInvincible)
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            TakeDamage(damageDealer.damageAmount, knockbackDirection);
        }
    }

    public override void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvincible) return;

        currentHealth -= damage;
        onDamageTaken.Invoke();

        if (currentHealth <= 0)
        {
            onPlayerDied.Invoke();
            OnZeroHealth();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
            StartCoroutine(ApplyKnockback(knockbackDirection));
        }
    }

    protected override void OnZeroHealth()
    {
        // Add additional player-specific behaviors here
        Destroy(gameObject);
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < invincibilityDuration)
        {
            playerRenderer.color = playerRenderer.color == originalColor ? Color.clear : originalColor;
            elapsedTime += flickerInterval;
            yield return new WaitForSeconds(flickerInterval);
        }

        playerRenderer.color = originalColor;
        isInvincible = false;
    }

    protected override IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        rb.velocity = knockbackDirection * knockbackForce;
        col.enabled = false; // Disable collider to prevent further collisions
        yield return new WaitForSeconds(damageCooldown);
        col.enabled = true;

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }
}
