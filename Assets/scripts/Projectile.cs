using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 2f;
    public float damage = 1f; // Initial damage

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BaseEnemy BaseEnemy = collision.gameObject.GetComponent<BaseEnemy>();
        if (BaseEnemy != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            BaseEnemy.TakeDamage((int)damage, knockbackDirection);
        }

        Destroy(gameObject);
    }
}
