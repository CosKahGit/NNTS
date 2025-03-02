using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    private float elapsedTime;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timerText.text = FormatTime(elapsedTime);
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    public float getTime()
    {
        return elapsedTime;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100); // Get the last two decimal places

        return string.Format("{0:00}:{1:00},{2:00}", minutes, seconds, milliseconds);
    }

}


