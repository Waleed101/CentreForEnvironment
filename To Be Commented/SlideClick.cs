using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.EventSystems;

public class SlideClick : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public bool changeOnHover = true, left = false;

    public void OnPointerEnter(PointerEventData eventData) {}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (left)
            gameObject.GetComponentInParent<FinalManager>().NextPage();
        else
            gameObject.GetComponentInParent<FinalManager>().PreviousPage();
    }
}
