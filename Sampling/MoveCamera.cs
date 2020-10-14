/*
 * Name: Move Camera (MoveCamera.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-03
 * Last Modified: 2020-09-06
 * Used in: Sampling
 * Description: Controls the movement of the camera between the different positions based on sampling. To add new positions, simply add a new gameobject under the camera positions parent. Also controls tracking where the user currently is. Ensure that the number 
 * of objects under Camera Positions cooresponds to the number under fish.
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // Reference to various components; camera parent is the parent of all the camera positions
    public GameObject cameraParent, gameControl, UIParent;

    // Array to store references to the different camera positions
    private GameObject[] cameraPositions;

    // Settings regarding the move speed and rotation. Time at buckets is the default time it'll spend at the first and last bucket before moving to the next station. The user can override this by simply pressing R
    public float cameraMoveSpeed = 10f, cameraRotateSpeed = 20f, timeAtBuckets = 2f;

    // The amount of time at the current station
    private float sTime = 0f;

    public int currentCameraPosition = 0;
    public bool reachedRotating = false;

    private bool endActive = false; // Keeps track of if they've finished sampling

    void Start()
    {
        // Get all the camera positions
        Transform[] childrenCount = cameraParent.transform.GetComponentsInChildren<Transform>();
        cameraPositions = new GameObject[childrenCount.Length];
        for (int i = 0; i < childrenCount.Length - 1; i++)
            cameraPositions[i] = cameraParent.transform.GetChild(i).gameObject;

        if (UIParent == null)
            UIParent = GameObject.FindGameObjectWithTag("UI Parent");
    }

    void Update()
    {
        if(!endActive) // Check to make sure sampling isn't done
        {
            endActive = gameControl.GetComponent<PlayFish>().EndSceneActive(); // Read in end scene

            // Move and rotate towards the correct position
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraPositions[currentCameraPosition].transform.position, cameraMoveSpeed * Time.deltaTime); // accounts for computer lag
            if (!reachedRotating)
                Camera.main.transform.rotation = Quaternion.RotateTowards(Camera.main.transform.rotation, cameraPositions[currentCameraPosition].transform.rotation, cameraRotateSpeed * Time.deltaTime);

            // If they've reached the correct rotation, disable the rotation lock so that they can move freely using the arrow keys
            if (Camera.main.transform.rotation == cameraPositions[currentCameraPosition].transform.rotation && !reachedRotating)
            {
                sTime = Time.time;
                reachedRotating = true;
            }

            if (Input.GetKeyDown(KeyCode.R)) // Record data confirmed
            {
                if(UIParent.GetComponent<Sampler_UIManager>().FinishedRecordingData()) // Ensure that they've actually recorded something; only actually used in the ruler measurement - prevents a 0 meausrement
                {
                    if (currentCameraPosition == 4) // If theyre at the end; reset to the beginning and spawn a new fish
                    {
                        currentCameraPosition = 0; 
                        gameControl.GetComponent<PlayFish>().NextFish();
                    }
                    else 
                        currentCameraPosition++;
                    sTime = Time.time;
                    reachedRotating = false;
                }
            }
            if ((Time.time - sTime) >= timeAtBuckets && reachedRotating) // Check to see if the max time at the buckets has elasped
            {
                if (currentCameraPosition == 0) // If its the beginning go up one
                {
                    reachedRotating = false;
                    currentCameraPosition++;
                }
                else if (currentCameraPosition == cameraPositions.Length - 2) // if at end, spawn new fish
                {
                    currentCameraPosition = 0;
                    reachedRotating = false;
                    gameControl.GetComponent<PlayFish>().NextFish();
                }
            }
        }     
    }

    public int GetCurrentPosition() { return currentCameraPosition; } // Return current sampling position
    public void SetCurrentPosition(int toSet) { currentCameraPosition = toSet; }  // Set the current sampling position
    public float CameraMoveSpeed() { return cameraMoveSpeed; } // Get camera move speed (used as reference for how fast fish should move)
}
