using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 2f;
    public int damage = 1;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            health.TakeDamage(damage, knockbackDirection);
        }

        Destroy(gameObject);
    }
}
