using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDamage : MonoBehaviour
{
    public int maxHealth = 3;
    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.5f; // Duration for which the knockback effect lasts
    public UnityEvent onDamageTaken;
    public UnityEvent onPlayerDied;

    private int currentHealth;
    private bool isInvincible = false;
    private SpriteRenderer playerRenderer;
    private Color originalColor;
    private Rigidbody2D rb;
    private PlayerMovement movementScript;

    private void Start()
    {
        currentHealth = maxHealth;
        playerRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<PlayerMovement>();

        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D component found on the player.");
        }

        if (movementScript == null)
        {
            Debug.LogError("No Movement script found on the player.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Red") && !isInvincible)
        {
            TakeDamage(collision.transform);
        }
    }

    private void TakeDamage(Transform redObject)
    {
        currentHealth--;

        onDamageTaken.Invoke();

        if (currentHealth <= 0)
        {
            onPlayerDied.Invoke();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
            StartCoroutine(KnockbackCoroutine(redObject));
        }
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

    private IEnumerator KnockbackCoroutine(Transform redObject)
    {
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D component found on the player.");
            yield break;
        }

        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        Vector2 knockbackDirection = (transform.position - redObject.position).normalized;
        Debug.Log("Knockback direction: " + knockbackDirection);
        Vector2 knockbackForceVector = knockbackDirection * knockbackForce;
        rb.AddForce(knockbackForceVector, ForceMode2D.Impulse);
        Debug.Log("Player velocity after knockback: " + rb.velocity);

        yield return new WaitForSeconds(knockbackDuration);

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }
}
