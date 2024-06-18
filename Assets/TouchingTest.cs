using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingTest : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("A collission has been detected");
        Destroy(collision.gameObject);
    }
}