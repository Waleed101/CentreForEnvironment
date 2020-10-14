/*
 * Name: RecordAquaculture (RecordAquaculture.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-01
 * Last Modified: 2020-09-05
 * Used in: Aquaculture
 * Description: Script used to determine and control the behavior of the diver when they enter the bounds/collider of the aquaculture
 * Status: UNUSED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordAquaculture : MonoBehaviour
{
    public GameObject waypointIndicator, diver, diverCamera, diverParent; // References to the diver to determine when collision occurs
    private GameObject waypoint;
    private bool[] sampledHere;
    public bool allowPrevious = false, readyToChoose = true, firstTime = true;
    public int numberToSample = 1, currentToSample = 0;
    
    private Vector3 moveCamLocation = new Vector3(278f, -7.2f, 175.7f), recordCamLocation = new Vector3(278f, -14.4f, 222f);
    private Quaternion moveCamRotation = Quaternion.Euler(-5f, 0f, 0f), recordCamRotation = Quaternion.Euler(55f, 0f, 0f); 

    void Start()
    {
        sampledHere = new bool[(int)this.GetComponent<RegionControl>().GetNumberOfAquacultures()];
        Camera.main.transform.position = moveCamLocation;
        Camera.main.transform.rotation = moveCamRotation;
    }

    void Update()
    {
        readyToChoose = this.GetComponent<RegionControl>().CompletedSetup();
        if((Input.GetKeyUp(KeyCode.Backspace) || firstTime) && readyToChoose) {
            Destroy(waypoint);
            firstTime = false;
            SelectRandomAquaculture();
            Vector3 pos = this.GetComponent<RegionControl>().GetAquaculture(currentToSample).transform.position;
            pos.y = 0f;
            waypoint = Instantiate(waypointIndicator, pos, this.GetComponent<RegionControl>().GetAquaculture(currentToSample).transform.rotation);
        }
        if ((Vector3.Distance(diver.transform.position, waypointIndicator.transform.position) < 650f))
        {
            if(Input.GetKeyUp(KeyCode.Q))
            {
                diverParent.GetComponent<ThirdPersonSwimming>().DisableSwimming();
                diver.transform.position = waypointIndicator.transform.position;
                diverCamera.transform.position = recordCamLocation;
                diverCamera.transform.rotation = recordCamRotation;
            }
        }
    }

    void SelectRandomAquaculture() {
        bool notPrevious = true;
        currentToSample = Random.Range(0, sampledHere.Length-1);
        print(currentToSample);
        if (sampledHere[currentToSample] && !allowPrevious)
            notPrevious = false;
        while(!notPrevious)
        {
            notPrevious = true;
            currentToSample = Random.Range(0, sampledHere.Length);
            if (sampledHere[currentToSample] && !allowPrevious)
                notPrevious = false;
        }

        sampledHere[currentToSample] = true;
    }
}
