using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.EventSystems;

public class RotatingFish_PlusMinusButtons : MonoBehaviour, IPointerClickHandler
{ 
    public bool increase = false;
    public GameObject parent;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (increase)
            parent.GetComponent<Sampler_UIManager>().IncreaseCurrentValue();
        else
            parent.GetComponent<Sampler_UIManager>().DecreaseCurrentValue();
    }
}