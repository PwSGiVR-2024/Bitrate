using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePointRight; // Reference to the right fire point
    public Transform firePointLeft; // Reference to the left fire point
    public float projectileSpeed = 10f; // Speed of the projectile

    void Update()
    {
        // Check for input to shoot
        if (Input.GetButtonDown("Fire1")) // Default is left mouse button or Ctrl
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Calculate direction towards the mouse position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;
        Vector2 direction = (mousePosition - playerPosition).normalized;

        // Determine which fire point to use based on the mouse position relative to the player
        Transform chosenFirePoint = mousePosition.x > playerPosition.x ? firePointRight : firePointLeft;

        // Instantiate the projectile at the chosen fire point
        GameObject projectile = Instantiate(projectilePrefab, chosenFirePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Set the velocity of the projectile
        rb.velocity = direction * projectileSpeed;

        // Rotate the projectile to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
