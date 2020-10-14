using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
//using UnityEngine.Debug as Debug;

public class WaypointManagement : MonoBehaviour // Script to manage which waypoint the plane should be going to next, as well to tell the fish to spawn at the waypoint
{
    public int currentWaypoint = -1, route = 0;
    public bool activeFlight = true, spawnFish = false, spawnObjects = false, playMusic = false, aquaculture = true;
    private bool ableToGo = false;
    [HideInInspector] private GameObject[] waypoints; // Array to store all the waypoint
    public GameObject waypointParent, endMenu, recordData, debugData;

    void Start()
    {
        SetupWaypoints();
    }

    public void SetupWaypoints()
    {
        Transform[] childrenCount = waypointParent.transform.GetChild(route).GetComponentsInChildren<Transform>();
     //   print(waypointParent.transform.GetChild(1).transform.position);
        waypoints = new GameObject[childrenCount.Length];
        for (int i = 0; i < childrenCount.Length - 1; i++)
            waypoints[i] = waypointParent.transform.GetChild(route).GetChild(i).gameObject;
        print(childrenCount.Length);
        ableToGo = true;
        DrawRoute();
    }

    public void DrawRoute()
    {
        if(waypoints.Length > 1)
        {
          //  print(waypoints.Length);
            for (int i = 0; i < waypoints.Length - 2; i++)
            {
              //  print(i);
                Debug.DrawLine(waypoints[i].transform.position, waypoints[i + 1].transform.position, Color.black, 900f);
            }
            Debug.DrawLine(waypoints[waypoints.Length - 2].transform.position, waypoints[0].transform.position, Color.black, 900f);
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            if (GetFlightStatus())
            {
           /*     if(recordData.GetComponent<RecordData>().GetNumberRecorded() > 0)
                {
                    endMenu.SetActive(!endMenu.activeSelf);
                    endMenu.GetComponent<FinalManager>().SetFtime(true);
                }*/
            }
        }
    }

    public Vector3 NextWaypoint() // Navigating to the next waypoint
    {
        while (!ableToGo) { SetupWaypoints(); }
      /*  if(waypoints.Length == 1)
            this.GetComponent<ErrorHandeling>().DisplayError("No waypoints stored.", 20f);
       */ currentWaypoint++; 
        if (currentWaypoint >= waypoints.Length-1 && waypoints.Length != 1) // If reached the end, just return an empty vector
        {
          //  this.GetComponent<ErrorHandeling>().DisplayError("Finished flight!", 5f);
            activeFlight = false;
            print("end of flight");
            if (aquaculture)
                this.GetComponent<ManageScene>().SetActiveComponent(2);
            else
                recordData.GetComponent<RecordDataORG>().FinishedFlight();
            endMenu.SetActive(true);
            this.GetComponent<MoveCameraScript>().TurnOffBinoculars();
            return new Vector3(0f, 0f, 0f);
        }
        if(spawnFish)
            this.GetComponent<SpawnFish>().SpawnWaypointFish(currentWaypoint); // Tell the fish to spawn
        if(playMusic)
            this.GetComponent<AudioManager>().NextWaypoint(currentWaypoint);
        return waypoints[currentWaypoint].transform.position;
    }

    public int GetCurrentWaypont() { return currentWaypoint; }
    public GameObject[] GetWaypointArray() { return waypoints; }
    public bool GetFlightStatus() { return activeFlight; }
    public bool GetAbleToGo() { return ableToGo; }
    public Vector3 GetCurrentWaypointLocation() { return waypoints[currentWaypoint].transform.position; }
    public GameObject GetCurrentWaypointGM() { return waypoints[currentWaypoint]; } 
    public Vector3 GetWaypointLocation(int i) { return waypoints[i].transform.position; }
    public int GetNumberOfWaypoints() { return waypoints.Length-1; }
    public GameObject GetSpecificWaypoint(int inst) { return waypoints[inst]; }
    public int GetSelectedRoute() { return route; }
}
