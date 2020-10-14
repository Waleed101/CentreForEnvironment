/*
 * Name: Hover Control (HoverControl.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: PHASED OUT
 * Description: Previously used to track what the current UI page is on for the final/launch screen.
 * Status: UNUSED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverControl : MonoBehaviour
{
    public GameObject start, controls, credits;
    public int active = -1;

    public int GetActive() { return active; }
    public void SetActive(int i) { active = i; }
}
