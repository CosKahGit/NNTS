using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCam : MonoBehaviour
{
    public float sens; // Default sensitivity
    public Slider slider; // Assigned via Inspector

    public Transform orientation;
    public Transform camHolder;

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Load sensitivity from PlayerPrefs
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sens = PlayerPrefs.GetFloat("Sensitivity");
        }
        else
        {
            PlayerPrefs.SetFloat("Sensitivity", sens); // Save default if not found
            PlayerPrefs.Save();
        }

        if (slider != null)
        {
            slider.value = sens; // Set slider to saved value
            slider.onValueChanged.AddListener(sensChange); // Connect function to slider
        }
    }

    private void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }

    public void sensChange(float newSpeed)
    {
        sens = newSpeed; // Update mouse sensitivity
        PlayerPrefs.SetFloat("Sensitivity", sens); // Save to PlayerPrefs
        PlayerPrefs.Save();
    }
}
