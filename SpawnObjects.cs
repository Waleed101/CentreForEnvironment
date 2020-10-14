/*
 * Name: SpawnObjects (SpawnObjects.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-01
 * Last Modified: 2020-09-05
 * Used in: Waypoint Survey
 * Description: Script used to control the spawn and movement of random objects to add more variety to the scene
 * Status: PRODUCTION
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    public Objects[] objects; // Creating array of object asset files
    GameObject[] objectControllers; // Creating array to reference individual objects to control them (no real need for this right now, can be replaced with list as well)
    public GameObject parent; // Reference to parent to organize the scene better
    int size = 10;
    private int[] numberOfEachToSpawn; // Stores the number to spawn per individual object

    void Start()
    {
        numberOfEachToSpawn = new int[objects.Length];
        int total = 0;
        for (int i = 0; i < objects.Length; i++) // Figuring out the number that will be spawned in order to intiate the gameobject array
        {
            numberOfEachToSpawn[i] = (int)objects[i].GetWaypointSpawnRate();
            print("For " + i + ", spawning: " + numberOfEachToSpawn[i]);
            total += numberOfEachToSpawn[i];
        }
        print("Spawning: " + total);
        objectControllers = new GameObject[total];
        SpawnAll(); // Spawning all of them in
    }

    void SpawnAll()
    {
        int currentObject = 0;
        for(int i = 0; i < objectControllers.Length; i++) // Cycling through all the randomized objects to spawn
        {
            if (i > numberOfEachToSpawn[currentObject]) // Cycle through all the objects
                currentObject++;
            if (currentObject >= objects.Length) // Prevent null exception
                break;

            float angle = Random.Range(0f, 360f); // Getting a random rotation
            objectControllers[i] = Instantiate(objects[currentObject].GetPrefab()); // Spawning in the prefab
            objectControllers[i].transform.localScale = Vector3.one * objects[currentObject].GetPrefabSizeChange(); // Changing the size as needed
            objectControllers[i].transform.rotation = Quaternion.Euler(0f, angle + objects[currentObject].GetAngleOffset(), 0f); // Rotating to the random
            Vector3 objectStartLocation = new Vector3(RandomRange(600f, 4000f), objects[currentObject].GetMaxSeaLevel(), RandomRange(-800f, 1100)); // Setting the start location
            objectControllers[i].transform.position = objectStartLocation; // Setting position to randomized poisition (line 52 and 53 can be condensened into 2 if needed)
            if (objects[currentObject].GetMovement()) // Check if it should be moving
                objectControllers[i].GetComponent<Rigidbody>().velocity = new Vector3(objects[currentObject].GetSpeed() * Mathf.Sin(Mathf.Deg2Rad * angle), 0f, objects[currentObject].GetSpeed() * Mathf.Cos(Mathf.Deg2Rad * angle)); // Getting and setting a random velocity
            objectControllers[i].transform.SetParent(parent.transform); // Setting parent for organization
        }
    }

    float RandomRange(float s, float e) // Random range function to prevent errors
    {
        if (s == e)
            return s + Random.Range(-100f, 100f);
        else
            return Random.Range(s, e);
    }
}
