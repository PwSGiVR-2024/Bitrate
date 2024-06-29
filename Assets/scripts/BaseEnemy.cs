using UnityEngine;
using System.Collections;

public class BaseEnemy : MonoBehaviour
{
    public int experiencePoints = 1; // Experience points this enemy is worth
    public int maxHealth = 3; // Maximum health of the enemy
    public float knockbackForce = 5f;
    public float damageCooldown = 0.2f;
    public GameObject experienceOrbPrefab; // Reference to the ExperienceOrb prefab

    private int currentHealth;
    private Rigidbody2D rb;
    private Collider2D col;
    private bool isTakingDamage = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isTakingDamage) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback(knockbackDirection));
        }
    }

    private void Die()
    {
        DropExperienceOrbs();
        Destroy(gameObject);
    }

    private void DropExperienceOrbs()
    {
        if (experienceOrbPrefab != null)
        {
            GameObject orb = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
            ExperienceOrb experienceOrb = orb.GetComponent<ExperienceOrb>();
            if (experienceOrb != null)
            {
                experienceOrb.SetExperienceValue(experiencePoints);
            }
        }
        else
        {
            Debug.LogError("ExperienceOrb prefab not assigned.");
        }
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackDirection)
    {
        isTakingDamage = true;
        rb.velocity = knockbackDirection * knockbackForce;
        col.enabled = false;
        yield return new WaitForSeconds(damageCooldown);
        col.enabled = true;
        isTakingDamage = false;
    }
}
