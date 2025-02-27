using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    private Timer timer;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();

    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
        }

        player.transform.position = respawnPoint.transform.position;

        if (timer != null)
        {
            timer.ResetTimer();
        }
    }
}
