using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Health, IUpgradeable
{
    public float invincibilityDuration = 2f;
    public float flickerInterval = 0.1f;
    public Slider healthSlider;
    public UnityEvent onDamageTaken;
    public UnityEvent onPlayerDied;
    public AudioClip damageSoundClip;
    public AudioClip deathSoundClip;

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

    public void ApplyUpgrade(string upgradeType, float value)
    {
        if (upgradeType == "Health")
        {
            maxHealth += (int)value;
            currentHealth = maxHealth;
            UpdateHealthUI();
        }
    }

    public void RefillHealth()
    {
        currentHealth = maxHealth;
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
        UpdateHealthUI();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        if (shootingScript != null)
        {
            shootingScript.enabled = false;
        }

        col.enabled = false;

        if (deathSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSoundClip);
        }

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
        col.enabled = false;
        yield return new WaitForSeconds(damageCooldown);
        col.enabled = true;

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }

    private IEnumerator DeathFlickerCoroutine()
    {
        float flickerDuration = 0.05f;
        float flickerIncrease = 0.05f;
        float maxFlickerDuration = 0.5f;

        while (flickerDuration < maxFlickerDuration)
        {
            playerRenderer.color = Color.clear;
            yield return new WaitForSeconds(flickerDuration);

            playerRenderer.color = originalColor;
            yield return new WaitForSeconds(flickerDuration);

            flickerDuration += flickerIncrease;
        }

        playerRenderer.color = Color.clear;
        SceneManager.LoadScene("GameOverScreen");
    }
}
