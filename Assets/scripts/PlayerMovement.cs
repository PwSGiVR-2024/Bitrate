using UnityEngine;

public class PlayerMovement : MonoBehaviour, IUpgradeable
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void ApplyUpgrade(string upgradeType, float value)
    {
        if (upgradeType == "Speed")
        {
            moveSpeed *= value;
        }
    }
}
