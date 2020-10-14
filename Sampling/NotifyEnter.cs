/*
 * Name: Notify Enter (NotifyEnter.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-09
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to track whether or not the diver enters the region of the aquaculture and to control the colliders
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyEnter : MonoBehaviour
{
    // Reference to various gameobjects, thirdPerson is the diver
    private GameObject gameControl, notificationUI, thirdPerson, aquaParent;

    // AquaID keeps track of the current net being sampled
    private int aquaID = -1;
    private bool ftime = true;

    void Start()
    {
        // If the references are null, grab them by their tags
        if (gameControl == null)
            gameControl = GameObject.FindGameObjectWithTag("GameController");
        if (notificationUI == null)
            notificationUI = GameObject.FindGameObjectWithTag("Notification_UI");
        if (thirdPerson == null)
            thirdPerson = GameObject.FindGameObjectWithTag("Third Person Camera");
        if (aquaParent == null)
            aquaParent = GameObject.FindGameObjectWithTag("Aqua Parent");

        // Turn off the notification UI
        notificationUI.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.GetComponents<SphereCollider>().Length > 1) // Originally had an error where a second sphere collider would spawn in, only way around was to destroy the second instance of the sphere collider
            Destroy(this.GetComponents<SphereCollider>()[1]);
        if (aquaID != -1 && gameControl.GetComponent<ManageScene>().GetActiveComponent() == 3) // Make sure that the correct scene is in play and that the aquaculture has been set up properly
            this.GetComponent<SphereCollider>().enabled = gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID() == aquaID; // Only enable the collider if you're supposed to be sampling it, reduces lag
    }

    public void OnCollisionEnter(Collision collision) // Track when they enter
    {
        if(!gameControl.GetComponent<SelectRoute>().isDone())
            if (collision.gameObject.tag == "Player")
                if (gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID() == aquaID)
                    notificationUI.transform.GetChild(0).gameObject.SetActive(true); // Turn on UI; universally used throughout the scripts to signal that they are currently sampling
    }

    public void OnCollisionExit(Collision collision) // Track when they exit
    {
        if (!gameControl.GetComponent<SelectRoute>().isDone())
            if (collision.gameObject.tag == "Player")
                if (gameControl.GetComponent<SelectRoute>().GetCurrentTargetAquacultureID() == aquaID)
                    notificationUI.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void SetAquacultureID(int instance) { aquaID = instance; } // Set aquaculture ID during setup process
    public int GetAquacultureID() { return aquaID; }
}
