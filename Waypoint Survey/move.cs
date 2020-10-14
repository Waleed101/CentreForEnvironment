/*
 * Name: Move (move.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-07
 * Last Modified: 2020-09-06
 * Used in: Waypoint Survey, Aquaculture
 * Description: Script used to control the movement and rotation of the plane during the flight. 
 * Status: PRODUCTION
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3; // With classes included, need to specify this to make sure right functionality
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class move : MonoBehaviour
{
    public int curWaypoint = 0;

    // References to various gameobjects used in the script; turbines are everything rotating (left and right) in an array and startScreen is the launch screen prior to the user pressing P
    public GameObject gameControl, aircraft, startScreen;
    public GameObject[] turbines;
    public Vector3 currentWaypoint, prevWaypoint, beginning;
    public bool going = true, pointingAt = true, animateTurn = true, delayedStart = false, pressedP = false;
    // Various in flight settings, sTakeOffEnd and sLandingEnd are indictors of when to change the flight speed to the faster takeOffSpeed. Coorelates to the waypoint ID
    public float takeOffSpeed = 100f, flightSpeed = 50f, timer = 0f, angle = 0f, turbineSpeed, sTakeoffEnd, sLandingEnd, timeToWait = 7.5f; // Can modify how fast it moves here or in Unity
    public float moveSpeed = 0f;


    void Start()
    {
        NextWaypoint();
        moveSpeed = takeOffSpeed;
        if(going)
            transform.position = currentWaypoint; // Don't fly to start position, just teleport there.
        if(delayedStart)
            StartCoroutine(DelayedStartForFish());
    }
    IEnumerator DelayedStartForFish()
    {
        yield return new WaitForSeconds(timeToWait);
        delayedStart = false;
    }

    void Update()
    {
        if(!delayedStart && pressedP)
        {
            for (int i = 0; i < turbines.Length; i++)
                turbines[i].transform.RotateAround(turbines[i].transform.position, turbines[i].transform.forward, Time.deltaTime * turbineSpeed);
            if (going)
            {
             //   transform.rotation = Quaternion.RotateTowards(transform.rotation, gameControl.GetComponent<WaypointManagement>().GetCurrentWaypointGM().transform.rotation, moveSpeed * Time.deltaTime);
                if (pointingAt) // Ensure that the plane is actually pointing at the waypoint before flying towards it
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, Time.deltaTime * moveSpeed); // Move from current position towards waypoint, Time.deltaTime is to adjust for latency between computers
                else
                {
                    if (animateTurn) // can be set whether or not to spin in the inspector
                    {
                        Vector3 newDir = Vector3.RotateTowards(transform.forward, currentWaypoint, Time.deltaTime, 0.0f); // Create a euler rotation vector of turning between the current rotation and where the next waypoint is
                        transform.rotation = Quaternion.LookRotation(newDir, Vector3.up); // Make the rotation happen
                                                                                          // Everything below is to determine if its pointing at the new location
                        Vector3 dirFromAtoB = (currentWaypoint - this.transform.position).normalized;
                        float dotProd = Vector3.Dot(dirFromAtoB, this.transform.forward);
                        if (dotProd >= 0.5f) // Threshold can be changed the modify the accuracy or the senstivity of the turn based on the current route
                        {
                            Vector3 rotating = transform.rotation.eulerAngles;
                            pointingAt = true;
                            rotating = transform.rotation.eulerAngles;
                            rotating.z = 0f;
                            transform.LookAt(currentWaypoint);
                        }
                    }
                    else // if not, just look at the next waypoint
                    {
                        transform.LookAt(currentWaypoint);
                        pointingAt = true;
                    }

                }
                if (transform.position == currentWaypoint) // If its reached the target waypoint, move onto the next one
                {
                    going = NextWaypoint();
                    pointingAt = false;
                }
            }
        }
        if (!pressedP) // Enable/disable the launch screen with the press of P
            if (Input.GetKeyDown(KeyCode.P))
                SetP();
    }

    bool NextWaypoint() // Waypoint function to reach out to the WaypointMagement script and ask for the next waypoints coordinates
    {
        curWaypoint++;
        if (curWaypoint <= sTakeoffEnd || curWaypoint >= sLandingEnd)
            moveSpeed = takeOffSpeed;
        else
            moveSpeed = flightSpeed;
       // moveSpeed = 500f; //testing purposes ONLY
        prevWaypoint = currentWaypoint;
        currentWaypoint = gameControl.GetComponent<WaypointManagement>().NextWaypoint();
        if (currentWaypoint.Equals(Vector3.zero))
            return false;
        return true;
    }

    void SetP()
    {
        pressedP = true;
        startScreen.SetActive(false);
    }

    public Vector3 GetLocation() { return transform.position; } // Function to get current location
    public Vector3 GetAngle() { return transform.rotation.eulerAngles; } // Function to get current rotation
    public Vector3 GetTargetWaypoint() { return currentWaypoint; } // Function to get current waypoint location
    public float GetMovementSpeed() { return moveSpeed; } // Function to get flight speed of the plane
    public float GetTurbineSpeed() { return turbineSpeed; } // Function to get the animation speed of the turbines
    public float GetCurrentWaypointID() { return curWaypoint; } // Function to get the current waypoint ID

    public void SetGoing() { going = !going; } // Function to enable/disable the flight

    public bool IsPPressed() { return pressedP; } // Function to get whether or not "P" has been pressed and the flight has started
}