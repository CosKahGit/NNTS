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
            Debug.LogError("ERROR: Leaderboard UI is missing!");
            return;
        }

        entryTemplate.gameObject.SetActive(false);

        // Detect if we are in Campaign Mode
        isCampaign = CampaignManager.GetInstance() != null;

        if (isCampaign)
        {
            LoadCampaignLeaderboard();
        }
        else
        {
            currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            LoadLevelLeaderboard();
        }
    }

    private void LoadCampaignLeaderboard()
    {
        float totalTime = PlayerPrefs.GetFloat("TotalTime", 0);
        int totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);

        AddEntry(totalTime, totalDeaths);
        Debug.Log("Loaded Campaign Leaderboard");
    }

    private void LoadLevelLeaderboard()
    {
        int runCount = PlayerPrefs.GetInt(currentLevel + "_RunCount", 0);
        if (runCount == 0)
        {
            Debug.Log("No previous runs found for " + currentLevel);
            return;
        }

        for (int i = 0; i < runCount; i++)
        {
            float time = PlayerPrefs.GetFloat(currentLevel + "_Time_" + i, 0);
            int deaths = PlayerPrefs.GetInt(currentLevel + "_Deaths_" + i, 0);
            AddEntry(time, deaths);
        }

        Debug.Log("Loaded Individual Leaderboard for " + currentLevel);
    }

    private void AddEntry(float time, int deaths)
    {
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        entryTransform.gameObject.SetActive(true);

        TextMeshProUGUI timeText = entryTransform.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI deathText = entryTransform.Find("DeathText")?.GetComponent<TextMeshProUGUI>();

        if (timeText == null || deathText == null)
        {
            Debug.LogError("ERROR: 'TimeText' or 'DeathText' is missing inside 'LeaderboardEntryTemplate'.");
            return;
        }

        timeText.text = FormatTime(time);
        deathText.text = "" + deaths;
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
            PlayerPrefs.DeleteKey("TotalTime");
            PlayerPrefs.DeleteKey("TotalDeaths");
            Debug.Log("Reset Campaign Leaderboard");
        }
        else
        {
            PlayerPrefs.DeleteKey(currentLevel + "_RunCount");
            for (int i = 0; i < 10; i++)
            {
                PlayerPrefs.DeleteKey(currentLevel + "_Time_" + i);
                PlayerPrefs.DeleteKey(currentLevel + "_Deaths_" + i);
            }
            Debug.Log("Reset Leaderboard for " + currentLevel);
        }

        PlayerPrefs.Save();

        foreach (Transform child in entryContainer)
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
