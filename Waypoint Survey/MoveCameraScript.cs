/*
 * Name: Move Camera Script (MoveCameraScript.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-11
 * Last Modified: 2020-09-06
 * Used in: Waypoint Survey, Aquaculture
 * Description: Controls the movement of the camera using the arrows, as well as the locations of which the camera can switch between. User can control location by clicking C or numbers 1-9.
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoveCameraScript : MonoBehaviour // Script to control camera movement
{
    public Camera firstPerson; // Stores both cameras; first person sits on the left wing
    public GameObject binoculars, cameraParent, gameControl; // Reference to various 
    private GameObject[] cameraPositions;
    public int startLocation = 1, curCameraLocation = 0;

    // Camera max/min bounds for rotation
    public float cameraBoundsXLow = 70f, cameraBoundsXHigh = 290f, cameraBoundsYLow = 160f, cameraBoundsYHigh = 360f;

    // Whether or not the bounds and binoculars are active
    public bool cameraBoundsActive = true, binocularsActive = true;
    private int currentImage = 0, mult = 0;
    Vector3 startPos;

    // Default zoom, binocular zoom and the binocular change for zoom
    public float normalZoom = 70f, binocularsZoom = 50f, binocularsChange = 5f, rotateSpeed = 100f;
    private float inputX, inputZ; // To store the user input, as well as the speed of the rotation
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    void Start()
    {
        firstPerson.enabled = true; // Enabling the cameras as needed
        if(binocularsActive) // Turn off the binoculars
            binoculars.SetActive(false);
        firstPerson.fieldOfView = normalZoom;
        startPos = transform.position;

        // Get all the camera positions
        Transform[] childrenCount = cameraParent.GetComponentsInChildren<Transform>();
        cameraPositions = new GameObject[childrenCount.Length];
        for (int i = 0; i < childrenCount.Length - 1; i++)
            cameraPositions[i] = cameraParent.transform.GetChild(i).gameObject;

        // Switch the camera to the start location set in inspector
        SwitchCamera(startLocation);
        curCameraLocation = startLocation;

        if (gameControl == null)
            gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        transform.position = startPos;
        inputX = Input.GetAxis("Horizontal"); // Taking in the camera rotation input
        inputZ = Input.GetAxis("Vertical") * -1f;
        if (inputX != 0)
            rotateX(); // If there is input, rotate accordingly
        if (inputZ != 0)
            rotateZ();
        Vector3 rot = firstPerson.transform.rotation.eulerAngles;
        if(rot.z != 0f) // Rotate accordingly
            firstPerson.transform.eulerAngles = new Vector3(firstPerson.transform.rotation.eulerAngles.x, firstPerson.transform.rotation.eulerAngles.y, 0f);
        if(rot.x > cameraBoundsXLow && rot.x < cameraBoundsXHigh && cameraBoundsActive) // Make sure that the rotation is between camera bounds; bugs with rotation vibrating, so clamps are disabled
        {
            if(rot.x - 10f < cameraBoundsXLow)
                firstPerson.transform.eulerAngles = new Vector3(cameraBoundsXLow, firstPerson.transform.rotation.eulerAngles.y, 0f);
            else
                firstPerson.transform.eulerAngles = new Vector3(cameraBoundsXHigh, firstPerson.transform.rotation.eulerAngles.y, 0f);
        }
        if (rot.y > cameraBoundsYLow && rot.y < cameraBoundsYHigh && cameraBoundsActive)  // Make sure that the rotation is between camera bounds; bugs with rotation vibrating, so clamps are disabled
        {
            if (rot.y - 10f < cameraBoundsYLow)
                firstPerson.transform.eulerAngles = new Vector3(firstPerson.transform.rotation.eulerAngles.x, cameraBoundsYLow, 0f);
            else
                firstPerson.transform.eulerAngles = new Vector3(firstPerson.transform.rotation.eulerAngles.x, cameraBoundsYHigh, 0f);
        }
        if (Input.GetKeyUp(KeyCode.B)) // Enabling binoculars
        {
            if (this.GetComponent<WaypointManagement>().GetFlightStatus()) // Ensure that the plane is currently flying
            {
                binoculars.SetActive(!binoculars.activeSelf);
                mult = 0;
            }
        }
        if (binoculars.activeSelf) // If the binculars are active, allow cycling of zoom
        {
            for (int i = 0; i < keyCodes.Length; ++i)
                if (Input.GetKeyDown(keyCodes[i])) // Cycling through all the different numbers to see what they've pressed
                    mult = i;
            firstPerson.fieldOfView = binocularsZoom - binocularsChange * mult;
        }
        else // If not, cycle through the camera locations
        {
            if (Input.GetKeyUp(KeyCode.C) && gameControl.GetComponent<WaypointManagement>().GetFlightStatus()) // Switch camera location
            {
                curCameraLocation++;
                SwitchCamera(curCameraLocation);
            }
            for (int i = 0; i < keyCodes.Length; ++i)
            {
                if (Input.GetKeyDown(keyCodes[i])) // Cycling through all the different numbers to see what they've pressed
                {
                    curCameraLocation = i; // Set the camera to the correct location
                    SwitchCamera(curCameraLocation);
                }
            }
            firstPerson.fieldOfView = normalZoom; // Set the camera to the default zoom
        }

        // Old functionality to allow screenshots, not optimized for WebGL
        /* if (Input.GetKeyUp(KeyCode.Z))
         {
             ScreenCapture.CaptureScreenshot("Screencaptures/capture" + currentImage + ".png");
             currentImage++;
         }*/
    }

    private void rotateX() { firstPerson.transform.Rotate(new Vector3(0f, inputX * Time.deltaTime * rotateSpeed * (firstPerson.fieldOfView / 60f), 0f)); }

    private void rotateZ() { firstPerson.transform.Rotate(new Vector3(inputZ * Time.deltaTime * rotateSpeed * (firstPerson.fieldOfView/60f), 0f, 0f)); }

    private void SwitchCamera(int dir) // Switch camera location
    {
        if (dir == 0) // Ensure that its not 0
            dir = 10;
        if (dir > cameraPositions.Length-1) // Ensure that its not larger than the number of camera positions
        {
            dir = 1;
            curCameraLocation = 1;
        }
        firstPerson.transform.position = cameraPositions[dir - 1].transform.position;
    }

    public void TurnOffBinoculars() { if (binocularsActive) { binoculars.SetActive(false); } } // Disable binoculars; used mainly when other UI is opened
}
