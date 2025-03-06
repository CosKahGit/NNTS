using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowImageOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject imageToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        imageToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        imageToShow.SetActive(false);
    }
}
