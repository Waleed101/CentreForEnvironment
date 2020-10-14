/*
 * Name: Communicate Data (CommunicateData.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-05
 * Last Modified: 2020-09-06
 * Used in: Waypoint Survey
 * Description: Script used to communicate in flight data and fish spawning data in real time for in game debugging purposes. Fish spawning reference has not been tested recently, so proceed with caution on usage.
 * Status: TEST
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommunicateData : MonoBehaviour
{
    public GameObject gameControl, textComm, plane, bg; // References to the different UI components
    [HideInInspector] public int clickD = 0; // Tracks current page

    // Different values communicated to the user about the flight
    [HideInInspector] private Vector3 currentLocation, targetWaypoint, flightAngle;
    [HideInInspector] private float moveSpeed, turbineSpeed, currentWaypoint;

    // Different values communicated to the user about the fish
    [HideInInspector] string fishName;
    [HideInInspector] float speed, groupSize;
    [HideInInspector] Vector3 currentSchoolLocation, relativeDistance, heading;

    // Actual strings communicated in the UI
    [HideInInspector] private string flightText = "", fishText = "No Current Fish Spawned";
    
    void Start()
    {
        // Turning off the UI text component
        textComm.GetComponent<TextMeshProUGUI>().text = "null";
        textComm.SetActive(false);
        bg.SetActive(false);
    }

    void Update()
    {
        // Turn on the UI if there is an active flight
        // YOU CAN TURN THIS WHOLE FUNCTION OFF BY COMMENTING OUT THE NEXT THREE LINES
        if (Input.GetKeyUp(KeyCode.F))
            if(gameControl.GetComponent<WaypointManagement>().GetFlightStatus())
                incD();

        if(clickD != 0)
        {
            GetFlightInfo();
            GetFishInfo();
            if (clickD == 1) // Display flight data
                textComm.GetComponent<TextMeshProUGUI>().text = flightText;
            else // Display fish data
                textComm.GetComponent<TextMeshProUGUI>().text = fishText;
        }
    }

    void GetFlightInfo()
    {
        // Get all the current flight data 
        currentLocation = plane.GetComponent<move>().GetLocation();
        flightAngle = plane.GetComponent<move>().GetAngle();
        targetWaypoint = plane.GetComponent<move>().GetTargetWaypoint();
        moveSpeed = plane.GetComponent<move>().GetMovementSpeed();
        turbineSpeed = plane.GetComponent<move>().GetTurbineSpeed();
        currentWaypoint = plane.GetComponent<move>().GetCurrentWaypointID();
        flightText =
          "FLIGHT STATS\n" +
          "Location: " + Mathf.Round(currentLocation.x * 100) / 100 + ", " + Mathf.Round(currentLocation.y * 100) / 100 + ", " + Mathf.Round(currentLocation.z * 100) / 100 + "\n" +
          "Angle: " + flightAngle.x + ", " + flightAngle.y + ", " + flightAngle.z + "\n" +
          "Target Waypoint (" + currentWaypoint + "): " + Mathf.Round(targetWaypoint.x * 100) / 100 + ", " + Mathf.Round(targetWaypoint.y * 100) / 100 + ", " + Mathf.Round(targetWaypoint.z * 100) / 100 + "\n" +
          "Move Speed: " + moveSpeed + "\t Turbine Speed: " + turbineSpeed +
          "\nFPS: " + (int)(Mathf.Round(1.0f/Time.deltaTime)/10) * 10;
    }

    void GetFishInfo()
    {
        // Get all the current fish data; could be more optimized in the future
        int fish = -1, fishCount = 0;
        Animals[] animalInfo = gameControl.GetComponent<SpawnFish>().animalInfo;
        for (int i = 0; i < animalInfo.Length; i++) // Cycle through all the animal data
        {
            if (animalInfo[i].GetWaypointSpawn((int)currentWaypoint) && gameControl.GetComponent<SpawnFish>().fishes[i, 0] != null) // Checking if the fish is currently spawned
            {
                fish = i;
                fishName = animalInfo[fish].GetName();
                speed = animalInfo[fish].GetSpeed();
                for (int j = 0; j < gameControl.GetComponent<SpawnFish>().max; j++)
                {
                    if (gameControl.GetComponent<SpawnFish>().fishes[fish, j] != null)
                        fishCount++;
                    else
                        break;
                }
                groupSize = fishCount;
                currentSchoolLocation = gameControl.GetComponent<SpawnFish>().fishes[fish, 0].transform.position;
                relativeDistance = currentLocation - currentSchoolLocation;
                heading = gameControl.GetComponent<SpawnFish>().fishes[fish, 0].transform.rotation.eulerAngles;
                fishText =
                    "FISH STATS\n" +
                    "Fish Name: " + fishName + "\n" +
                    "Group Size: " + groupSize + "\n" +
                    "Speed: " + speed + "\n" +
                    "Current Location: " + Mathf.Round(currentSchoolLocation.x * 100) / 100 + ", " + Mathf.Round(currentSchoolLocation.y * 100) / 100 + ", " + Mathf.Round(currentSchoolLocation.z * 100) / 100 + "\n" +
                    "Heading: " + heading.x + ", " + heading.y + ", " + heading.z + "\n"+
                    "Relative Distance: " + Mathf.Round(relativeDistance.x * 100) / 100 + ", " + Mathf.Round(relativeDistance.y * 100) / 100 + ", " + Mathf.Round(relativeDistance.z * 100) / 100 + "\n";

            }
        }
        if (fish == -1)
            fishText = "No Fish Found";
    }

    void incD () // This coorelates to the current page that the debug mode is one
    {
        clickD++;
        if (clickD > 2)
            clickD = 0;
        if (clickD == 0)
            textComm.SetActive(false);
        else
            textComm.SetActive(true);
        bg.SetActive(textComm.activeSelf);
    }

    public void TurnOffDebugMode() { bg.SetActive(false); clickD = 0; } // Function not currently used, but can be to turn off the debug mode; could be referenced by other UI components to turn off the debug mode before displaying theres
}
