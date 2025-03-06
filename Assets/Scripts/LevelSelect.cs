using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene(2); // Load Level 1
        new GameObject("CampaignManager").AddComponent<CampaignManager>(); // Create the Campaign Manager
    }

    public void PlayLevel1() { SceneManager.LoadScene(2); } //TUTORIAL
    public void PlayLevel2() { SceneManager.LoadScene(3); } //TUT 2
    public void PlayLevel3() { SceneManager.LoadScene(4); } //DEMO
    public void PlayLevel4() { SceneManager.LoadScene(5); } //HANNES
    public void Back() { SceneManager.LoadScene(0); }
    public void ExitGame() { Application.Quit(); }
}
