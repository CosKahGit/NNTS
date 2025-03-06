using UnityEngine;
using UnityEngine.SceneManagement;

public class ReachGoal : MonoBehaviour
{
    private Timer timer;
    private DeathCounter deathCounter;
    private CampaignManager campaignManager;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        deathCounter = FindObjectOfType<DeathCounter>();
        campaignManager = CampaignManager.GetInstance();

        if (campaignManager != null)
        {
            campaignManager.RegisterComponents(); //F�r fr�n Campaignmanager
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        float time = timer != null ? timer.getTime() : 0;
        int deaths = deathCounter != null ? deathCounter.getDeaths() : 0;
        string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (campaignManager != null)//Kollar ifall det �r campaign
        {
            campaignManager.LevelCompleted();
        }
        else
        {
            SaveLevelRun(currentLevel, time, deaths);
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel);
        }
    }

    private void SaveLevelRun(string levelName, float time, int deaths)
    {
        int runCount = PlayerPrefs.GetInt(levelName + "_RunCount", 0);

        PlayerPrefs.SetFloat(levelName + "_Time_" + runCount, time);
        PlayerPrefs.SetInt(levelName + "_Deaths_" + runCount, deaths);
        PlayerPrefs.SetInt(levelName + "_RunCount", runCount + 1);
        PlayerPrefs.Save();

        Debug.Log("Saved Run for " + levelName + " - Time: " + time + ", Deaths: " + deaths);
    }
}
