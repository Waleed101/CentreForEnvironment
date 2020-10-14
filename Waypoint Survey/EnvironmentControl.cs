/*
 * Name: EnvironmentControl (EnvironmentControl.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-13
 * Last Modified: 2020-09-06
 * Used in: Aquaculture, Waypoint Survey
 * Description: Test script used to try to modify the ocean AQUAS transparency in script form. Not currently used in scenes, but can be to start adding weather change.
 * Status: TEST
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentControl : MonoBehaviour
{
    [Range(0.0f, 100.0f)] public float oceanTransparency = 0.0f;
    public GameObject ocean;
    private Renderer render;
    private Color color;

    void Start()
    {
        render = ocean.GetComponent<Renderer>(); // Get the ocean renderer
    }

    void Update()
    {
        // Set the color based on whats set in the inspector
        color = render.material.color;
        color.a = oceanTransparency / 100f;
        render.material.color = color;
     }
}
