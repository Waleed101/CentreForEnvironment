using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecordDataORG : MonoBehaviour
{
    public Vector2 latLangStart = new Vector2(33.406989f, -120.39900f), latLangCurrent = new Vector2(0f, 0f);
    public GameObject latLngComm, waypointComm, speciesNameComm, speciesPictureComm, gSizeComm, error;
    private GameObject plane, gameControl;
    private float factor = 0.00000660528f;
    private Vector3 sPos;

    private System.Collections.Generic.List<Vector2> recordPosition = new System.Collections.Generic.List<Vector2>();
    public System.Collections.Generic.List<int> waypointID = new System.Collections.Generic.List<int>(), animalID = new System.Collections.Generic.List<int>(), gSize = new System.Collections.Generic.List<int>();
    public int currentRecord = -1, numOfSpecies = 0;

    private bool going = false, ftime = true, activelyRecording = false, finishedFlight = false;
    // Start is called before the first frame update
    void Start()
    {
        if (plane == null)
            plane = GameObject.FindGameObjectWithTag("Plane");
        if (gameControl == null)
            gameControl = GameObject.FindGameObjectWithTag("GameController");
        numOfSpecies = gameControl.GetComponent<SpawnFish>().GetNumberOfAnimals();
        print(numOfSpecies);
    }

    // Update is called once per frame
    void Update()
    {
        if (!going)
            going = gameControl.GetComponent<WaypointManagement>().GetAbleToGo();
        else
        {
            if(ftime)
            {
                sPos = gameControl.GetComponent<WaypointManagement>().GetSpecificWaypoint(0).transform.position;
                ftime = false;
            }
            latLangCurrent.x = latLangStart.x + factor * (sPos.x - plane.transform.position.x);
            latLangCurrent.y = latLangStart.y + factor * (sPos.y - plane.transform.position.y);
        }

        if(Input.GetKeyDown(KeyCode.R) && !activelyRecording)
        {
            activelyRecording = true;
            error.SetActive(false);
            currentRecord++;
            recordPosition.Add(latLangCurrent);
            waypointID.Add(gameControl.GetComponent<WaypointManagement>().GetCurrentWaypont());
            animalID.Add(0);
            gSize.Add(0);
        }
        if((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace)) && activelyRecording)
        {
            activelyRecording = false;
            recordPosition.RemoveAt(currentRecord);
            waypointID.RemoveAt(currentRecord);
            animalID.RemoveAt(currentRecord);
            gSize.RemoveAt(currentRecord);
            currentRecord--;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (gSize[currentRecord] > 0)
                activelyRecording = false;
            else
                error.SetActive(true);
        }

        this.transform.GetChild(0).gameObject.SetActive(activelyRecording);
        if(activelyRecording)
        {
            if (animalID[currentRecord] >= numOfSpecies)
                animalID[currentRecord] = 0;
            if (gSize[currentRecord] < 0)
                gSize[currentRecord] = 0;
            latLngComm.GetComponent<TextMeshProUGUI>().text = "(" + recordPosition[currentRecord].x + ", " + recordPosition[currentRecord].y + ")";
            waypointComm.GetComponent<TextMeshProUGUI>().text = "Waypoint #" + waypointID[currentRecord];
            Animals temp = gameControl.GetComponent<SpawnFish>().GetSpecificAnimal(animalID[currentRecord]);
            speciesNameComm.GetComponent<TextMeshProUGUI>().text = temp.GetName();
            speciesPictureComm.GetComponent<Image>().sprite = temp.GetPicture();
            gSizeComm.GetComponent<TextMeshProUGUI>().text = gSize[currentRecord].ToString();
        }

        if (Input.GetKeyDown(KeyCode.O))
            if (activelyRecording)
                animalID[currentRecord]++;
        if (Input.GetKeyDown(KeyCode.L))
            if (activelyRecording)
                gSize[currentRecord]++;
        if (Input.GetKeyDown(KeyCode.K))
            if (activelyRecording)
                gSize[currentRecord]--;
        if (Input.GetKeyDown(KeyCode.C))
            if (finishedFlight)
                CopyToClipboard(GetData());

        if (Input.GetKeyDown(KeyCode.Q))
            CopyToClipboard(GetData());
    }

    public void FinishedFlight()
    {
        finishedFlight = true;
        CopyToClipboard(GetData());
    }

    public string GetData()
    {
        string s = "Sample ID, Fish ID, Fish Name, Latitude, Longitude, Waypoint, Group Size\n";
        for(int i = 0; i <= currentRecord; i++)
        {
            Animals temp = gameControl.GetComponent<SpawnFish>().GetSpecificAnimal(animalID[i]);
            s += (i + 1) + ", " + animalID[i] + ", " + temp.GetName() + ", " + recordPosition[i].x + ", " + recordPosition[i].y + ", " + waypointID[i] + ", " + gSize[i] + "\n";
         }
        return s;
    }
    public static void CopyToClipboard(string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }

    public Vector2 GetLatLng() { return latLangCurrent; }
}
