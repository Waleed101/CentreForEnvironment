/*
 * Name: Health Bar Scuba (HealthBarScuba.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-07-15
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Previously used to have a health/air indictaor to provide a more scuba diver feel, but phased out due to an improper UI design. Key functionality is stil present, but needs a proper radial oxygen UI in the inspector.
 * Status: UNUSED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScuba : MonoBehaviour
{
    // All can be set in the inspector, changeToRed is the threshold at which the UI will change to the red color
    public float startingAir = 220f, changeToRed = 50f, decreasePerSecond = 0.5f, currentAir = 0f;

    // Reference to the indicator gameobject
    public GameObject indicator;
    // Start is called before the first frame update
    void Start()
    {
        currentAir = startingAir;
        indicator.GetComponent<Slider>().maxValue = startingAir;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrease air by specified factor
        currentAir = startingAir - Time.time * decreasePerSecond;
        indicator.GetComponent<Slider>().value = currentAir;
        if (currentAir < changeToRed) // Change color if red threshold is met
            indicator.GetComponentInChildren<Image>().color = Color.red;

        // Simply print "dead" when air is done, no other functionaility was outlined in the inital design, but more can be added by simply changing out whats under this logic statement
        if (currentAir < 0)
            print("dead");
    }
}
