using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // If using TextMeshPro
using UnityEngine.UI;

public class TooltipHandler : MonoBehaviour
{
    public GameObject tooltipObject; // Assign in Inspector
    public TextMeshProUGUI tooltipText; // Use Text or TextMeshPro

    public void ShowTooltip(string message)
    {
        tooltipObject.SetActive(true);
        tooltipText.text = message;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
