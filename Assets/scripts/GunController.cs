using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform firePoint; // Reference to the fire point
    public float distanceFromPlayer = 1f; // Distance of the gun from the player

    private Transform player;

    void Start()
    {
        player = transform.parent; // Assume the gun is a child of the player
    }

    void Update()
    {
        RotateAndPositionGun();
    }

    void RotateAndPositionGun()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)player.position).normalized;

        // Calculate the new position of the gun
        Vector2 gunPosition = (Vector2)player.position + direction * distanceFromPlayer;
        transform.position = gunPosition;

        // Rotate the gun to face the mouse pointer, then adjust by 180 degrees to face away
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180f));
    }
}
