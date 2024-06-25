using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 3;
    protected int currentHealth;
    public float knockbackForce = 5f;
    public float damageCooldown = 0.2f;

    protected Rigidbody2D rb;
    protected Collider2D col;
    private bool isTakingDamage = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isTakingDamage) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnZeroHealth();
        }
        else
        {
            StartCoroutine(ApplyKnockback(knockbackDirection));
        }
    }

    protected virtual void OnZeroHealth()
    {
        // Placeholder for additional behaviors upon destruction
        // Override this method in derived classes for specific behaviors
        Destroy(gameObject);
    }

    protected virtual IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        isTakingDamage = true;
        rb.velocity = knockbackDirection * knockbackForce;
        col.enabled = false; // Disable collider to prevent further collisions
        yield return new WaitForSeconds(damageCooldown);
        col.enabled = true;
        isTakingDamage = false;
    }
}
