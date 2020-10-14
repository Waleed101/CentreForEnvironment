/*
 * Name: Objects (Objects.cs)
 * Created By: Waleed Sawan
 * Created On: 2020-06-12
 * Last Modified: 2020-09-05
 * Used in: Waypoint Survey
 * Description: Scriptable object class used to allow for ease of customizability of the different objects. New Object asset file can easily be created by right clicking in the asset area and selecting Object.
 * Double clicking the file created will pull up all the following options. These files can then be dragged into the Spawn Object script object in Gamecontrol and then accessed by that script to spawn in the 
 * Play scene.
 * Status: PRODUCTION
 */
 using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Object")]
public class Objects : ScriptableObject // Object class to allow ease of creation of new objects to add to the scene
{
    new public string name = "New Object";
    public GameObject prefab = null;
    public float speed = 10f, maxWaypointSize = 10f, minWaypointSize = 3f;
    public float aboveSeaLevelMin = 0f, aboveSeaLevelMax = 5f, angleOffset = 0f;
    public float prefabSize = 3f;
    public bool applyVelocity = true;

    public string GetName() { return name; } // Get name, not used for anything but easier for reference
    public GameObject GetPrefab() { return prefab; } // Get the prefab
    public float GetSpeed() { return speed; } // Get set speed of the object
    public bool GetMovement() { return applyVelocity; } // Get whether or not top apply movement
    public float GetAngleOffset() { return angleOffset; } // Get the angle offset to adjust for proper animation
    public float GetWaypointSpawnRate() { return Random.Range(minWaypointSize, maxWaypointSize); } // Get number to spawn
    public float GetMaxWaypointSpawnRate() { return maxWaypointSize; } // Get max number to spawn
    public float GetMinWaypointSpawnRate() { return minWaypointSize; } // Get min number to spawn
    public float GetRandomSeaLevel() { return Random.Range(aboveSeaLevelMin, aboveSeaLevelMax); } // Get height to spawn the boat
    public float GetMaxSeaLevel() { return aboveSeaLevelMax; } // Get max height of the boat
    public float GetMinSeaLevel() { return aboveSeaLevelMin; } // Get min height of the boat
    public float GetPrefabSizeChange() { return prefabSize; } // Get size change of the prefab to adjust the boat
}