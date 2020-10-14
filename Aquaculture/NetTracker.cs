/*
 * Name: Net Tracker (NetTracker.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to communicate the current net sampled to the user to give them an indication on their progress of sampling
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NetTracker : MonoBehaviour
{
    GameObject gameControl;

    void Start()
    {
        // Gamecontrol reference
        gameControl = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        // Modifying the UI to display the current net over the number of net to be sampled
        this.GetComponent<TextMeshProUGUI>().text = gameControl.GetComponent<SelectRoute>().GetCurrentComponentOfRoute() + "/" + gameControl.GetComponent<SelectRoute>().GetNumberOfAquaculturesPerRoute();
    }
}
