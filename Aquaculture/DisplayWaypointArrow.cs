/*
 * Name: Display Waypoint Arrow (DisplayWaypointArrow.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to display the somewhat sideways arrow under the current aquaculture target to be sampled. Could be modified with old code from waypoint survey to add a bounce.
 * Issues: Waypoint arrow is tilted for some reason
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWaypointArrow : MonoBehaviour
{
    public GameObject waypointPrefab;

    // References to the gamecontrol and the in scene waypoint arrow (only needs to be spawned once and then moved)
    private GameObject gameControl, waypointArrow;
    private int currentInst = 0;
    private bool ftime = true;
    // Start is called before the first frame update
    void Start()
    {
        gameControl = GameObject.FindGameObjectWithTag("GameController");
        // Instantiate the arrow at the start of the game (before the flight) and hide it
        waypointArrow = Instantiate(waypointPrefab, Vector3.zero, new Quaternion(0f, 0f, 0f, 0f));
        waypointArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameControl.GetComponent<ManageScene>().GetActiveComponent() == 3) // Currently on the sampling scene
        {
            if (ftime)
                ChangeWaypointArrow(); // Move the waypoint arrow the first time
            ftime = false;
        }
    }

    public void ChangeWaypointArrow() { // Function used to move the waypoint arrow to its current position
        waypointArrow.SetActive(true);
        // Get current aquaculture location and add 15 to its height
        int currentAqua = gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID();
        Vector3 loc = gameControl.GetComponent<RotatingFish>().GetAquacultureLocation(currentAqua);
        loc.y = 15f;
        waypointArrow.transform.position = loc;
        waypointArrow.transform.rotation = Quaternion.EulerAngles(0f, 0f, 90f); // Rotation seems unaffected, unfixable bug
        waypointArrow.transform.localScale = Vector3.one * 60f;
    }

    // Function mainly used by the diver sampling to turn off the arrow when theyre sampling
    public void WaypointViewStatus(bool status) { waypointArrow.SetActive(status); }
}
