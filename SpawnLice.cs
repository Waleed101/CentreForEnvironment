using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLice : MonoBehaviour
{
    public GameObject licePrefab, parentOfLice, fish;
    public float minToSpawn, maxToSpawn;
    private GameObject[] lice;
    // Start is called before the first frame update
    void Start()
    {
        lice = new GameObject[(int)Random.Range(minToSpawn, maxToSpawn)];
        LiceSpawner();
    }

    void LiceSpawner()
    {
        for (int i = 0; i < lice.Length; i++)
        {
            SkinnedMeshRenderer r = fish.GetComponent<SkinnedMeshRenderer>();
            float randomX = Random.Range(r.bounds.min.x, r.bounds.max.x), randomY = Random.Range(r.bounds.min.y, r.bounds.max.y)/2, randomZ = Random.Range(r.bounds.min.z, r.bounds.max.z/2);
            RaycastHit hit;

            Vector3 loc = new Vector3(randomX, randomY, randomZ);
            if (Physics.Raycast(loc, -Vector3.up, out hit))
                lice[i] = Instantiate(licePrefab, loc, Quaternion.Euler(0f, 0f, 0f), parentOfLice.transform);
            else
                print("doesn't work here");
        }
    }
}
