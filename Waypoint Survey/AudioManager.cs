/*
 * Name: Audio Manager (AudioManager.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-06-01
 * Last Modified: 2020-09-05
 * Used in: Waypoint Survey
 * Description: Controls what audio is played at what specific points from the sound files provided in the Speaker array.
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Speaker[] soundFiles; // array where controller can drag in all the .Speaker files (found in Assets > Custom Asset Files > Sound Bites)
    public GameObject AudioParent, audioSource, text, image; // references to the different components of the UI
    private int currentSound = -1; // stores the current sound for reference in the functions, can be phased out for a variable thats passed over

    private bool audioPlaying = false;


    void Start()
    {
        AudioParent.SetActive(false); 
    }

    void Update()
    {
        if(audioPlaying) // Turning off the UI when no audio is playing
            if (!audioSource.GetComponent<AudioSource>().isPlaying)
                TurnOffInterface();
    }

    public void NextWaypoint(int waypoint) // Function used by waypoint management to signal for a check of the audio
    {
        for(int i = 0; i < soundFiles.Length; i++) // Cycle through all the sound files 
        {
            if (soundFiles[i].GetWaypointID() == waypoint) // Check the waypoint ID on each sound file to the current waypoint
            {
                currentSound = i; // Set the sound
                PlaySound(); // Play it
                break; // Break out of the loop, if multiple sound files share the same waypoint ID - the one in the highest order takes precedent
            }
        }
    }

    void PlaySound() // Function to play sound
    {
        ChangeInterface(); // Turn on the interface
        if (audioSource.GetComponent<AudioSource>()) // Ensure that there is an actual audio source on the object, this prevents a fatal error
        {
            audioSource.GetComponent<AudioSource>().clip = soundFiles[currentSound].GetAudioClip(); // Get the clip from the sound file and set it
            audioSource.GetComponent<AudioSource>().Play(); // Play the sound
            audioPlaying = true;
        }
    }

    void ChangeInterface() // Modify interface to display name and picture of speaker
    {
        AudioParent.SetActive(true);
        text.GetComponent<TextMeshProUGUI>().text = soundFiles[currentSound].GetName() + "\n speaking";
        image.GetComponent<Image>().sprite = soundFiles[currentSound].GetPersonPicture();
    }

    void TurnOffInterface() // Turn of interface
    {
        audioPlaying = false;
        audioSource.GetComponent<AudioSource>().Stop();
        AudioParent.SetActive(false);
    }
}
