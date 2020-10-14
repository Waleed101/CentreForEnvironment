/*
 * Name: Demo DELME (DemoDELME.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-05
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Script used for testing and demo purposes when original development of the aquaculture scene.
 * Status: DO NOT USE
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoDELME : MonoBehaviour
{
    public bool plane, diver;
    public GameObject[] planeComps, diverComps;

    void Start()
    {
        // Setting plane and diver components to assigned boolean in the scene management
        for (int i = 0; i < planeComps.Length; i++) 
            planeComps[i].SetActive(plane);
        for (int i = 0; i < diverComps.Length; i++)
            diverComps[i].SetActive(diver);
    }
}
