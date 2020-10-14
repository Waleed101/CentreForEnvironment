/*
 * Name: Menu Navigation Arrow (MenuNavigationArrows_Aquaculture.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-17
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used by left and right buttons in UI to track whether or not they are clicked and trigger the function in the ManageScene/RouteSelector script
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


public class MenuNavigationArrows_Aquaculture : MonoBehaviour, IPointerClickHandler
{
    public bool left = false;
    public GameObject parent;

    public void OnPointerClick(PointerEventData pointerEventData) // Trigger function when clicked
    {
        if (left)
            parent.GetComponent<DisplayRoutes>().LeftClicked();
        else
            parent.GetComponent<DisplayRoutes>().RightClicked();
    }
}
