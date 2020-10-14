/*
 * Name: Display Routes (DisplayRoutes.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-05
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to communicate the different route options for the aquaculture sampling to the user and record which one they select
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayRoutes : MonoBehaviour
{
    // References to different UI components and gamecontrol
    public GameObject gameControl, cylinderPrefab, pageText, leftArrow, rightArrow;
    public Transform cylinderParent;
    public int cylinderHeight;
    private GameObject[] cylinders;
    private int numberOfRouteChoices = 0, numberOfChoicesPerRoute, currentPage = 1;
    private int[,] routes;

    private bool ftime = true;

    // Update is called once per frame
    void Update()
    {
        if(ftime && (gameControl.GetComponent<ManageScene>().GetActiveComponent() == 2))
        {
            cylinders = new GameObject[gameControl.GetComponent<SelectRoute>().GetNumberOfAquaculturesPerRoute()];
            routes = gameControl.GetComponent<SelectRoute>().GetRouteChoices();
            numberOfRouteChoices = gameControl.GetComponent<SelectRoute>().GetNumberOfRouteChoices();
            FixButtonPreviews();
            IntitateCylinderPrefabs();
            UpdateOverlays();
        }
    }

    // Functions referenced by the buttons to control which route they are currently on
    public void LeftClicked() { print("left"); currentPage--; FixButtonPreviews(); UpdateOverlays(); }
    public void RightClicked() { print("right"); currentPage++; FixButtonPreviews(); UpdateOverlays(); }

    private void FixButtonPreviews() // Function to adjust numbers and whether or not there is a left/right button based
    {
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
        pageText.GetComponent<TextMeshProUGUI>().text = currentPage + "/" + numberOfRouteChoices;
        if (currentPage == 1)
            leftArrow.SetActive(false);
        if (currentPage == numberOfRouteChoices)
            rightArrow.SetActive(false);
    }

    private void IntitateCylinderPrefabs() // Spawn in the cylinder prefabs
    {
        for (int i = 0; i < cylinders.Length; i++)
            cylinders[i] = Instantiate(cylinderPrefab, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), cylinderParent);
    }

    private void UpdateOverlays() // Function to update the overlays
    {
        if (routes == null) // Ensure that there are actually routes defined, this prevents a fatal error
            print("null");
        else
        {
            for (int i = 0; i < cylinders.Length; i++) 
            {
                Vector3 aquaLocation = gameControl.GetComponent<RotatingFish>().GetAquacultureLocation(routes[currentPage - 1, i]);
                aquaLocation.y = cylinderHeight;
                cylinders[i].transform.position = aquaLocation;
                ftime = false;
            }
        }
    }

    private void OnDrawGizmos() // Unused function to draw lines between each of the spheres to outline the route; can easily be turned on by displaying gizmos in the Play mode
    {
        if (gameControl.GetComponent<ManageScene>().GetActiveComponent() == 2) // Checks to make sure currently at the Select scene mode
            for (int i = 1; i < cylinders.Length; i++)
                Gizmos.DrawLine(cylinders[i - 1].transform.position, cylinders[i].transform.position);
    }

    public int GetCurrentPage() { return currentPage; } // Function to get the current page
}
