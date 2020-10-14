/*
 * Name: Draw Seaweed Border (DrawSeaweedBorder.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-05
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to clearly identify the border at which the seaweed and grass will spawn programmatically in the region
 * Status: TEST
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSeaweedBorder : MonoBehaviour
{
    public Transform topRightTransform, bottomLeftTransform;
    private void OnDrawGizmosSelected() // Non gameplay function (can be run without clicking the Play button
    {
        // Draws a rectangle between the two corner gameobjects dropped into to the seaweed border
        Vector3 topRight = topRightTransform.position, bottomLeft = bottomLeftTransform.position;
        Gizmos.DrawLine(topRight, new Vector3(topRight.x, 0f, bottomLeft.z));
        Gizmos.DrawLine(bottomLeft, new Vector3(topRight.x, 0f, bottomLeft.z));
        Gizmos.DrawLine(bottomLeft, new Vector3(bottomLeft.x, 0f, topRight.z));
        Gizmos.DrawLine(topRight, new Vector3(bottomLeft.x, 0f, topRight.z));
    }
}
