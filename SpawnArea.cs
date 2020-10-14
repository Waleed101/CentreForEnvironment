using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnArea : MonoBehaviour
{
    public GameObject spawnPrefab;
    public float numberToSpawn = 0, height = 8f, sizeChange = 3f;
    public float[] radius;
    public Vector3[] locations;
    public Vector3 offset;
    private GameObject[] prefab;
    // Start is called before the first frame update
    void Start()
    {
        int currentZone = 0;
        prefab = new GameObject[(int)numberToSpawn];
        for(int i = 0; i < numberToSpawn; i++)
        {
            currentZone = ChooseZone();
            Vector3 location;
            location = RandomPoints(currentZone);
          /*  do
            {
                location = RandomPoints(currentZone);
            } while (ConflictingSpawn(location, i));*/
            prefab[i] = Instantiate(spawnPrefab);
            prefab[i].transform.localScale *= sizeChange;
            prefab[i].transform.position = location;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int ChooseZone()
    {
        float fullRadiusSum = 0f;
        for (int i = 0; i < radius.Length; i++)
            fullRadiusSum += radius[i] * 2;
        float randRadius = Random.Range(0, fullRadiusSum);
        for (int i = 0; i < radius.Length; i++)
            if (FindZoneFromRandom(randRadius, i))
                return i;
        return 0;
    }

    bool FindZoneFromRandom(float radiusRand, int zoneID)
    {
        float fullRadiusSum = 0f;
        for (int i = 0; i < zoneID; i++)
            fullRadiusSum += radius[i] * 2;
        if (radiusRand > fullRadiusSum && radiusRand < (fullRadiusSum + radius[zoneID] * 2))
            return true;
        return false;
    }

    Vector3 RandomPoints(int zone)
    {
        Vector3 point = locations[zone] + offset;
        point.x += Random.Range(-radius[zone], radius[zone]);
        point.z += Random.Range(-radius[zone], radius[zone]);
        point.y = height;
        return point;
    }

    bool ConflictingSpawn(Vector3 location, int cur)
    {
        for(int i = 0; i < cur; i++)
        {
            Vector3 diff = prefab[i].transform.position - location;
            diff = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));
            if (diff.x < 30f || diff.z < 30f)
                return false;
        }
        return true;
    }
}
