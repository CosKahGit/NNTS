using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Settings : MonoBehaviour
{
   public void toMainMenu()
    {
        SceneManager.LoadSceneAsync(0); 
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
