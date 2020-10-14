/*
 * Name: Draw Flight Path (DrawFlightPath.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture, Waypoint Survey
 * Description: Used to identify the route defined, referencing the selected route in gamecontrol's selected route variable
 * Status: TEST
 */

using UnityEngine;
using System.Collections;

public class DrawFlightPath : MonoBehaviour
{
    public bool connectToEnd = true;

    // Reference to gamecontrol
    public GameObject gameControl;

    void OnDrawGizmos() // Non gameplay function (can be run without clicking the Play button
    {
        // Get the route and all its child components
        int route = gameControl.GetComponent<WaypointManagement>().GetSelectedRoute();
        Transform[] childrenCount = this.transform.GetChild(route).GetComponentsInChildren<Transform>();
        Gizmos.color = Color.black;

        // Draw out route
        for(int i = 1; i < childrenCount.Length-1; i++)
            Gizmos.DrawLine(childrenCount[i].position, childrenCount[i+1].position);

        // Connection to end (last to first point line draw between can be turned off in the inspector
        if(connectToEnd)
            Gizmos.DrawLine(childrenCount[childrenCount.Length - 1].position, childrenCount[1].position);
    }
}