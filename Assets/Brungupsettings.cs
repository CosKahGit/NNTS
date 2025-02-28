using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bringupsettings : MonoBehaviour
{
    public GameObject setting;
    private bool issettingactive;
    private PlayerCam playerCam;

    private void Start()
    {
        playerCam = GetComponent<PlayerCam>();

        // Ensure settings menu is OFF at start
        setting.SetActive(false);
        issettingactive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!issettingactive)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        setting.SetActive(true);
        issettingactive = true;

        if (playerCam != null)
            playerCam.enabled = false; // Disable camera movement

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        setting.SetActive(false);
        issettingactive = false;

        if (playerCam != null)
            playerCam.enabled = true; // Re-enable camera movement

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
