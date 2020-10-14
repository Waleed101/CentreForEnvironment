using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Principal;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnFish : MonoBehaviour
{
    public Animals[] animalInfo; // Stores the animal info scripts
    public GameObject[,] fishes; // Holds the game objects with all the fishes
    public float[,] floors, yAngle; // Stores the y spawn coordinates are for each fish, used for jumping scripts
    public bool[,] curJumper; // Stores whether or not a fish is currently jumping; could be replaced by checking if its not equal to the floor *****
    public int[] cirInst, groupSize, sFrameAnimation; // 1: stores which part of the school is currently jumping, 2: stores the group sizes of all the schools
    public float[] animationRate;
    public int max, currentWaypoint;
    public GameObject parent;

    private int animationDuration = 2000/30;
    private float refFrameRate = 50f;
    public float timeFrame = 0f, diffTimeFrame = 0f, xOffsetcst = 250f;

    // Array with status of fish 0-swimming, 1-jumping, 2-floating - along with time its been at that status for

    void Start()
    {
        groupSize = new int[animalInfo.Length];
        max = 0;
        for (int i = 0; i < animalInfo.Length; i++)
        {
            groupSize[i] = (int)animalInfo[i].GetGroupSize();
            if (groupSize[i] > max)
                max = groupSize[i];
        }
        
        // Intiating all arrays as needed
        fishes = new GameObject[animalInfo.Length, max];
        floors = new float[animalInfo.Length, max];
        curJumper = new bool[animalInfo.Length, max];
        cirInst = new int[animalInfo.Length];
        sFrameAnimation = new int[max];
        animationRate = new float[max];
        yAngle = new float[animalInfo.Length, max];
    }

    void FixedUpdate()
    {
        for (int i = 0; i < animalInfo.Length; i++) // Cycle through all the animal scripts
        {
            bool currentlySpawned = animalInfo[i].GetWaypointSpawn(currentWaypoint);
            if (!currentlySpawned)
                continue;

            bool packJumping = false; // Determine if jumping is currently taking place
            for (int j = 0; j < max; j++) // Cycle through the whole array
            {
                if (fishes[i, j] == null) // This signals that the end of the current school has been reached, so jump out of the loop
                    break;
                if (curJumper[i, j]) //Check to see if its currently jumping
                {
                    //  print(i + ", " + j + " Jumping");
                    bool animationStillPlaying = false;
                    packJumping = true; // Then someone in the pack is jumping
                    if (animalInfo[i].GetJumpAnimatedType())
                        animationStillPlaying = CheckAnimationPlaying(i, j);
                    else
                    {
                        if (ReachedCieling(i, j)) // If its passed the cieling, turn it around
                            YMotion(i, j, false, false);
                        else if (IsGrounded(i, j)) // If its still going, but below the ground, reset it to its spot
                        {
                            GroundIt(i, j);
                            curJumper[i, j] = false;
                            continue;
                        }
                    }
                    //  print(animationStillPlaying);
                    if (!animationStillPlaying)
                    {
                        print(Time.frameCount);
                        print("Animation Stopped");
                        GroundIt(i, j);
                        curJumper[i, j] = false;
                    }

                }
                else // if it's not jumping, make sure its on the floor
                {
                    fishes[i, j].transform.position = Floor(fishes[i, j].transform.position, i, j);
                    print(i);
                    PlayAnimation(i, j, "Fast Swim");
                    Animation anim = fishes[i, j].GetComponent<Animation>();
                    anim["Fast Swim"].speed = animationRate[j];
                }
            }
            if (!packJumping) // If there is no jumping currently going on - start it!
            {
               // print(animalInfo[i].GetJumpRate());
                bool animatedJump = animalInfo[i].GetJumpAnimatedType();
                int jumpPointerLocation = cirInst[i];
                if ((jumpPointerLocation + animalInfo[i].GetJumpRate()) >= groupSize[i])
                    jumpPointerLocation = 0;
                for (int x = jumpPointerLocation; x < (jumpPointerLocation + animalInfo[i].GetJumpRate()); x++)
                {
                    if (fishes[i, x] == null)
                        break;
                    curJumper[i, x] = true;
                    if (animatedJump)
                    {
                      //  print(fishes[i, x].GetInstanceID());
                        PlayAnimation(i, x, "Jump");
                        Animation anim = fishes[i, x].GetComponent<Animation>();
                        anim["Jump"].speed = Random.Range(0.8f, 1f);
                        sFrameAnimation[x] = Time.frameCount;
                    }
                    else
                        print("No jump attached.");
                }          
                jumpPointerLocation+=(int)animalInfo[i].GetJumpRate();
                cirInst[i] = jumpPointerLocation;
            }
        }
    }

    public void PlayAnimation(int i, int k, string name)
    {
        if (fishes[i, k] != null)
        {
            Animation curAnim = fishes[i, k].GetComponent<Animation>();
            curAnim.Play(name);
        }
    }

    public bool CheckAnimationPlaying(int i, int k)
    {
        float curFrameRate = 1.0f / Time.deltaTime;
        float multiplierFrameRate = curFrameRate / refFrameRate;
        timeFrame = Time.frameCount;
        diffTimeFrame = Time.frameCount - sFrameAnimation[k];
        if ((Time.frameCount - sFrameAnimation[k] * multiplierFrameRate) > animationDuration*30)
            return false;
        else
            return true;

       /* Animation curAnim = fishes[i, k].GetComponent<Animation>();
        if (curAnim.isPlaying)
            return true;
        else
            return false;*/
    }

    public void DeleteFish()
    {
        for(int i = 0; i < animalInfo.Length; i++)
        {
            for(int j = 0; j < max; j++)
            {
                if (fishes[i, j] == null)
                    break;
                Destroy(fishes[i, j]);
            }
        }
    }

    public bool SpawnWaypointFish(int curr)
    {
        float waterHeight = GameObject.FindGameObjectWithTag("Water").transform.position.y;
        //DeleteFish();
        currentWaypoint = curr; // Giving global access to the current waypoint

        if (currentWaypoint > this.GetComponent<WaypointManagement>().GetNumberOfWaypoints()-1)
            return false;

        for (int i = 0; i < animalInfo.Length; i++)
        {
           
            if (!animalInfo[i].GetWaypointSpawn(curr)) // If the animal is not supposed to spawned at this waypoint, skip to the next one
                continue;
            Vector3 diff = this.GetComponent<WaypointManagement>().GetWaypointArray()[curr].transform.position - this.GetComponent<WaypointManagement>().GetWaypointArray()[curr - 1].transform.position;

            float curRow = -1f, yAngleTemp = Random.Range(0f, 180f), curSpot = 0f,
                speed = animalInfo[i].GetSpeed() * 50f * Time.deltaTime; // Getting the speed, 50f was added as the fish would move too slow without it, Time.deltaTime was to reduce different device latency
            for (int j = 0; j < groupSize[i]; j++)
            {
                animationRate[j] = animalInfo[i].GetRandomAnimationSpeed();
                if (j % animalInfo[i].GetRowWidth() == 0)
                {
                    // This is to create a better pack like form for the fish
                    curRow += 1f;
                    curSpot = 0f;
                }
                if (animalInfo[i].GetGroupType())
                    yAngle[i, j] = yAngleTemp;
                else
                    yAngle[i, j] = yAngleTemp + Random.Range(-10f, 10f);
                if(animalInfo[i].GetPrefab() == null)
                {
                    this.GetComponent<ErrorHandeling>().DisplayError("No prefab available for " + animalInfo[i].GetName(), 10f);
                    break;
                }
                float xOffset = 0f, zOffset = 0f;
                if (diff.x > 0)
                    xOffset = -1*xOffsetcst;
                else
                    xOffset = xOffsetcst;

                fishes[i, j] = Instantiate(animalInfo[i].GetPrefab()); // Intiating the prefab into the environment
                fishes[i, j].GetComponent<Collider>().enabled = false;
                fishes[i, j].transform.localScale = Vector3.one * animalInfo[i].GetPrefabSizeChange() * animalInfo[i].GetRandomSize();
                Vector3 sLoc = new Vector3(500f * fishes[i,j].transform.localScale.x * curSpot + xOffset, -1f * animalInfo[i].GetDepth() + waterHeight - 2f, 50f * curRow);
                curSpot++;

                // Above line places it in its spot, a bit of randomness to this but its usually in a nice pack like form
                Vector3 myWaypoint = this.GetComponent<WaypointManagement>().GetWaypointArray()[curr].transform.position; // Get the waypoint so it spawns close to it
                myWaypoint.y = 0f;
                Vector3 spacing = fishes[i,j].GetComponent<Collider>().bounds.size;
                spacing.y = 0f;
              //  spacing.x *= animalInfo[i].GetXSpacing(); spacing.y = 0f; spacing.z *= animalInfo[i].GetZSpacing();
                fishes[i, j].transform.position = sLoc + myWaypoint + spacing; // Setting fish position correctly
               // yAngle[i, j] -= 180;
                fishes[i, j].transform.rotation = Quaternion.Euler(0f, yAngle[i,j]-180, 0f); // Setting the angle of the fish
                floors[i, j] = fishes[i, j].transform.position.y; // Getting the fishes' default bottom
                curJumper[i, j] = false;
                fishes[i,j].GetComponent<Rigidbody>().velocity = new Vector3(speed * Mathf.Sin(Mathf.Deg2Rad * yAngle[i, j]), 0f, speed * Mathf.Cos(Mathf.Deg2Rad * yAngle[i, j])); // Set the velocity of the fish depending on the multiplier and direction
                fishes[i,j].transform.SetParent(parent.transform);
            }
        }
        return true;
    }

    public void YMotion(int i, int j, bool up, bool stationary) // Modifys the y motion of the fish for jumping purposes
    {
        Vector3 curVelocity = fishes[i, j].GetComponent<Rigidbody>().velocity; // Getting the current velocity
        curVelocity.y += animalInfo[i].GetJumpSpeed() * Time.deltaTime * 10f; // Adding the jump
        if (!up)
            curVelocity.y *= -1; // If its supposed to go down instead
        else if (stationary)
            curVelocity.y = 0; // If its supposed to stop
        fishes[i, j].GetComponent<Rigidbody>().velocity = curVelocity; // Setting the motion
        Tilt(i, j); // Setting the tilt
    }

    public Vector3 Floor(Vector3 curPos, int i, int j) // Getting the floor vector of the fish
    {
        Vector3 newPos = new Vector3(curPos.x, floors[i, j], curPos.z);
        return newPos;
    }

    public bool ReachedCieling(int i, int j) // Check to see if its above the cieling
    {
        if (fishes[i, j].transform.position.y >= (0f + animalInfo[i].GetCieling()))
            return true;
        return false;
    }

    public bool IsGrounded(int i, int j) // Check to see if its below the ground
    {
        if (fishes[i, j].transform.position.y <= floors[i, j])
            return true;
        return false;
    }

    public void GroundIt(int i, int j) // Method to stop the y velocity 
    {
        fishes[i, j].GetComponent<Rigidbody>().velocity = StopVelocity(fishes[i, j].GetComponent<Rigidbody>().velocity);
        fishes[i, j].transform.position = incVector(fishes[i, j].transform.position, 0f, 0f, 0f, false);
        Tilt(i, j); // Tilt the fish back to the correct position
    }

    public void Tilt(int i, int j) // TIlt the fish to mimic a more natural jumping motion
    {
        float angle = 45f;
        if (!animalInfo[i].GetTiltType())
            angle = 15f;
        float curYVelocity = fishes[i, j].GetComponent<Rigidbody>().velocity.y; // Get the current y velocity to determine if its going up or down
        if (curYVelocity > 0) // Going up, tilt up
            fishes[i, j].transform.rotation = Quaternion.Euler(-1*angle, yAngle[i,j]-180, 0f);
        else if (curYVelocity < 0) // Going down, tilt down
            fishes[i, j].transform.rotation = Quaternion.Euler(angle, yAngle[i, j]-180, 0f);
        else // Just going, reset to normal tilt
            fishes[i, j].transform.rotation = Quaternion.Euler(0f, yAngle[i, j]-180, 0f);
    }

    public Vector3 incVector(Vector3 pos, float x, float y, float z, bool reset) // Easy method to incrment a vector or reset it to zero as needed
    {
        Vector3 newPos = new Vector3(pos.x + x, pos.y + y, pos.z + z);
        if (reset)
            newPos = Vector3.zero;
        return newPos;
    }

    public Vector3 StopVelocity(Vector3 velocity) // Stop y velocity when jumping
    {
        return new Vector3(velocity.x, 0f, velocity.z);
    }

    public Animals[] GetAnimals() { return animalInfo; }
    public int GetNumberOfAnimals() { return animalInfo.Length; }
    public Animals GetSpecificAnimal(int inst) { return animalInfo[inst];}
}
