using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public static event Action OnPlayerDamaged; // The static event to subscribe to

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player has collided with something red
        if (collision.gameObject.GetComponent<SpriteRenderer>() != null &&
            collision.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
        {
            // Broadcast the damage event to all subscribers
            OnPlayerDamaged?.Invoke();
        }
    }
}