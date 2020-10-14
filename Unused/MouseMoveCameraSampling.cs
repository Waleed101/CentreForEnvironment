/*
 * Name: MouseMoveCameraSampling (MouseMoveCameraSampling.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-26
 * Last Modified: 2020-09-06
 * Used in:
 * Description: 
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMoveCameraSampling : MonoBehaviour
{
    public float sensitivity = 10f;
    public bool lockRotation = true;

    // Update is called once per frame
    void Update()
    {
        if(lockRotation)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        transform.Rotate(-transform.up * rotateHorizontal * sensitivity);
        transform.Rotate(transform.right * rotateVertical * sensitivity);
    }
}
