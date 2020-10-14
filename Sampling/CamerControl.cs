/*
 * Name: Camera Controller (CamerControl.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-01
 * Last Modified: 2020-09-05
 * Used in: Sampling
 * Description: Script to control how the user moves the camera to look at the fish in sampling mode. Functionality includes arrow keys/wasd cooresponding to camera movement and zoom based on the scroll bar/double pinching the touchbar.
 * Status: PRODUCTION
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerControl : MonoBehaviour
{
    // Various user defined settings; minClamp and maxClamp: min and max zoom, zoomSenstivity: speed at which the camera zooms, arrowSenstivity: speed at which the camera moves, defaultFieldOfView: default zoom
    public float minClamp, maxClamp, zoomSensitivity, arrowSensitivity, defaultFieldOfView = 70f;

    // References to to the gameobject
    public GameObject gameControl;

    public bool invert; // Used for testing when camera was inverted or moving incorrectly
    private bool clampEnabled = true, zoomEnabled = true, controlsEnabled = true;
    
    void Start()
    {
        Camera.main.fieldOfView = defaultFieldOfView; // Setting zoom to default
    }

    void Update()
    {
        Vector3 inputs = new Vector3(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")*-1f, 0f) * arrowSensitivity; // Reading in the arrow inputs
        if (!invert) // Inverting
            inputs *= -1f;
        // Modifying the camera rotation accordingly
        Vector3 tempRot = Camera.main.transform.eulerAngles + inputs;
        if(controlsEnabled)
            Camera.main.transform.eulerAngles = tempRot;

        // Changing zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float fov = Camera.main.fieldOfView;
        if(zoomEnabled)
            fov += scrollInput * zoomSensitivity * -1;
        if(clampEnabled)
            fov = Mathf.Clamp(fov, minClamp, maxClamp);
        Camera.main.fieldOfView = fov;            
    }

    // Functions for external scripts to enable or disable the different controls
    public void ClampSettings(bool clamp) { clampEnabled = clamp; }
    public void ControlSettings(bool control) { controlsEnabled = control; }
    public void ZoomSettings(bool zoom) { zoomEnabled = zoom; }
}
