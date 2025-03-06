using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampaignManager : MonoBehaviour
{
    private static CampaignManager instance;

    private int currentLevelIndex = 0;
    private float totalTime = 0f;
    private int totalDeaths = 0;

    private Timer timer;
    private DeathCounter deathCounter;

    
    private int[] levelSceneIndexes = { 2, 3, 4, 5 }; // Scene indexes for levels 1-4

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void RegisterComponents()
    {
        timer = FindObjectOfType<Timer>();
        deathCounter = FindObjectOfType<DeathCounter>();
    }

    private void LoadLevel(int levelIndex)
    {
        if (levelIndex < levelSceneIndexes.Length)
        {
            SceneManager.LoadScene(levelSceneIndexes[levelIndex]);
            currentLevelIndex = levelIndex;
        }
        else
        {
            EndCampaign(); // All levels completed
        }
    }

    public void LevelCompleted()
    {
        if (timer != null && deathCounter != null)
        {
            totalTime += timer.getTime();
            totalDeaths += deathCounter.getDeaths();
        }

        currentLevelIndex++;
        if (currentLevelIndex < levelSceneIndexes.Length)
        {
            LoadLevel(currentLevelIndex);
        }
        else
        {
            EndCampaign();
        }
    }

    private void EndCampaign()
    {
        PlayerPrefs.SetFloat("TotalTime", totalTime);
        PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
        PlayerPrefs.Save();

        SceneManager.LoadScene(6); // Load leaderboard or results scene
    }

    public static CampaignManager GetInstance()
    {
        return instance;
    }
}
