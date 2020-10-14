/*
 * Name: Change Weight (ChangeWeight.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-15
 * Last Modified: 2020-09-06
 * Used in: Sampling
 * Description: Script used to change the number that comes up on the scale to provide a more intuitive feel to the sampling. Can be modified to include a more natural count of the number by referencing gameControl and the time at which the fish arrives at the 
 * specific sampling point
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeWeight : MonoBehaviour
{
    public GameObject UIControl, gameControl; // References to the UI control object and the gamecontrol

    void Start()
    {
        // Ensure that they have been dragged in appropriately, if not reference the specific tags 
        if (UIControl == null)
            UIControl = GameObject.FindGameObjectWithTag("UI Parent");
        if (gameControl == null)
            gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        if (UIControl.GetComponent<Sampler_UIManager>().GetCurrentRecording() == 2) // Ensure that they are at the correct station, no need to modify otherwise
            GetComponent<TextMeshPro>().text = UIControl.GetComponent<Sampler_UIManager>().GetData().ToString("F2"); // Get the weight from UI manager and display it on the screen
    }
}
