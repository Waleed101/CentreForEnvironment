/*
 * Name: Hover Click Behaviour (HoverClickBehaviour.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: PHASED OUT
 * Description: Previously used to display arrows on hover over text in the final UI of the Waypoint Survey. Upon phase out of that system, this was deemed unnecssary. Can be removed before final build.
 * Status: UNUSED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityEngine.EventSystems;

public class HoverClickBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public GameObject control;
    public int menuSpot = 0;
    public bool changeOnHover = false;

    void Update()
    {
        if (changeOnHover)
        {
            if (control.GetComponent<HoverControl>().GetActive() == menuSpot)
                transform.GetChild(1).gameObject.SetActive(true);
            else
                transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (changeOnHover)
            control.GetComponent<HoverControl>().SetActive(menuSpot);
        else
            print("Hover doesn't matter");
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        control.GetComponent<Menu>().SetScene(menuSpot);
    }
}