/*
 * Name: ThirdPersonSwimming (ThirdPersonSwimming.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-01
 * Last Modified: 2020-09-05
 * Used in: Aquaculture
 * Description: Script used to contorl the movement of the diver and to change animation as needed
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonSwimming : MonoBehaviour
{
    public float moveSpeed = 25f, rotationSpeed = 30f; // Getting the settings for move and rotation speed
    private bool crouching = false;
    public bool allowSwim = true, allowRotate = true, swimmingAnimation = true; // Bounds for testing, uncheck in Unity to block certain components
    public GameObject mainCamera, diver, target; // References to needed game objects
    private Animator anim; // Reference to animator

    void Update()
    {
        // Two lines should be moved to Start, need to test :
        anim = diver.GetComponent<Animator>(); // Get the animator
        anim.SetBool("OnGround", false); // Set on ground to false
        // **

        float scaleMotion = Input.GetAxis("Vertical"); // Getting the movement from the arrow keys
        if (scaleMotion != 0f && allowSwim) // Ensure that theres actually movement and swimming is allowed, change animations
            SwapAnimations(true);
        else
            SwapAnimations(false);
        anim.SetFloat("Forward", scaleMotion); // Set speed of animation based on movement

        if(allowSwim)
            transform.position += transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime; // Move the diver
        if(allowRotate)
            transform.RotateAround(target.transform.position, Vector3.up, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime); // Rotate the diver around their center
       // mainCamera.transform.position = transform.position;
    }

    void SwapAnimations(bool swimming)
    {
        anim.SetBool("IsSwimming", swimmingAnimation);
    }

    // Functions to allow external access of functionality
    public void DisableSwimming() { allowSwim = false; }
    public void EnableSwimming() { allowSwim = true; }
    public void DisableRotating() { allowRotate = false; }
    public void EnableRotating() { allowRotate = true; }
}
