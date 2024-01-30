using System;
using UnityEngine;

/// <summary>
/// This class is used to save all information that needs to be saved and gets passed from and to the save system to save and set values.
/// </summary>
[Serializable]
public class SaveData
{   
    public float globalVolume; // Stores the global volume.
   

    public float musicVolume; // Stores the current volume of ingame music.
   

    public float soundVolume; // Stores the current volume of ingame sound.

    public int resolutionIndex;

    public int qualitySettingsIndex = 2;

    public bool fullscreen = true;
}
