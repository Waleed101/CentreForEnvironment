/*
 * Name: Draw Area (DrawArea.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: Aquaculture, Waypoint Survey, Rotating Fish
 * Description: Class to communicate errors from external scripts to users in the game mode. Takes in text and time to display and displays a properly formatted error for the specificed time.
 * Status: TEST
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorHandeling : MonoBehaviour
{
    // Reference to the error UI in the scene
    public GameObject errorUI;

    private float timeSinceError = 0f, timeForError = 0f;
    private bool displayError = false;
    private string errorText = "";
    // Start is called before the first frame update
    void Start()
    {
        // Turn off error gameobject
        errorUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(displayError)
        {
            if (!((Time.time - timeSinceError) < timeForError)) // Check if the time has elasped specified when the function is called
            {
                // If time is elasped, turn off the error UI
                displayError = false;
                errorUI.SetActive(false);
            }
        }
    }

    public void DisplayError(string text, float timeToDisplay) // Function called by external scripts to display their specific error for a certain time (in seconds)
    {
        errorText = text;
        // Used to calculate the length of the background of the text to make it look better
        int UILength = text.Length;
        errorUI.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UILength*25f);

        // Set time and turn on display, change text
        timeForError = timeToDisplay;
        displayError = true;
        timeSinceError = Time.time;
        ChangeText();
    }

    void ChangeText()
    {
        errorUI.SetActive(true);
        errorUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = errorText;
    }
}
