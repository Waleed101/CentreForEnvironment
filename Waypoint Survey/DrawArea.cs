/*
 * Name: Draw Area (DrawArea.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture, Waypoint Survey
 * Description: Used to clearly identify the waypoints for route creation, outlining them in yellow
 * Status: TEST
 */

using UnityEngine;
using System.Collections;

public class DrawArea : MonoBehaviour
{
    public float radius = 10f;
    void OnDrawGizmosSelected() // Non gameplay function (can be run without clicking the Play button
    { 
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}