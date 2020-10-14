using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionControl : MonoBehaviour
{
    GameObject[] aquaculture;
    public GameObject diver, gameControl;
    private float numOfCultures = 33f;
    private float[] indivConcentration, indivConcentrationRadius;
    public bool turnOffIndividualLight = false, turnOffNet = false;
    private bool drawGizmos = true, completedSetup = false;
    public float minConcentration, maxConcentration, minConcentrationRadius, maxConcentrationRadius;
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        numOfCultures = gameControl.GetComponent<RotatingFish>().GetNumberOfCultures();
       // numOfCultures = 33f;
        aquaculture = new GameObject[(int)numOfCultures];
        indivConcentration = new float[(int)numOfCultures];
        indivConcentrationRadius = new float[(int)numOfCultures];
        foreach (Transform child in this.transform)
        {
            foreach (Transform all in child)
            {
                aquaculture[i] = all.gameObject;
                if (turnOffNet)
                    aquaculture[i].transform.GetChild(3).GetComponent<SkinnedMeshRenderer>().enabled = false;
                if(turnOffIndividualLight)
                    aquaculture[i].transform.GetChild(2).GetComponent<Light>().enabled = false;
                SphereCollider temp = aquaculture[i].AddComponent<SphereCollider>();
                temp.center = Vector3.zero;
                temp.radius = 15f;
                indivConcentration[i] = Random.Range(minConcentration, maxConcentration);
                indivConcentrationRadius[i] = Random.Range(minConcentrationRadius, maxConcentrationRadius);
                // Draw a yellow sphere at the transform's position
                i++;
            }
        }
        completedSetup = true;
    }

    private void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            for (int i = 0; i < numOfCultures; i++)
            {
                Color gizmosColor = new Color(0f, 255f, 0f, 0.5f);
                Gizmos.color = gizmosColor;
              //  Gizmos.DrawSphere(aquaculture[i].transform.position, indivConcentrationRadius[i]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameControl.GetComponent<ManageScene>().GetActiveComponent() > 1)
        {
            float currentConcentration = 0f;
            for (int i = 0; i < numOfCultures; i++)
            {
                if (aquaculture[i] == null)
                    break;
                float distance = Vector3.Distance(diver.transform.position, aquaculture[i].transform.position);
                if (distance < indivConcentrationRadius[i])
                    currentConcentration += indivConcentration[i] * ((indivConcentrationRadius[i] - distance) / indivConcentrationRadius[i]);
            }
        //    if(currentConcentration != 0f)
        //        print(currentConcentration);
        }
    }

    public GameObject GetAquaculture(int inst) {
        if (inst >= numOfCultures)
            return null;
        return aquaculture[inst];
    }
    public GameObject[] GetAquaculture() { return aquaculture; }
    public float GetNumberOfAquacultures() { return numOfCultures; }
    public float GetIndividualConcentration(int inst) {
        if (inst >= numOfCultures)
            return 0f;
        return indivConcentration[inst];
    }
    public float[] GetIndividualConcentration() { return indivConcentration; }
    public float GetIndividualConcentrationRadius(int inst)
    {
        if (inst >= numOfCultures)
            return 0f;
        return indivConcentrationRadius[inst];
    }
    public float[] GetIndividualConcentrationRadius() { return indivConcentrationRadius; }

    public bool CompletedSetup() { return completedSetup; }
}
