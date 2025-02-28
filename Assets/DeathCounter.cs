using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathCounter;
    int deaths;

    private void Start()
    {
        UpdateDeathUI();
    }

    public void Increment()
    {
        deaths++;
        UpdateDeathUI();
    }

    private void UpdateDeathUI()
    {
        deathCounter.text = "Deaths: " + deaths;
    }
}
