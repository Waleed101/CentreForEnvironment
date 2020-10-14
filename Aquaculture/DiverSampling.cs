/*
 * Name: Diver Sampling (DiverSampling.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to both display the sampling device (including whats on the device, text wise) and to kepe track of the data sampled at each location
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DiverSampling : MonoBehaviour
{
    // References to the different UI components and gameobjects used in this scene (names are pretty self explanatory) - pHComm and mtcComm are the first and second numbers respectively on the device
    public GameObject samplingCam, swimmingCam, samplingDevice, swimmingDevice, gameControl, notificationUI, pHComm, mtcCOMM, aquacultureParent;

    // Lists tracking the recorded pHData and aquaID
    private List<float> pHData = new List<float>();
    private List<int> aquaID = new List<int>();

    // User set minimum and maximum concentration used to randomize the pH; can be set in inspector mode
    public float minConcentration = 5f, maxConcentration = 10f, oxyMin = 50f, oxyMax = 70f;
    private int currentAqua = -1;

    void Start()
    {
        if(gameControl == null)
            gameControl = GameObject.FindGameObjectWithTag("GameController");

        if (notificationUI == null)
            notificationUI = GameObject.FindGameObjectWithTag("Notification_UI");

        ChangeMode(true); // Enable swimming mode
    }

    void Update()
    {
        if(gameControl.GetComponent<SelectRoute>().isDone()) // If finished sampling, enable copy to clipboard of data and display the end screen
        {
            GameObject.FindGameObjectWithTag("End").transform.GetChild(0).gameObject.SetActive(true);
            if(Input.GetKeyDown(KeyCode.C))
                CopyToClipboard(GetData());
        }
        if(Input.GetKeyDown(KeyCode.R)) // R is the key they press to sample
        {
            if(notificationUI.transform.GetChild(0).gameObject.activeSelf) // Make sure that theyre currently under the current sampling aquaculture
            {
                ChangeMode(false); // Enable sampling mode, disable swimming
                currentAqua++; // Change to the next aqua, if first itll become the first aqua
                GameObject.FindGameObjectWithTag("Notification_UI").transform.GetChild(0).gameObject.SetActive(false); // Turn off the notification about current sampling
                pHData.Add(Random.Range(minConcentration, maxConcentration)); // Get a random concentration based on the ranges provided, add it to the list of data recorded
                aquaID.Add(gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID()); // Add the aquaculture to the list of data recorded
                
                // Set locaiton to center to make looking at the fish better
                Vector3 loc = gameControl.GetComponent<RotatingFish>().GetAquacultureLocation(gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID());
                loc.y = 36.2f;
                this.transform.position = loc;
            }
            else // Use in-house error system to display an error about them needing to be at the right aquaculture
                gameControl.GetComponent<ErrorHandeling>().DisplayError("Please find the aquaculture before entering sampling mode.", 5f);
        }

        if (samplingCam.activeSelf) // Check to see if sampling is currently active
        {
            if(Input.GetKeyDown(KeyCode.Return)) // If they click enter (which cooresponds to sample), sampling mode is exited
            {
                ChangeMode(true);
                gameControl.GetComponent<SelectRoute>().NextAquaculture();
            }

            // Set the UI components of the device to the correct values
            pHComm.GetComponent<TextMeshPro>().text = pHData[currentAqua].ToString("F2");
            mtcCOMM.GetComponent<TextMeshPro>().text = GetMTC(pHData[currentAqua]).ToString("F2"); 
            // 280, 82
            if (Input.GetAxis("Vertical") != 0f)
                samplingCam.transform.Rotate(new Vector3(Input.GetAxis("Vertical") * Time.deltaTime * 40f * -1f, 0f, 0f));

            Vector3 rot = samplingCam.transform.rotation.eulerAngles;
            //rot.x = Mathf.Clamp(rot.x, -100f, 82f);
            if (rot.x < 275 && rot.x > 100)
                rot.x = 275;
            else if (rot.x > 80 && rot.x < 265)
                rot.x = 80;
            samplingCam.transform.rotation = Quaternion.Euler(rot);
        }
    }

    // Function to get the second value on the device
    public float GetMTC(float ph) { return Random.Range(oxyMin, oxyMax); }

    // Function to concate the string to be copied to the users clipboard
    public string GetData()
    {
        string s = "Sampling ID, Net #, pH, Oxygen Saturation\n";
        for(int i = 0; i <= currentAqua; i++)
            s += (i + 1) + ", " + aquaID[i] + ", " + pHData[i] + ", " + GetMTC(pHData[i]) + "\n";
        return s;
    }

    // Function to copy the data to the clipboard
    public void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    // Function to change the mode to either swimming or sampling
    void ChangeMode(bool swimming)
    {
        // Swimming system:
        this.GetComponent<ThirdPersonSwimming>().swimmingAnimation = swimming;
        this.GetComponent<ThirdPersonSwimming>().allowSwim = swimming;
        aquacultureParent.GetComponent<DisplayWaypointArrow>().WaypointViewStatus(swimming);
        swimmingCam.SetActive(swimming);
        swimmingDevice.SetActive(swimming);

        // Sampling system:
        samplingCam.SetActive(!swimming);
        samplingDevice.SetActive(!swimming);
    }
}
