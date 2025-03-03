using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachGoal : MonoBehaviour
{
    private Timer timer;
    private DeathCounter deathCounter;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        deathCounter = FindObjectOfType<DeathCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Get current run stats
        float lastRunTime = timer != null ? timer.getTime() : 0;
        int lastRunDeaths = deathCounter != null ? deathCounter.getDeaths() : 0;

        // Save stats to PlayerPrefs
        SaveRun(lastRunTime, lastRunDeaths);

        // Load the leaderboard scene (or restart)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveRun(float time, int deaths)
    {
        List<(float time, int deaths)> runs = new List<(float, int)>();

        int runCount = PlayerPrefs.GetInt("RunCount", 0);

        // Load existing runs
        for (int i = 0; i < runCount; i++)
        {
            float savedTime = PlayerPrefs.GetFloat("RunTime_" + i, 0);
            int savedDeaths = PlayerPrefs.GetInt("RunDeaths_" + i, 0);
            runs.Add((savedTime, savedDeaths));
        }

        // Add new run
        runs.Add((time, deaths));

        // Sort runs by time (ascending)
        runs.Sort((a, b) => a.time.CompareTo(b.time));

        // Keep only the top runs
        int maxEntries = Mathf.Min(9, runs.Count);
        PlayerPrefs.SetInt("RunCount", maxEntries);

        for (int i = 0; i < maxEntries; i++)
        {
            PlayerPrefs.SetFloat("RunTime_" + i, runs[i].time);
            PlayerPrefs.SetInt("RunDeaths_" + i, runs[i].deaths);
        }

        PlayerPrefs.Save();

        Debug.Log("Saved top " + maxEntries + " runs." );
    }

}
