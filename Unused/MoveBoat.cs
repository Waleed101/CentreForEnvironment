/*
 * Name: Move Boat (MoveBoat.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-07-14
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Old script used when the diver was originally on a boat prior to diving into the water and sampling. Unused and can be deleted prior to final build.
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoat : MonoBehaviour
{
    public float waterHeight = 20.8f, speed = 50f, rotationSpeed = 2f, offset = -3.7f;
    private float inputX = 0f, inputZ = 0f;
    private Vector3 location;
    public GameObject boat, driver, camera;

    void Start()
    {
        transform.Rotate(0, 10f, 0, Space.Self);
    }

    void Update()
    {
        location = this.transform.position;
        StayAfloat();
        PlayerMove();
    }

    void PlayerMove()
    {
        inputX = Input.GetAxis("Horizontal"); // Taking in the camera rotation input
        inputZ = Input.GetAxis("Vertical");
        if (inputZ > 0)
            transform.position += boat.transform.forward;
        else if (inputZ < 0)
            transform.position -= boat.transform.forward;
        
        boat.transform.Rotate(0f, rotationSpeed * inputX, 0f);
    }

    void StayAfloat()
    {
        Vector3 loc = boat.transform.position;
        loc.y = waterHeight;
        boat.transform.position = loc;
        loc = driver.transform.position;
        loc.y = waterHeight + offset;
        driver.transform.position = loc;
    }
}
