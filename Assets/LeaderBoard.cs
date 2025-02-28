using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private DeathCounter deathCounter = new DeathCounter();
    private Timer timer = new Timer();

    private Transform entryContainer;
    private Transform entryTemplate;
    // Start is called before the first frame update
    private void Awake()
    {
        entryContainer = transform.Find("HighScoreContainer");
        entryTemplate = entryContainer.Find("HighScoreTemplate");

        entryTemplate.gameObject.SetActive(false);

        float templateHeight = 20f;

        for(int i = 0; i < 5; i++)
        {
            //Instantiate(entryTemplate, entryContainer);
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

        }
    }
}
