using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private string currentLevel;
    private bool isCampaign;

    private void Awake()
    {
        entryContainer = transform.Find("LeaderboardContainer");
        entryTemplate = entryContainer.Find("LeaderboardEntryTemplate");

        if (entryContainer == null || entryTemplate == null)
        {
            Debug.LogError(" Leaderboard UI missing");
            return;
        }

        entryTemplate.gameObject.SetActive(false);//Tar bort template texten

        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        isCampaign = PlayerPrefs.GetInt("LastPlayedMode", 0) == 1;

        if (isCampaign)
        {
            Invoke(nameof(LoadCampaignLeaderboard), 0.2f);//Floaten ger en delay så det laddas korrekt, kan bli null error
        }
        else
        {
            Invoke(nameof(LoadLevelLeaderboard), 0.2f);
        }
    }




    private void LoadCampaignLeaderboard()
    {
        int runCount = PlayerPrefs.GetInt("Campaign_RunCount", 0);
        if (runCount == 0)
        {
            Debug.Log("No campaign runs");
            return;
        }

        List<(float time, int deaths)> runs = new List<(float, int)>();

        for (int i = 1; i <= runCount; i++) // Start at 1, since we now save multiple runs
        {
            float time = PlayerPrefs.GetFloat($"Campaign_Time_{i}", 0);
            int deaths = PlayerPrefs.GetInt($"Campaign_Deaths_{i}", 0);
            runs.Add((time, deaths));
        }

        runs.Sort((a, b) => a.time.CompareTo(b.time)); //Sorterar

        int maxEntries = Mathf.Min(9, runs.Count);
        for (int i = 0; i < maxEntries; i++)
        {
            AddEntry(runs[i].time, runs[i].deaths);
        }

        Debug.Log(" Loaded Top 9 Campaign Runs");
    }



    private void LoadLevelLeaderboard()
    {
        if (isCampaign) return; //Bug fix för att inte kunna kö campaign 

        int runCount = PlayerPrefs.GetInt(currentLevel + "_RunCount", 0);
        if (runCount == 0)
        {
            Debug.Log("No previous runs found for " + currentLevel);
            return;
        }

        List<(float time, int deaths)> runs = new List<(float, int)>();

        for (int i = 0; i < runCount; i++)
        {
            float time = PlayerPrefs.GetFloat(currentLevel + "_Time_" + i, 0);
            int deaths = PlayerPrefs.GetInt(currentLevel + "_Deaths_" + i, 0);
            runs.Add((time, deaths));
        }

        runs.Sort((a, b) => a.time.CompareTo(b.time));

        int maxEntries = Mathf.Min(9, runs.Count);
        for (int i = 0; i < maxEntries; i++)
        {
            AddEntry(runs[i].time, runs[i].deaths);
        }

        Debug.Log("Loaded Top 9 Runs for " + currentLevel);
    }



    private void AddEntry(float time, int deaths)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        entryTransform.gameObject.SetActive(true);

        TextMeshProUGUI timeText = entryTransform.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI deathText = entryTransform.Find("DeathText")?.GetComponent<TextMeshProUGUI>();

        if (timeText == null || deathText == null)
        {
            Debug.LogError("'TimeText' or 'DeathText' is missing inside 'LeaderboardEntryTemplate'");
            return;
        }

        timeText.text = FormatTime(time);
        deathText.text = "" + deaths;//Gör till string
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
        if (isCampaign)
        {
            int runCount = PlayerPrefs.GetInt("Campaign_RunCount", 0); //Får antalet runs för leaderboarden

            
            for (int i = 1; i <= runCount; i++)
            {
                PlayerPrefs.DeleteKey($"Campaign_Time_{i}");
                PlayerPrefs.DeleteKey($"Campaign_Deaths_{i}");
            }

            PlayerPrefs.DeleteKey("Campaign_RunCount");

        }
        else
        {
            PlayerPrefs.DeleteKey(currentLevel + "_RunCount");
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.DeleteKey(currentLevel + "_Time_" + i);
                PlayerPrefs.DeleteKey(currentLevel + "_Deaths_" + i);
            }
            Debug.Log($"Reset Leaderboard for {currentLevel}");
        }

        PlayerPrefs.Save();

        foreach (Transform child in entryContainer)//Tar bort alla entries
        {
            if (child != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        if (isCampaign)
        {
            LoadCampaignLeaderboard();
        }
        else
        {
            LoadLevelLeaderboard();
        }
    }
    }

