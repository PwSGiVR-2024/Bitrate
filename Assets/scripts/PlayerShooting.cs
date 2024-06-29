using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Reference to the fire point
    public float projectileSpeed = 10f; // Speed of the projectile
    public float rateOfFire = 0.5f; // Rate of fire in seconds
    public AudioClip shootingSoundClip; // Reference to the shooting sound clip

    private float nextFireTime = 0f;
    private float currentDamage = 1f; // Initial damage

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + rateOfFire;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = -firePoint.right * projectileSpeed; // Reverse the direction to match the gun's facing

        // Adjust the size of the projectile based on damage
        float sizeMultiplier = Mathf.Sqrt(currentDamage);
        projectile.transform.localScale *= sizeMultiplier;

        // Set the damage of the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = currentDamage;
        }

        // Play shooting sound effect
        PlayShootingSound();
    }

    void PlayShootingSound()
    {
        // Create a new GameObject to hold the AudioSource
        GameObject soundGameObject = new GameObject("ShootingSound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.clip = shootingSoundClip;
        audioSource.Play();

        // Destroy the GameObject after the clip has finished playing
        Destroy(soundGameObject, shootingSoundClip.length);
    }

    public void SetDamage(float newDamage)
    {
        currentDamage = newDamage;
    }
}
