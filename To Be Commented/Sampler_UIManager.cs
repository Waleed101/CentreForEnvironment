using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sampler_UIManager : MonoBehaviour
{
    public GameObject UI_Parent, gameControl, error, back;
    private GameObject[] indivUIParents;
    public float[,] data;
    private int currentStage = 0, currentFish, stageAtError;

    private bool endActive = false;
    // Start is called before the first frame update
    void Start()
    {
        error.SetActive(false);
        back.SetActive(false);
        indivUIParents = new GameObject[UI_Parent.transform.childCount];
        data = new float[gameControl.GetComponent<PlayFish>().GetNumberOfFish(), UI_Parent.transform.childCount];
        for (int i = 0; i < UI_Parent.transform.childCount; ++i)
            indivUIParents[i] = UI_Parent.transform.GetChild(i).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(!endActive)
        {
            endActive = gameControl.GetComponent<PlayFish>().EndSceneActive();
            currentStage = gameControl.GetComponent<MoveCamera>().GetCurrentPosition();
            currentFish = gameControl.GetComponent<PlayFish>().GetCurrentFishNumber();
            data[currentFish, 2] = Mathf.Round(gameControl.GetComponent<PlayFish>().GetCurrentWeight() * 100f) / 100f;
            if (error.activeSelf)
                if (stageAtError != currentStage)
                    error.SetActive(false);
            if (!(currentStage <= 1) && currentStage != 4)
            {
                if (data[currentFish, currentStage - 1] != 0)
                    back.SetActive(true);
                else
                    back.SetActive(false);
            }
            else
                back.SetActive(false);
            for (int i = 0; i < indivUIParents.Length; i++)
            {
                if (i == currentStage)
                    indivUIParents[i].SetActive(true);
                else
                    indivUIParents[i].SetActive(false);
                if (indivUIParents[i].transform.childCount > 0)
                    indivUIParents[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "" + data[currentFish, i];
            }

            if (indivUIParents[currentStage].transform.childCount > 6)
            {
                if (Input.GetKeyDown(KeyCode.L))
                    IncreaseCurrentValue();
                if (Input.GetKeyDown(KeyCode.K))
                    DecreaseCurrentValue();
            }

            if (Input.GetKeyDown(KeyCode.B))
                if (back.activeSelf)
                    gameControl.GetComponent<MoveCamera>().SetCurrentPosition(currentStage - 1);
        }
    }
    public int GetCurrentRecording() { return currentStage; }
    public float GetData() { return data[currentFish, currentStage]; }
    public float GetSpecificData(int inst) { return data[currentFish, inst]; }
    public void IncreaseCurrentValue() { data[currentFish, currentStage]++; }
    public void DecreaseCurrentValue() { data[currentFish, currentStage]--; }
    public bool FinishedRecordingData() { 
        if (data[currentFish, currentStage] != 0f || currentStage == 0 || currentStage == 4 || currentStage == 1) 
            return true; 
        else
        {
            stageAtError = currentStage;
            error.SetActive(true);
            return false;
        }
    }

    public string GetAllData(int numOfFish)
    {
        string dataStr = "Fish Number,Fish Type,Number of Lice,Weight,Length";
        for(int i = 0; i < numOfFish; i++)
        {
            dataStr += "\n";
            dataStr += (i + 1) + ",Salmon," + data[i, 1] + "," + data[i, 2] + "," + data[i, 3]; 
        }
        return dataStr;
    }
}
