/*
 * Name: Manage Scene (ManageScene.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-28
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to manage and track the current scene (flight, select route, diver, final) and relay that information to different scripts. Fade (black screen) is currently commented out due to issues, but if figured out can be easily uncommented and added back in.
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageScene : MonoBehaviour
{
    // Controlling the fade, turn this on to activate the fade in
    public bool doFade = false;

    // References to the different components used; fadeCanvas is the black screen, and each of the others are the different parents of the scene components
    public GameObject fadeCanvas, diverScene, planeScene, routeSelector;

    // part to switch is when the actual switch will happen during the fade, can be changed in the inspector
    public float fadeInSpeed = 0.5f, partToSwitch = 0.1f;
    private int activeComp;
    private bool activateSceneSwitch = false, completedSceneSwitch = true;
    void Start()
    {
        // Set the current scene to the first one; plane flight and change the scene
        activeComp = 1;
        ChangeActive();
    }

    void ChangeActive() // Function to change the scene to the current set in the variable "activeComp"
    {
        // Uncomment the following to reactivate the scene switch
      //  Color c = fadeCanvas.GetComponent<Image>().color;
      //  c.a = 0f;
      //  fadeCanvas.GetComponent<Image>().color = c;
        switch (activeComp)
        {
            case 1: // Flight scene
                planeScene.SetActive(true);
                routeSelector.SetActive(false);
                diverScene.SetActive(false);
                break;

            case 2: // Route selector scene
                planeScene.SetActive(false);
                routeSelector.SetActive(true);
                diverScene.SetActive(false);
                break;

            case 3: // Diver scene
                planeScene.SetActive(false);
                routeSelector.SetActive(false);
                diverScene.SetActive(true);
                break;
        }
      //  completedSceneSwitch = true;
    }

    void Update()
    {
        if(activateSceneSwitch && doFade) { // Function to fade
          StartCoroutine(FadeScreenIn());
        }

        if(completedSceneSwitch || !doFade){ // Switch active
            ChangeActive();
        }
    }

   IEnumerator FadeScreenIn()
    {
        activateSceneSwitch = false;
        for(float f = 0.5f; f <= 1f; f+=0.05f) // Fade in
        {
            Color c = fadeCanvas.GetComponent<Image>().color;
            c.a = f;
            fadeCanvas.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(fadeInSpeed);
        }
        for (float f = 1f; f >= -0.05f; f -= 0.05f) // Fade out
        {
            Color c = fadeCanvas.GetComponent<Image>().color;
            c.a = f;
            fadeCanvas.GetComponent<Image>().color = c;
            if (f < partToSwitch)
                completedSceneSwitch = true;
            yield return new WaitForSeconds(fadeInSpeed);
        }
    }

    public void SetActiveComponent(int num) { activeComp = num; activateSceneSwitch = true; } // Function to change the active component by an external script
    public int GetActiveComponent() { return activeComp; } // Function to get the active component
}
