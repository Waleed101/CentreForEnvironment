/*
 * Name: Animals (Animals.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-05-01
 * Last Modified: 2020-09-05
 * Used in: Waypoint Survey
 * Description: Scriptable object class used to allow for ease of customizability of the different species. New Animal asset file can easily be created by right clicking in the asset area and selecting Fish.
 * Double clicking the file created will pull up all the following options. These files can then be dragged into the Spawn Fish script object in Gamecontrol and then accessed by that script to spawn in the 
 * Play scene.
 * Status: PRODUCTION
 */

using UnityEngine;

[CreateAssetMenu(fileName = "New Animal", menuName = "Fish")]
public class Animals : ScriptableObject // Animal class to allow ease of creation of new species
{

    [Header("General Fish Information")]
    new public string name = "New Animal";
    public GameObject prefab = null;
    public Sprite picture = null;

    [Header("Spawn Settings")]
    public float speed = 10f;

    public Vector2 groupSize;
    public Vector2 depth;
    public float rowWidth = 10f;
    public Vector2 animationSpeed;
    public float mainSizeChange;
    public Vector2 randSizeChange;
    public float xSpacing = 1f, zSpacing = 1f;
    public bool swimAsAGroup = true;
    
    [Header("Jump Settings")]
    public bool isJumpAnimated = false;
    public bool tilt = false;
    public float jumpSpeed = 100f; // Normal movement speed, jump speed
    public float jumpRate = 1f;
    public float ceiling = 0.5f; // The max height it will jump above the surface

    [Header("Where to Spawn")]
    public bool[] waypoints = new bool[100]; // The boolean array that stores which waypoints it spawns it, needs to be manually modified with the addition fo new waypoints

    public string GetName() { return name;  } // Fish name, as defined returned as a string, used in record data and at end of flight when copied into clipboard
    public GameObject GetPrefab() { return prefab;  } // Prefab as supplied; prefab added needs to have a sphere collider, and legacy animation object with two animations ("Fast Swim" and "Jump"), Actual fish to be supplied
    public Sprite GetPicture() { return picture; } // Sprite as provided in inspector; used in the record data tab
    public float GetSpeed() { return speed;  } // Speed of the animal
    public float GetJumpSpeed() { return jumpSpeed; } // Jumpseed for non-animated jump; !!!WARNING!!! the non-animated jump was not used in the second-half of development, so functionality hasn't been tested fully and how it will work is questionable
    public float GetGroupSize() { return Random.Range(groupSize.x, groupSize.y); } // Returns a random number between the provided min and max group size; used to determine how many of the species to spawn
    public float GetMaxGroupSize() { return groupSize.y; } // Get largest possible groupsize; primarly used when creating arrays in older scripts
    public float GetMinGroupSize() { return groupSize.x; } // Get smallest possible groupsize
    public float GetDepth() { return Random.Range(depth.x, depth.y); } // Returns a random number between the provided min and max depth; used to determine the depth at which each individual instance of the species should spawn
    public float GetMinDepth() { return depth.x; } // Get lowest possible depth
    public float GetMaxDepth() { return depth.y; } // Get highest possible depth
    public float GetRowWidth() { return rowWidth; } // Get the row width; this essentially states how many in a "row" the program will put before pushing the next species to create a more natural group effect
    public float GetRandomSize() { return Random.Range(randSizeChange.x, randSizeChange.y); } // Returns a random number between the provided min and max size change; this directly affects the .localSize of each of the individual prefabs
    public float GetMinSize() { return randSizeChange.x; } // Get min size change
    public float GetMaxSize() { return randSizeChange.y; } // Get max size change
    public float GetRandomAnimationSpeed() { return Random.Range(animationSpeed.x, animationSpeed.y); } // Returns a random animation speed between the provided min and max animation speed; this directly affects the .speed (1 is normal speed), provides a more natural swimming
    public float GetMinAnimationSpeed() { return animationSpeed.x; } // Get min animation speed
    public float GetMaxAnimationSpeed() { return animationSpeed.y; } // Get max animation speed
    public float GetPrefabSizeChange() { return mainSizeChange; } // Gets the main size change, which is something that is applied to all the species of this type spawned 
    public float GetXSpacing() { return xSpacing; } // X spacing between each species, not currently used but can be incorporated to space out fish in the Spawn Fish script
    public float GetZSpacing() { return zSpacing; } // Z spacing between each species, not currently used but can be incorportaed to space out fish in the Spawn Fish script
    public float GetJumpRate() { return jumpRate; } // Get the number of species that will be jumping at a given time
    public bool GetTiltType() { return tilt; } // 
    public bool GetGroupType() { return swimAsAGroup; }
    public bool GetJumpAnimatedType() { return isJumpAnimated; } // If true, there is an animation attached to the prefab. If not, the script needs to tilt the species to model its jump. The second component has not been tested in recent iteration; so proceed with caution
    public float GetCieling() { return ceiling; } // Used in non-animated jump to set the height that the species will jump too
    public bool GetWaypointSpawn(int i) { if (i > waypoints.Length) { return false; } return waypoints[i]; } // Returns the specific instance of the waypoint array (waypoint array is a boolean array that speciifies whether or not this species will be spawned at a specific waypoint
    public bool[] GetWaypointSpawn() { return waypoints; } // Returns the whole waypoint array

}
