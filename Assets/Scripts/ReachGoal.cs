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
            campaignManager.RegisterComponents(); // Ensure stats are tracked
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (campaignManager != null)
        {
            campaignManager.LevelCompleted(); // Move to next level
        }
        else
        {
            // Normal single level play (fallback)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
