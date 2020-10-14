/*
 * Name: Force Animation (forceanimation.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-07-21
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Originally used when testing out problems with diver animation/movement, but eventually problems were figured out. Can be deleted prior to final build.
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceRotation : MonoBehaviour
{
    public bool x = false, y = false, z = false;

    void FixedUpdate()
    {
        float xCh = 0f, yCh = 0f, zCh = 0f;
        if (x)
            xCh = 180f;
        if (y)
            yCh = 180f;
        if (z)
            zCh = 180f;
        this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x + xCh, this.transform.rotation.eulerAngles.y + yCh, this.transform.rotation.eulerAngles.z + zCh);
    }
}
