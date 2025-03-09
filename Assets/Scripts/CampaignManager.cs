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

    
    private int[] levelSceneIndexes = { 2, 3, 4, 5, 6 };

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // För att campaignen ska fungera så behöver man samma playerobject
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

    public void RegisterComponents()//Skickar till ReachGoal
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
            EndCampaign();
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

    private void EndCampaign() //För att leaderboarden ska dyka upp efter spelet
    {

        if (totalTime <= 0f) //Sparade en 00:00 time hela tiden
        {
            return;
        }


        int runCount = PlayerPrefs.GetInt("Campaign_RunCount", 0); // PlayerPrefs gör att man kan spara inom Unity
        runCount++;

        PlayerPrefs.SetInt("Campaign_RunCount", runCount);  
        PlayerPrefs.SetFloat($"Campaign_Time_{runCount}", totalTime);//Man sparar inom PlayerPrefs med Campaign_Time_i 
        PlayerPrefs.SetInt($"Campaign_Deaths_{runCount}", totalDeaths);
        PlayerPrefs.Save();

        Debug.Log("Campaign Finished! Stats Saved.");

        SceneManager.LoadScene(6); // Sista skärmen
    }


    public static CampaignManager GetInstance()
    {
        return instance;
    }
}
