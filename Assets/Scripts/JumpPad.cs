using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the jump pad!"); // Check if anything enters

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected!"); // Check if the player is detected

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Debug.Log("Rigidbody found! Applying force."); // Check if Rigidbody is found

                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reset vertical velocity
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("No Rigidbody found on Player!");
            }
        }
        else
        {
            Debug.Log("Non-player object detected.");
        }
    }
}
