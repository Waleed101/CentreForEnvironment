using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoute : MonoBehaviour
{
    public int[,] routeChoices;
    public int numberOfRouteChoices, aquaculturesPerRoute;
    public GameObject aquacultureParent;

    private int currentComponentOfRoute = 0, pickedRoute = 0;
    private bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        routeChoices = new int[numberOfRouteChoices, aquaculturesPerRoute];
        PickRandomRoute();
    }

    void PickRandomRoute()
    {   
        int numberOfAquacultures = (int)GetComponent<RotatingFish>().GetNumberOfCultures();
        for (int i = 0; i < numberOfRouteChoices; i++)
        {
            string routeToPrint = "Route " + (i + 1) + ": ";
            for (int j = 0; j < aquaculturesPerRoute; j++)
            {
                bool previouslyPicked = true;
                while(previouslyPicked)
                {
                    previouslyPicked = false;
                    routeChoices[i, j] = (int)Random.Range(0f, (float)numberOfAquacultures);
                    for(int p = 0; p < j; p++)
                    {
                        if (routeChoices[i, p] == routeChoices[i, j])
                        {
                            previouslyPicked = true;
                            break;
                        }
                    }
                }
                routeToPrint += (routeChoices[i, j] + ", ");
            }
            print(routeToPrint);
        }
    }

    public void NextAquaculture()
    {
        if (currentComponentOfRoute + 1 == aquaculturesPerRoute)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<DiverSampling>().CopyToClipboard(GameObject.FindGameObjectWithTag("Player").GetComponent<DiverSampling>().GetData());
            done = true;
            aquacultureParent.GetComponent<DisplayWaypointArrow>().WaypointViewStatus(false);
        }
        else
        {
            currentComponentOfRoute++;
            aquacultureParent.GetComponent<DisplayWaypointArrow>().ChangeWaypointArrow();
        }
    }

    public int[,] GetRouteChoices() { return routeChoices; }
    public int GetNumberOfRouteChoices() { return numberOfRouteChoices; }
    public int GetNumberOfAquaculturesPerRoute() { return aquaculturesPerRoute; }
    public int GetCurrentComponentOfRoute() { return currentComponentOfRoute; }
    public int GetCurrentTargetAquacultureID() { return routeChoices[pickedRoute, currentComponentOfRoute]; }
    public bool CheckIfAquacultureOnRoute(int aquaInst)
    {
        for (int i = 0; i < aquaculturesPerRoute; i++)
            if (routeChoices[pickedRoute, i] == aquaInst)
                return true;
        return false;
    }
    public void SetPickedRoute(int inst) { pickedRoute = inst; }
    public bool isDone() { return done; }
}
