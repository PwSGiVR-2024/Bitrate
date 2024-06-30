using UnityEngine;

public class PlayerShooting : MonoBehaviour, IUpgradeable
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float rateOfFire = 0.5f;
    public AudioClip shootingSoundClip;

    private float nextFireTime = 0f;
    private float currentDamage = 1f;

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
        rb.velocity = -firePoint.right * projectileSpeed;

        float sizeMultiplier = Mathf.Sqrt(currentDamage);
        projectile.transform.localScale *= sizeMultiplier;

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.damage = currentDamage;
        }

        PlayShootingSound();
    }

    void PlayShootingSound()
    {
        GameObject soundGameObject = new GameObject("ShootingSound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = shootingSoundClip;
        audioSource.Play();
        Destroy(soundGameObject, shootingSoundClip.length);
    }

    public void SetDamage(float newDamage)
    {
        currentDamage = newDamage;
    }

    public void ApplyUpgrade(string upgradeType, float value)
    {
        if (upgradeType == "RateOfFire")
        {
            rateOfFire *= value;
        }
    }
}
