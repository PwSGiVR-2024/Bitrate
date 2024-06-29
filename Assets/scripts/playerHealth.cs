using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;
    public Slider healthSlider;
    public UnityEvent onDamageTaken;
    public UnityEvent onPlayerDied;
    public AudioClip damageSoundClip; // Reference to the damage sound effect
    public AudioClip deathSoundClip; // Reference to the death sound effect

    private bool isInvincible = false;
    private SpriteRenderer playerRenderer;
    private Color originalColor;
    private PlayerMovement movementScript;
    private PlayerShooting shootingScript;
    private AudioSource audioSource;


    protected override void Start()
    {
        base.Start();
        playerRenderer = GetComponent<SpriteRenderer>();
        originalColor = playerRenderer.color;
        movementScript = GetComponent<PlayerMovement>();
        shootingScript = GetComponent<PlayerShooting>();
        audioSource = GetComponent<AudioSource>();
        UpdateHealthUI();
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
        UpdateHealthUI();
        onDamageTaken.Invoke();

        // Play damage sound effect
        if (damageSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(damageSoundClip);
        }

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
    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
    protected override void OnZeroHealth()
    {
        //Update health UI
        UpdateHealthUI();

        // Disable player controls
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        if (shootingScript != null)
        {
            shootingScript.enabled = false;
        }

        // Disable collider to prevent future collisions
        col.enabled = false;

        // Play death sound effect
        if (deathSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSoundClip);
        }

        // Start the death flicker coroutine
        StartCoroutine(DeathFlickerCoroutine());
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

    private IEnumerator DeathFlickerCoroutine()
    {
        float flickerDuration = 0.05f; // Starting flicker duration
        float flickerIncrease = 0.05f; // Increase in flicker duration each cycle
        float maxFlickerDuration = 0.5f; // Maximum flicker duration before the player becomes invisible

        while (flickerDuration < maxFlickerDuration)
        {
            playerRenderer.color = Color.clear;
            yield return new WaitForSeconds(flickerDuration);

            playerRenderer.color = originalColor;
            yield return new WaitForSeconds(flickerDuration);

            flickerDuration += flickerIncrease;
        }

        playerRenderer.color = Color.clear;

        SceneManager.LoadScene("GameOverScreen"); // Change to your Game Over scene
    }
}
