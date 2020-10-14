using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RotatingFish : MonoBehaviour
{
    [Header("Fish Customizability")]
    public GameObject fishPrefab, fishParent;
    public Vector2 radius;
    public Vector2 height;
    public Vector2 toSpawn;
    public Vector2 speed;
    public Vector2 size;

    [Header("Aquacultures")]
    public GameObject homeParents;
    public int numOfCultures = 33;


    private Transform[] aquaculture;
    private GameObject[,] fishSpawned;
    private float[,] fishSpeeds;
    private bool[,] negStart;

    private Vector3 axisOfRotation = new Vector3(0f, 1f, 0f);
    // Start is called before the first frame update
    void Start()
    {

        int numTracker = 0;
        aquaculture = new Transform[numOfCultures];
        fishSpawned = new GameObject[numOfCultures, (int)toSpawn.y];
        fishSpeeds = new float[numOfCultures, (int)toSpawn.y];
        negStart = new bool[numOfCultures, (int)toSpawn.y];
        foreach (Transform child in homeParents.transform)
        {
            foreach(Transform all in child)
            {
                if (numTracker >= numOfCultures)
                    goto DoneSpawning;
                aquaculture[numTracker] = all;
                all.gameObject.GetComponent<NotifyEnter>().SetAquacultureID(numTracker);
                numTracker++;
            }
        }
    DoneSpawning:
        print("Done Spawning");
        SpawnFish();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < numOfCultures; i++)
        {
            for(int p = 0; p < toSpawn.y; p++)
            {
                if (fishSpawned[i, p] == null)
                    break;
                else
                {
                   Vector3 tempAxis = axisOfRotation;
                   if (negStart[i,p])
                        tempAxis.y *= -1;
                    fishSpawned[i, p].transform.RotateAround(aquaculture[i].position, tempAxis, fishSpeeds[i, p] * Time.deltaTime);
                    Animation anim = fishSpawned[i, p].transform.GetChild(0).gameObject.GetComponent<Animation>();
                    anim["SalmonSwim"].speed = Random.Range(0.5f, 1f);
                    fishSpawned[i, p].transform.LookAt(fishSpawned[i, p].transform);
                }
            }
        }
    }

    void SpawnFish()
    {
        for(int i = 0; i < numOfCultures; i++)
        {
            int numberToSpawn = Random.Range((int)toSpawn.x, (int)toSpawn.y);
            for(int p = 0; p < numberToSpawn; p++)
            {
                float radiusTemp = Random.Range(radius.x, radius.y);
                float heightChange = Random.Range(height.x, height.y);
                fishSpawned[i, p] = Instantiate(fishPrefab, fishParent.transform);
                fishSpawned[i, p].transform.localScale *= Random.Range(size.x, size.y);
                Vector3 pos = aquaculture[i].position;
                pos.x += radiusTemp; pos.y += heightChange;
                if (radiusTemp > 0)
                    negStart[i, p] = true;
                fishSpeeds[i, p] = Random.Range(speed.x, speed.y);
                fishSpawned[i, p].transform.position = pos;
            }
        }
    }

    public float GetNumberOfCultures() { return numOfCultures; }

    public Vector3 GetAquacultureLocation(int instance) { return aquaculture[instance].position; }

    public Transform[] GetAquacultures() { return aquaculture; }
}
