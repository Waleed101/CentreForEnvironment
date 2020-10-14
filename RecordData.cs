/*
 * Name: Record Data (RecordData.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Old script to record data, phased out for new data recording UI/script
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordData : MonoBehaviour
{
    public GameObject gameControl, fishName, fishImage, self, plane, waypoint, groupSize;
    public bool printToConsole = false;


    private int currWaypoint = 0, currSize = 0, id = -1, numRecorded = 0;
    private Vector3 recordLocation = Vector3.zero;
    private string appendF = "~", appendB = "";
    private bool fishSelected = false;
    private Animals[] animal;
    private KeyCode[] keyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    private int[] sampGroupSizes = new int[] { 1, 2, 5, 10, 15, 20, 30, 40, 50, 60, 75, 100, 150, 200, 400, 600, 800, 1000 };
    
    private int[] animalIDs = new int[100], waypointIDs = new int[100];
    private Vector3[] locationSpotted = new Vector3[100];
    private string[] groupSizes = new string[100];
    // Start is called before the first frame update
    void Start()
    {
        animal = gameControl.GetComponent<SpawnFish>().GetAnimals();
        self.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (self.activeSelf)
        { 
            if(Input.GetKeyDown(KeyCode.O))
            {
                id++;
                if (id >= animal.Length)
                    id = animal.Length - 1;
                ChangeFishStats(id);
            }
            if(Input.GetKeyDown(KeyCode.I))
            {
                id--;
                if (id < 0)
                    id = 0;
                ChangeFishStats(id);
            }
            if(fishSelected)
            {
                if (Input.GetKeyDown(KeyCode.L))
                    currSize++;
                else if (Input.GetKeyDown(KeyCode.K))
                    currSize--;
                if (currSize >= sampGroupSizes.Length)
                {
                    currSize = sampGroupSizes.Length - 1;
                    appendF = "";
                    appendB = "+";
                }
                if(currSize < sampGroupSizes.Length-1)
                {
                    appendF = "~";
                    appendB = "";
                    if (currSize < 0)
                        currSize = 0;
                }
                groupSize.GetComponent<TextMeshProUGUI>().text = appendF + sampGroupSizes[currSize].ToString() + appendB;
            }
            if (Input.GetKeyUp(KeyCode.Backspace))
                CloseTab();
            if(Input.GetKeyUp(KeyCode.Return) && fishSelected)
            {
                animalIDs[numRecorded] = id;
                waypointIDs[numRecorded] = currWaypoint;
                locationSpotted[numRecorded] = recordLocation;
                groupSizes[numRecorded] = appendF + sampGroupSizes[currSize].ToString() + appendB;
                if(printToConsole)
                {
                    print("Fish: " + animal[id].GetName());
                    print("Waypoint #: " + currWaypoint);
                    print("Location: (" + Mathf.Round(recordLocation.x * 100) / 100 + ", " + Mathf.Round(recordLocation.y * 100) / 100 + ", " + Mathf.Round(recordLocation.z * 100) / 100 + ")");
                    print("Group Size: " + appendF + sampGroupSizes[currSize].ToString() + appendB);
                }
                numRecorded++;
                CloseTab();
            }
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            if (gameControl.GetComponent<WaypointManagement>().GetFlightStatus())
            {
                recordLocation = plane.GetComponent<move>().GetLocation();
                plane.GetComponent<move>().SetGoing();
                currWaypoint = gameControl.GetComponent<WaypointManagement>().GetCurrentWaypont();
                self.SetActive(!self.activeSelf);
                waypoint.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Waypoint #" + currWaypoint;
                waypoint.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "(" + Mathf.Round(recordLocation.x * 100) / 100 + ", " + Mathf.Round(recordLocation.y * 100) / 100 + ", " + Mathf.Round(recordLocation.z * 100) / 100 + ")";
            }
        }   
    }

    void ChangeFishStats(int num)
    {
        id = num;
        fishSelected = true;
        fishImage.GetComponent<Image>().sprite = animal[num].GetPicture();
        fishName.GetComponent<TextMeshProUGUI>().text = animal[num].GetName();
    }
    
    void CloseTab()
    {
        self.SetActive(false);
        fishSelected = false;
        fishImage.GetComponent<Image>().sprite = null;
        fishName.GetComponent<TextMeshProUGUI>().text = "Select 0-9";
        groupSize.GetComponent<TextMeshProUGUI>().text = "?";
        appendF = "~"; appendB = "";
        currSize = 0;
        plane.GetComponent<move>().SetGoing();
    }

    public int GetNumberRecorded() { return numRecorded; }

    public int[] GetAnimalIDs()
    {
        int[] arrToReturn = new int[numRecorded];
        if (numRecorded == 0)
            return new int[] { -1 };
        else
            for (int i = 0; i < numRecorded; i++)
                arrToReturn[i] = animalIDs[i];
        return arrToReturn;
    }

    public int[] GetWaypointIDs()
    {
        int[] arrToReturn = new int[numRecorded];
        if (numRecorded == 0)
            return new int[] { -1 };
        else
            for (int i = 0; i < numRecorded; i++)
                arrToReturn[i] = waypointIDs[i];
        return arrToReturn;
    }

    public string[] GetGroupSizes()
    {
        string[] arrToReturn = new string[numRecorded];
        if (numRecorded == 0)
            return new string[] { "null" };
        else
            for (int i = 0; i < numRecorded; i++)
                arrToReturn[i] = groupSizes[i];
        return arrToReturn;
    }

    public Vector3[] GetLocations()
    {
        Vector3[] arrToReturn = new Vector3[numRecorded];
        if (numRecorded == 0)
            return new Vector3[] { new Vector3 (0f, 0f, 0f) };
        else
            for (int i = 0; i < numRecorded; i++)
                arrToReturn[i] = locationSpotted[i];
        return arrToReturn;
    }

    public void TurnOffRecording() { self.SetActive(false); }
}
