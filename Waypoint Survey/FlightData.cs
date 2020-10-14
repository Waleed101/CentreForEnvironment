/*
 * Name: Flight Data (FlightData.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-09-01
 * Last Modified: 2020-09-06
 * Used in: Aquaculture, Waypoint Survey
 * Description: Used to communicate the current time and location of the plane to provide a more HUD feel to the user
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlightData : MonoBehaviour
{
    // References to different game object components
    public GameObject recordData, locComms, timeComms;
    private bool active = false;
    private float actStartTime = 0f;
    private void Start()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!active) // Ensure that the flight is currently active
        {
            active = GameObject.FindGameObjectWithTag("Plane").GetComponent<move>().IsPPressed();
            if(active)
                actStartTime = Time.realtimeSinceStartup; // Get the start flight time
            this.transform.GetChild(0).gameObject.SetActive(active); // Set the visibility of the UI components to ON
        }
        else
        {
            Vector2 location = recordData.GetComponent<RecordDataORG>().GetLatLng(); // Get relative coordinates from the record data script
            locComms.GetComponent<TextMeshProUGUI>().text = "(" + location.x + ", " + location.y + ")"; // Convert them into readable
            
            // Get the flight time and convert it into readable format
            float timer = Time.realtimeSinceStartup - actStartTime; float minutes = Mathf.Floor(timer / 60);
            float seconds = Mathf.RoundToInt(timer % 60);
            string minutesStr = minutes.ToString(), secondsStr = Mathf.RoundToInt(seconds).ToString();
            if (minutes < 10)
                minutesStr = "0" + minutes.ToString();
            if (seconds < 10)
                secondsStr = "0" + Mathf.RoundToInt(seconds).ToString();
            timeComms.GetComponent<TextMeshProUGUI>().text = minutesStr + ":" + secondsStr; // Display it on the UI to the user
        }
    }
}
