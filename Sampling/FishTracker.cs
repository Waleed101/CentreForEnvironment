/*
 * Name: Fish Tracker (FishTracker.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-07-25
 * Last Modified: 2020-09-06
 * Used in: Sampling
 * Description: Used to communicate the current fish sampled to the user to give them an indication on their progress of sampling
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FishTracker : MonoBehaviour
{
    GameObject gameControl;
    void Start()
    {
       // Game control reference
       gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        // Modifying the UI to display the current fish over the number of fish to be sampled
        this.GetComponent<TextMeshProUGUI>().text = (gameControl.GetComponent<PlayFish>().GetCurrentFishNumber()+1) + "/" + gameControl.GetComponent<PlayFish>().GetNumberOfFish();
    }
}
