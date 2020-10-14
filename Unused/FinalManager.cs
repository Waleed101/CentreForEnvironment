/*
 * Name: Final Manager (FinalManager.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-19
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Old Final UI used to communicate the sampling data. Unused and can be deleted before launch
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FinalManager : MonoBehaviour
{
    public int curBackground = 0;
    public float timePer = 10f, prevChange = 0f;
    public Sprite[] backgrounds;
    public GameObject background, self, gameControl, recordData, arrows;
    public GameObject[] items;

    private int numberRecorded = 0, currentSlide = 0;
    private int[] waypoints, animalIDs;
    private string[] gSize;
    private Vector3[] location;
    private Animals[] animal;
    public bool fTime = true;
    // Start is called before the first frame update
    void Start()
    {
    //    animal = gameControl.GetComponent<SpawnFish>().GetAnimals();
        self.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (self.activeSelf)
        {
            if (fTime)
            {
                fTime = GetData();
                SetValues();
            }
            CycleImages();
        }
    }

    public void SetValues()
    {
        for(int i = currentSlide*4; i < (currentSlide*4 + 4); i++)
        {
            print(i);
            int j = i - currentSlide * 4;
            if ((i + 1) > numberRecorded)
                items[j].SetActive(false);
            else
                items[j].SetActive(true);

            if(i < numberRecorded)
            {
                items[j].transform.GetChild(0).GetComponent<Image>().sprite = animal[animalIDs[i]].GetPicture();
                items[j].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = animal[animalIDs[i]].GetName();
                items[j].transform.GetChild(2).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gSize[i];
                items[j].transform.GetChild(3).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "(" + Mathf.Round(location[i].x * 100) / 100 + ", " + Mathf.Round(location[i].y * 100) / 100 + ", " + Mathf.Round(location[i].z * 100) / 100 + ")";
            }
        }
        Arrows();
    }

    public void Arrows()
    {
        arrows.transform.GetChild(2).gameObject.SetActive(true);
        arrows.transform.GetChild(1).gameObject.SetActive(true);
        int numOfSlides = numberRecorded / 4;
        if (numberRecorded % 4 != 0)
            numOfSlides++;
        if (currentSlide == 0)
            arrows.transform.GetChild(2).gameObject.SetActive(false);
        if ((currentSlide+1) == numOfSlides)
            arrows.transform.GetChild(1).gameObject.SetActive(false);
        arrows.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (currentSlide+1) + "/" + numOfSlides;
    }

    public bool GetDataTest()
    {
        numberRecorded = 2;
        waypoints = new int[] { 1, 1 };
        animalIDs = new int[] { 0, 0 };
        gSize = new string[] { "100", "10" };
        location = new Vector3[] { new Vector3(12f, 12f, 12f), new Vector3(12f, 12f, 12f) };
        return true;
    }

    public bool GetData()
    {
        numberRecorded = recordData.GetComponent<RecordData>().GetNumberRecorded();
        if (numberRecorded == 0)
        {
            gameControl.GetComponent<ErrorHandeling>().DisplayError("No Fish Records were found!", 30f);
            this.gameObject.SetActive(false);
            return false;
        }
        waypoints = recordData.GetComponent<RecordData>().GetWaypointIDs();
        animalIDs = recordData.GetComponent<RecordData>().GetAnimalIDs();
        for (int i = 0; i < numberRecorded; i++)
            print("ID#" + animalIDs[i]);
        gSize = recordData.GetComponent<RecordData>().GetGroupSizes();
        location = recordData.GetComponent<RecordData>().GetLocations();
        return false;
    }

    public void CycleImages()
    {
        if ((Time.time - prevChange) >= timePer)
        {
            prevChange = Time.time;
            curBackground++;
            if (curBackground >= backgrounds.Length)
                curBackground = 0;
            ChangeImage();
        }
    }
    public void ChangeImage()
    {
        background.GetComponent<Image>().sprite = backgrounds[curBackground];
    }

    public void NextPage() { currentSlide++; SetValues(); }
    public void SetFtime(bool status) { fTime = status; }
    public void PreviousPage() { currentSlide--; SetValues(); }

}
