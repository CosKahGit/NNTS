using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI totalTimeText;
    public TextMeshProUGUI totalDeathsText;

    private void Start()
    {
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0);
        int totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);

        totalTimeText.text = "Total Time: " + FormatTime(totalTime);
        totalDeathsText.text = "Total Deaths: " + totalDeaths;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int milliseconds = Mathf.FloorToInt((time * 100) % 100);
        return string.Format("{0:00}:{1:00},{2:00}", minutes, seconds, milliseconds);
    }

    public void ResetLeaderboard()
    {
        PlayerPrefs.DeleteKey("TotalTime");
        PlayerPrefs.DeleteKey("TotalDeaths");
        PlayerPrefs.Save();

        totalTimeText.text = "Total Time: 00:00,00";
        totalDeathsText.text = "Total Deaths: 0";
    }
}
