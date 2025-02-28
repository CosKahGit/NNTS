using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ReachGoal : MonoBehaviour
{
    public LeaderBoard leaderBoard;
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadSceneAsync(1); 
    }
}
