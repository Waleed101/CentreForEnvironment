/*
 * Name: Record Data Aqua (RecordDataAqua.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-25
 * Last Modified: 2020-09-06
 * Used in: Phased Out
 * Description: Old script to record data, phased out for new data recording UI/script
 * Status: UN-USED
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordDataAqua : MonoBehaviour
{
    public GameObject diver;
    public float distance = 10f;
    // Update is called once per frame
    void Update()
    {
        if ((Vector3.Distance(diver.transform.position, this.transform.position) < distance))
            print("scuba diver");
    }
}
