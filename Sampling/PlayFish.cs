/*
 * Name: Play Fish (PlayFish.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-07-23
 * Last Modified: 2020-09-06
 * Used in: Sampling
 * Description: Controls the movement of the fish, the fish spawned, as well as the data about each fish 
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Profiling;

public class PlayFish : MonoBehaviour
{
    [Header("Fish Control")] // Fish control
    public float horizontalSpeed = 2.0F, numOfFishToSpawn = 5f;

    [Header("Testing aids")] // Testing aids to ease testing and trails
    public bool holdPosition = true, disableV = false, disableH = false;
    private bool reachedPosition = false, reachedHeight = false;
    private bool readyToPlay = false, finishedSampling = false;

    [Header("Fish Statistics")] // Fish stats
    public Vector2 sizeChange;
    private float[] weight;

    [Header("Draggable/Customizable Parents")]
    public GameObject[] fishes;
    public GameObject fishLocationsParent, fishParent, cameraParent;
    [HideInInspector] public GameObject[] fish, fishPositions;
    private bool disableRotation = false;

    [Header("End Screen")]
    public GameObject endScreen, UIParent;

    private int curFish = 0, curLocation = 0;
    private bool[] donePrev;

    private Vector3 sLoc;
    private Vector3[] startRot;

    private float numOfTimesPressed = 0f;
    void Start()
    {
        endScreen.SetActive(false); // Turn off the end screen

        // Get all the fish location gameobjects
        Transform[] childrenCount = fishLocationsParent.transform.GetComponentsInChildren<Transform>();
        fishPositions = new GameObject[childrenCount.Length];
        for (int i = 0; i < childrenCount.Length - 1; i++)
            fishPositions[i] = fishLocationsParent.transform.GetChild(i).gameObject;

        // Ensure that the number of fish is less than or equal to the number in the array
        if ((int)numOfFishToSpawn > fishes.Length)
            numOfFishToSpawn = fishes.Length;
        fish = new GameObject[(int)numOfFishToSpawn];
        int[] fishToSpawn = PickFish(); // Pick all the fish, so each is unique
        startRot = new Vector3[fish.Length]; // Get the starting rotation
        weight = new float[fish.Length]; // Generate weight
        donePrev = new bool[fish.Length];
        for (int i = 0; i < fish.Length; i++)
            donePrev[i] = false;
        for (int i = 0; i < fishToSpawn.Length; i++)
        {
            Vector3 spawnLocation = fishPositions[0].transform.position;
            StartCoroutine(Wait(spawnLocation, i, fishToSpawn[i]));
        }
        sLoc = transform.position;
        Cursor.visible = false; // Turn off lock cursor to make user movement easier 
        Cursor.lockState = CursorLockMode.Locked;
        if (UIParent == null)
            UIParent = GameObject.FindGameObjectWithTag("UI Parent");
    }

    IEnumerator Wait(Vector3 spawnLocation, int i, int fTS) // Function to space out spawning of fish so they don't explode, but fall in naturally
    {
        if(i != 0)
            while (!donePrev[i - 1]) // Ensure previous fish is spawned in
                yield return new WaitForSeconds(0.05f);

        fish[i] = Instantiate(fishes[fTS], spawnLocation, Quaternion.Euler(0f, 0f, 0f), fishParent.transform); // Spawn proper fish in
        startRot[i] = fish[i].transform.eulerAngles;
        float actSize = Random.Range(sizeChange.x, sizeChange.y);
        fish[i].transform.localScale *= actSize;
        weight[i] = (0.012f * (Mathf.Pow(70f * actSize, 2.958f)))/1000f; // insert equation here to reference weight to size
        readyToPlay = (i + 1) >= numOfFishToSpawn;
        yield return new WaitForSeconds(0.05f);
        donePrev[i] = true; // Mark that the current fish is spawned to signal next one to spawn
    }


    void Update()
    {
        if(readyToPlay && !finishedSampling) // Ensure that sampling isn't finished
        {
            disableRotation = curLocation == 3; // Turn off rotation when you're currently at the ruler/measurement station

            // Check to see if they've moved to a new location
            int prevLocation = curLocation;
            curLocation = this.GetComponent<MoveCamera>().GetCurrentPosition();
            fish[curFish].GetComponent<Rigidbody>().useGravity = reachedPosition;
            if (prevLocation != curLocation)
            {
                if (curLocation == 0) // When at first bucket, height has to be above table to make it easier to get to its correct location
                    reachedHeight = false;
                reachedPosition = false;
            }

            if (curLocation != 0 && !reachedPosition) // If they're not at the starting bucket and haven't reached their position
            {
                if (Mathf.Approximately(fish[curFish].transform.position.y, 1f) || fish[curFish].transform.position.y > 0.9f)
                    reachedHeight = true;
                if (!reachedHeight)
                {
                    Vector3 floatingLoc = fish[curFish].transform.position;
                    floatingLoc.y = 1f;
                    fish[curFish].transform.position = Vector3.MoveTowards(fish[curFish].transform.position, floatingLoc, Time.deltaTime * this.GetComponent<MoveCamera>().CameraMoveSpeed()); // Move towards the height above the table
                }
                else
                    fish[curFish].transform.position = Vector3.MoveTowards(fish[curFish].transform.position, fishPositions[curLocation].transform.position, Time.deltaTime * this.GetComponent<MoveCamera>().CameraMoveSpeed()); // Move towards the target location
            }

            if(curLocation == 1) // If they're at the counting lice position, allow them to reset the rotation to make sampling easier
            {
                if (Input.GetKeyDown(KeyCode.O))
                {
                    Vector3 rot = fish[curFish].transform.rotation.eulerAngles;
                    rot.y = 180f;
                    fish[curFish].transform.rotation = Quaternion.Euler(rot);
                }
            }

     /*       if (curLocation != 0 || curLocation != 4)
                fish[curFish].transform.rotation = Quaternion.EulerAngles(0f, 90f, 0f);
                */
            if (!reachedPosition) // If the fish haven't reached their next station
            {
                if (fish[curFish].transform.position == fishPositions[curLocation].transform.position) // Check to see if its reached
                {
                    reachedPosition = true;
                    if(curLocation == 1) // Reset rotation when it reaches the counting lice station
                        ResetRotation();
                    else if(curLocation == 3) // Set rotation and disable to make it easier to measure the fish
                    {
                        Quaternion rot = new Quaternion(-0.473811865f, -0.528679848f, -0.452342451f, -0.539802074f);
                        fish[curFish].transform.rotation = rot;
                        disableRotation = true;
                    }
                }
            }
            if (holdPosition)
                transform.position = sLoc;
            float h = horizontalSpeed * Input.GetAxis("Mouse Y");

            if (!disableRotation && !(curFish >= fish.Length) && (curFish != -1)) // Ensure that rotation is enabled
                SetRotation(h);

            if (Input.GetKeyDown("space") && !(curFish >= fish.Length) && curFish != -1 && curLocation != 3) // Reset rotation
                ResetRotation();
            if (reachedPosition && curLocation != 4 && curLocation != 3) // Freeze the position when it reaches the correct position
                fish[curFish].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            else if(curLocation != 3)
                fish[curFish].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            if (curLocation == 3) // If the current location is measurement 
            {
                // Turn on and off rotation to prevent lag movement/rotation
                fish[curFish].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                Quaternion rot = new Quaternion(-0.473811865f, -0.528679848f, -0.452342451f, -0.539802074f); // Hold rotation to prevent movement
                fish[curFish].transform.rotation = rot;
                fish[curFish].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                // Allows user to line up the fish on the correct tick and sample it more naturally
                if (Input.GetKeyDown(KeyCode.P))
                    numOfTimesPressed++;
                else if (Input.GetKeyDown(KeyCode.O))
                    numOfTimesPressed--;
                numOfTimesPressed = Mathf.Clamp(numOfTimesPressed, -20f, 70f); // Clamp so it doesn't fall off the table
                Vector3 curPos = fishPositions[curLocation].transform.position;
                curPos.x += 0.005f * numOfTimesPressed; // Move by the set factor
                fish[curFish].transform.position = curPos;
                fish[curFish].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition; // Freeze the position
            }

        }
        else if(!finishedSampling) // If fish are still being spawned in
        {
            print("Still loading in fish....");
        }
        else // Once they've finished sampling
            if(Input.GetKeyDown(KeyCode.C)) // Copy to clipboard
                CopyToClipboard(UIParent.GetComponent<Sampler_UIManager>().GetAllData((int)numOfFishToSpawn));
    }

    void SetRotation(float h) // Set rotation function
    {
        fish[curFish].transform.RotateAround(fish[curFish].transform.position, Vector3.up, 20 * h);
    }

    void ResetRotation() // Function to reset rotation so its parallel to the table
    {
        fish[curFish].transform.rotation = new Quaternion(0.035589695f, -0.717719555f, -0.0276653506f, 0.694871724f);
    }

    public int NextFish() // Function to cycle to the next fish
    {
        disableRotation = false; // Make sure rotation is enabled
        numOfTimesPressed = 0f; // Reset movement on the measurement
        if (curFish == -1)
            disableRotation = false;
        else
            fish[curFish].transform.position = fishPositions[fishPositions.Length - 2].transform.position;
        if ((curFish + 1) == fish.Length) // If they've reached the end, begin the end sequence
            EndScene();
        else
            curFish++;
        return 1;
    }

    public void EndScene() // End scene function
    {
        finishedSampling = true; // Finish sampling
        endScreen.SetActive(true); // Enable end screen
        CopyToClipboard(UIParent.GetComponent<Sampler_UIManager>().GetAllData((int)numOfFishToSpawn)); // Copy all the data to the clipboard
    }

    private int[] PickFish() // Function to pick the fish
    {
        int[] pickedFish = new int[(int)numOfFishToSpawn];
        for(int i = 0; i < pickedFish.Length; i++) // Cycle through the number of fish needed to be spawn
        {
            bool notChosen = false;
            while(!notChosen)
            {
                notChosen = true;
                pickedFish[i] = Random.Range(0, fishes.Length); // Pick a random array instance in the fish prefabs
                for (int j = 0; j < i; j++) // Make sure the fish hasn't previously been picked, if so - generate another one
                    if (pickedFish[i] == pickedFish[j])
                        notChosen = false;
            }
        }
        return pickedFish;
    }

    public static void CopyToClipboard(string s) // Function to copy data to the clipboard
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    public int GetCurrentFishNumber() { return curFish; } // Return the current fish being sampled
    public float GetCurrentWeight() { return weight[curFish]; } // Return the weight of the current fish
    public float GetFishWeight(int inst) { return weight[inst]; }  // Return the weight of a specific instance
    public GameObject GetCurrentFishPrefab() { return fish[curFish].gameObject; } // Return the reference to the object of the current fish
    public int GetNumberOfFish() { return (int)numOfFishToSpawn; } // Return the number of fish to be spawned
    public GameObject GetSpecificFish(int fishNum) { if (fishNum >= fish.Length) return null;  return fish[fishNum]; } // Return the reference to a specific fish object
    public bool EndSceneActive() { return finishedSampling; } // Return whether or not they've finished sampling
}
