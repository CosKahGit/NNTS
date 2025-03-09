using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents destruction on scene load
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true; // Ensures looping
            audioSource.Play(); // Starts playing the background music
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
}
