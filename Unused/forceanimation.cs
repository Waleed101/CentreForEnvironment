/*
 * Name: Force Animation (forceanimation.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-21
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Originally used to test out the whale animations, but phased out once modifications were made to the models in the scene mode and Blender that made this unnecessary. Can be deleted prior to final build.
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceanimation : MonoBehaviour
{
    Animation anim;

    void Start()
    {
        anim = GetComponentInChildren<Animation>();
    }

    void Update()
    {
        print(anim.Play("Scene"));
    }
}
