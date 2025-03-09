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

    public void GoBack()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartCampaign()
    {
        PlayerPrefs.SetInt("LastPlayedMode", 1); //S�tter till campaign
        PlayerPrefs.Save();

        SceneManager.LoadScene(2); //Level 1
        new GameObject("CampaignManager").AddComponent<CampaignManager>(); //Campaign manager g�r att man kan spara flera tider �ver flera levlar
    }

    public void PlayLevel1() { SetSingleLevelMode(); SceneManager.LoadSceneAsync(2); }
    public void PlayLevel2() { SetSingleLevelMode(); SceneManager.LoadSceneAsync(3); }
    public void PlayLevel3() { SetSingleLevelMode(); SceneManager.LoadSceneAsync(4); }
    public void PlayLevel4() { SetSingleLevelMode(); SceneManager.LoadSceneAsync(5); }
    public void PlayLevel5() { SetSingleLevelMode(); SceneManager.LoadSceneAsync(6); }

    private void SetSingleLevelMode()
    {
        PlayerPrefs.SetInt("LastPlayedMode", 0);//S�tter till 0
        PlayerPrefs.Save();
    }

}
