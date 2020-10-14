/*
 * Name: Generate Seaweed (GenerateSeaweed.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-08-14
 * Last Modified: 2020-09-06
 * Used in: Aquaculture
 * Description: Used to programmably spawn seaweed throughout the aquaculture scene. To modify the number spawned, go into inspector and change. Currently supports three types; seaweed (multi-type), small seaweed (multi-type), grass (one-type)
 * Status: PRODUCTION
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GenerateSeaweed : MonoBehaviour
{
    // Arrays of seaweed and reference to grass objects to be spawned
    public GameObject[] bigSeaweedToSpawn, smallSeaweedToSpawn;
    public GameObject grass;

    // Number of each of the types to be spawned
    public int bigNumberToSpawn, smallNumberToSpawn, grassToSpawn;

    // The bounds of where to spawn the environment, parent is used to organize the scene once spawned under one gameobject
    public Transform topRight, bottomLeft, parent;

    public float height = 0f, sizeChange = 5f;

    void Start()
    {
        for (int i = 0; i < bigNumberToSpawn; i++) // Big seaweed spawn
        {
            float x = Random.Range(topRight.transform.position.x, bottomLeft.transform.position.x), z = Random.Range(topRight.transform.position.z, bottomLeft.transform.position.z); // Generate random location across the XZ plane
            int typeToSpawn = (int)Random.Range(0f, (int)bigSeaweedToSpawn.Length - 1); // Select the type to spawn
            GameObject temp = Instantiate(bigSeaweedToSpawn[typeToSpawn], new Vector3(x, height, z), new Quaternion(0f, 0f, 0f, 0f), parent); // Spawn the prefab in at the random location
            temp.transform.localScale = Vector3.one * sizeChange; // Set the size to the change specified in the inspector
        }

        for (int i = 0; i < smallNumberToSpawn; i++) // Exactly the same as previous loop but for small seaweed
        {
            float x = Random.Range(topRight.transform.position.x, bottomLeft.transform.position.x), z = Random.Range(topRight.transform.position.z, bottomLeft.transform.position.z);
            int typeToSpawn = (int)Random.Range(0f, (int)smallSeaweedToSpawn.Length - 1);
            GameObject temp = Instantiate(smallSeaweedToSpawn[typeToSpawn], new Vector3(x, height, z), new Quaternion(0f, 0f, 0f, 0f), parent);
            temp.transform.localScale = Vector3.one * sizeChange;
        }

        for(int i = 0; i < grassToSpawn; i++) // Grass spawn
        {
            float x = Random.Range(topRight.transform.position.x, bottomLeft.transform.position.x), z = Random.Range(topRight.transform.position.z, bottomLeft.transform.position.z); // Generate random location across the xZ plane
            GameObject temp = Instantiate(grass, new Vector3(x, height, z), new Quaternion(0f, 0f, 0f, 0f), parent); // Spawn the prefab in at that random location
        }
    }
}
