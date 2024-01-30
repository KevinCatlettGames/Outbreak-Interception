using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// A singleton handling sound.
/// Deriving from Data Handler class allows communication with the games save system.
/// </summary>
public class SoundDataHandler : DataHandler
{
    public static SoundDataHandler instance;

    [Tooltip("Stores the global volume.")]
    [SerializeField] float globalVolume = 1; 
    public float GlobalVolume { get { return globalVolume; } set { globalVolume = value; } }

    [Tooltip(" Stores the current volume of ingame music.")]
    [SerializeField] float musicVolume = 1;
    public float MusicVolume { get { return musicVolume; } set { musicVolume = value; } }

    [Tooltip("Stores the current volume of ingame sound.")]
    [SerializeField] float soundVolume = 1;
    public float SoundVolume { get { return soundVolume; } set { soundVolume = value; } }

    [Tooltip("The audio mixer controlling game sound volume.")]
    [SerializeField] AudioMixer audioMixer;
    
    const string Master = "Master";
    const string Music = "Music";
    const string Sound = "SFX";
    
    #region Methods
    void Awake()
    {
        // Setting the singleton.
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    public override void RecieveData(SaveData saveData)
    {
        this.globalVolume = saveData.globalVolume;
        this.musicVolume = saveData.musicVolume;
        this.soundVolume = saveData.soundVolume;
        SetAudioMixer();
    }

    public override void SendData(SaveData saveData)
    {
        saveData.globalVolume = this.globalVolume;
        saveData.musicVolume = this.musicVolume;
        saveData.soundVolume = this.soundVolume;
    }

    void SetAudioMixer()
    {
        //if(!audioMixer) return;

        //audioMixer.SetFloat(Master, Mathf.Log10(globalVolume) * 20);

        //if (globalVolume == 0)
        //{
        //    audioMixer.SetFloat(Master, -80);
        //}

        //audioMixer.SetFloat(Music, Mathf.Log10(musicVolume) * 20);

        //if (musicVolume == 0)
        //{
        //    audioMixer.SetFloat(Music, -80);
        //}

        //audioMixer.SetFloat(Sound, Mathf.Log10(soundVolume) * 20);

        //if (soundVolume == 0)
        //{
        //    audioMixer.SetFloat(Sound, -80);
        //}
    }
    #endregion 

}
