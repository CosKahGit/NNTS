using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;

    private void Awake()
    {
        entryContainer = transform.Find("LeaderboardContainer");
        if (entryContainer == null)
        {
            Debug.LogError(" ERROR: 'LeaderboardContainer' is missing! Make sure it's inside 'LeaderboardPanel'.");
            return;
        }

        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");
        if (entryTemplate == null)
        {
            Debug.LogError(" ERROR: 'LeaderboardEntryTemplate' is missing! Make sure it's inside 'LeaderboardContainer'.");
            return;
        }

        entryTemplate.gameObject.SetActive(false);

        // Check if this is the first time running the game
        if (!PlayerPrefs.HasKey("RunCount"))
        {
            Debug.Log("No saved leaderboard found. Initializing default leaderboard.");
            //SetDefaultLeaderboard();
        }

        // Load leaderboard (either defaults or saved data)
        LoadLeaderboard();
    }


    private void LoadLeaderboard()
    {
        int runCount = PlayerPrefs.GetInt("RunCount", 0);
        if (runCount == 0)
        {
            Debug.Log("No previous runs found.");
            return;
        }

        List<(float time, int deaths)> runs = new List<(float, int)>();

        for (int i = 0; i < runCount; i++)
        {
            float time = PlayerPrefs.GetFloat("RunTime_" + i, 0);
            int deaths = PlayerPrefs.GetInt("RunDeaths_" + i, 0);
            runs.Add((time, deaths));
        }

        // Sort by fastest time
        runs.Sort((a, b) => a.time.CompareTo(b.time));

        // Show top 5
        int maxEntries = Mathf.Min(9, runs.Count);
        for (int i = 0; i < maxEntries; i++)
        {
            AddEntry(runs[i].time, runs[i].deaths);
        }
    }

    /*private void SetDefaultLeaderboard()
    {
        // Default leaderboard values
        float[] defaultTimes = { 20f, 25f, 30f, 35f, 40f }; // Times in seconds
        int[] defaultDeaths = { 13, 16, 5, 84, 43 }; // Deaths for each entry

        int maxEntries = defaultTimes.Length;
        PlayerPrefs.SetInt("RunCount", maxEntries);

        for (int i = 0; i < maxEntries; i++)
        {
            PlayerPrefs.SetFloat("RunTime_" + i, defaultTimes[i]);
            PlayerPrefs.SetInt("RunDeaths_" + i, defaultDeaths[i]);
        }

        PlayerPrefs.Save();
        Debug.Log("Default leaderboard set.");
    }*/




    private void AddEntry(float time, int deaths)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        entryTransform.gameObject.SetActive(true);

        TextMeshProUGUI timeText = entryTransform.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI deathText = entryTransform.Find("DeathText")?.GetComponent<TextMeshProUGUI>();

        if (timeText == null || deathText == null)
        {
            Debug.LogError(" ERROR: 'TimeText' or 'DeathText' is missing inside 'LeaderboardEntryTemplate'.");
            return;
        }

        timeText.text = FormatTime(time);
        deathText.text = "" + deaths;
    }



    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void ResetLeaderboard()
    {
        // Clear all previous runs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        Debug.Log("Leaderboard reset. Setting default values...");

        // Set the default leaderboard again
        //SetDefaultLeaderboard();

        // Clear the UI leaderboard entries
        foreach (Transform child in entryContainer)
        {
            Destroy(child.gameObject);
        }

        // Reload leaderboard to show default values
        LoadLeaderboard();
    }



}
