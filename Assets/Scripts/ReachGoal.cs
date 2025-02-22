using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("You entered the goal!"); 
    }
}
