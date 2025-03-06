using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;
    public float jumpBufferTime = 0.1f; // Delay to ensure consistent jump
    private bool isJumping = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isJumping)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                isJumping = true; // Prevents multiple triggers
                rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                Invoke(nameof(ResetJump), jumpBufferTime); // Reset jump pad buffer
            }
        }
    }

    private void ResetJump()
    {
        isJumping = false;
    }
}

