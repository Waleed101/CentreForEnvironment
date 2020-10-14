using UnityEngine;

[CreateAssetMenu(fileName = "New Audio", menuName = "AudioClip")]
public class Speaker : ScriptableObject // Animal class to allow ease of creation of new species
{
    public string speakerName = "Name";
    public AudioClip clip = null;
    public Sprite personPicture = null;
    public int waypoint = 0;
    public float volume = 1f;

    public string GetName() { return speakerName; }
    public AudioClip GetAudioClip() { return clip; }
    public Sprite GetPersonPicture() { return personPicture; }
    public int GetWaypointID() { return waypoint; }
    public float GetVolume() { return volume; }
}