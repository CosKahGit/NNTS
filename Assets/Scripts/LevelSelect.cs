using System.Collections;
using System.Collections.Generic;
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

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayLevel1()
    {
        SceneManager.LoadSceneAsync(2); 
    }

    public void PlayDemo()
    {
        SceneManager.LoadSceneAsync(5); 
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync(0); 
    }


}
