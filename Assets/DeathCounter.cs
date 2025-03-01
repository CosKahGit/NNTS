using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathCounter;
    private int deaths = 0;

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
        deathCounter.text = "Gen: " + deaths;
    }

    public int getDeaths()
    {
        return deaths;
    }
}
