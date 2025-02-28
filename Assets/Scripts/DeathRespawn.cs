using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI buttons

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private Button resetUIButton; // Assign in Inspector

    private Timer timer;
    private DeathCounter deathCounter;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        deathCounter = FindObjectOfType<DeathCounter>();

        // Ensure button is linked (if assigned)
        if (resetUIButton != null)
        {
            resetUIButton.onClick.AddListener(resetButton);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPlayer();
        }
    }

   
    public void resetButton()
    {
        ResetPlayer();
    }

    private void ResetPlayer()
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

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

        if (deathCounter != null)
        {
            deathCounter.Increment();
        }

        Debug.Log("Player Reset via Button!");
    }
}

