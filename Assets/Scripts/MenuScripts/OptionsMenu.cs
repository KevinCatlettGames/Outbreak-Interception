using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using System.Collections;

/// <summary>
/// Functionality for the options menu.
/// Functionality for audio management.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    [Header("Audio mixer")]
    [Tooltip("The audio mixer for managing sound volumes.")]
    [SerializeField] AudioMixer audioMixer;
    
    [Header("Volume sliders")]
    [Tooltip("The global volume slider.")]
    [SerializeField] Slider globalVolumeSlider;
    
    [Tooltip("The music volume slider.")]
    [SerializeField] Slider musicVolumeSlider;
    
    [Tooltip("The sound volume slider.")]
    [SerializeField] Slider soundVolumeSlider;

    [SerializeField] int sceneIndex;

    AudioSource audioSource;

    // Save and load singleton.
    SaveSystem saveAndLoad;
    
    // Sound data handler singleton.
    SoundDataHandler soundDataHandler;
    OptionsDataHandler optionsDataHandler;

    // Constant sound mixer group names. 
    const string Master = "Master";
    const string Music = "Music";
    const string Sound = "SFX";

    //Resolution[] resolutions;
    //[SerializeField] TMPro.TMP_Dropdown resolutionDropdown;
    //[SerializeField] TMPro.TMP_Dropdown qualityDropdown;
    //[SerializeField] Toggle fullscreenToggle;

    //public int width, height;

    #region Methods

    //private void Awake()
    //{
    //    width = Screen.currentResolution.width;
    //    height = Screen.currentResolution.height;
    //}

    private void Start()
    {

        if (SoundDataHandler.instance)
            soundDataHandler = SoundDataHandler.instance;

        if (OptionsDataHandler.instance)
            optionsDataHandler = OptionsDataHandler.instance;

        if (SaveSystem.instance)
            saveAndLoad = SaveSystem.instance;

        if (GetComponent<AudioSource>())
            audioSource = GetComponent<AudioSource>();
        OptionsInitialization();
    }

    //private void Update()
    //{
    //    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    //    Screen.SetResolution(width, height, true);
    //}

    /// <summary>
    /// Updates the slider values to represent the volumes of the sound mixer groups.
    /// Sets the onValueChanged events on the volume sliders to update the values of the soundDataHandler.
    /// </summary>
    public void OptionsInitialization()
    {
        if (!soundDataHandler)
        {
            return;
        }

        musicVolumeSlider.value = soundDataHandler.MusicVolume;

        soundVolumeSlider.value = soundDataHandler.SoundVolume;

        globalVolumeSlider.value = soundDataHandler.GlobalVolume;
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        globalVolumeSlider.onValueChanged.AddListener(SetMasterVolume);


    }
    //    resolutions = Screen.resolutions;

    //    List<string> options = new List<string>();

    //    int currentResolutionIndex = 0;

    //    for (int i = 0; i < resolutions.Length; i++)
    //    {
    //        string option = resolutions[i].width + " x " + resolutions[i].height;

    //        if (!options.Contains(option))
    //        {
    //            options.Add(option);
    //        }

    //        if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
    //        {
    //            currentResolutionIndex = i;
    //        }
    //    }

    //    resolutionDropdown.AddOptions(options);

    //    if (!optionsDataHandler)
    //    {
    //        return;
    //    }

    //    if (!optionsDataHandler.valuesSet)
    //    {
    //        resolutionDropdown.value = currentResolutionIndex;
    //        resolutionDropdown.RefreshShownValue();
    //    }
    //    else
    //    {
    //        StartCoroutine(ChangeDropdownValue(optionsDataHandler.ResolutionIndex));
    //        qualityDropdown.value = optionsDataHandler.QualitySettingsIndex;
    //        fullscreenToggle.isOn = optionsDataHandler.FullscreenValue;
    //    }
    //}

    /// <summary>
    /// Sets the master volume in the audioMixer and soundDataHandler.
    /// </summary>
    /// <param name="value"></param> The value that should be used.
    public void SetMasterVolume(float value)
    {
        SetVolume(value, Master);
        SetVolume(value, Music);
        SetVolume(value, Sound);

        musicVolumeSlider.value = soundDataHandler.MusicVolume;
        soundVolumeSlider.value = soundDataHandler.SoundVolume;

    }
    
    /// <summary>
    /// Sets the music volume in the audioMixer and soundDataHandler.
    /// </summary>
    /// <param name="value"></param> The value that should be used.
    public void SetMusicVolume(float value)
    {
        SetVolume(value,Music);
    }
  
    /// <summary>
    /// Sets the sound volume in the audioMixer and soundDataHandler.
    /// </summary>
    /// <param name="value"></param> The value that should be used.
    public void SetSoundVolume(float value)
    {
        SetVolume(value, Sound);
    }

    /// <summary>
    /// Sets the audioMixer value in a audioMixer group and updates the soundDataHandler.
    /// </summary>
    /// <param name="value"></param> The value that should be used.
    /// <param name="mixerGroup"></param> The mixer group where the value should be updated in. 
    void SetVolume(float value, string mixerGroup)
    {

        float tempValue = Mathf.Log10(value) * 20; // Changes the value parameter to function with the audioMixer.

        if (audioMixer != null)
        {
            if (value == 0)
                audioMixer.SetFloat(mixerGroup, -80);
            else
                audioMixer.SetFloat(mixerGroup, tempValue);
        }

        switch (mixerGroup)
        {
            case Master:
                soundDataHandler.GlobalVolume = value;
                break;
            case Music:
                soundDataHandler.MusicVolume = value;
                break;
            case Sound:
                soundDataHandler.SoundVolume = value;
                break;
        }
    }
    public void UnloadOptions()
    {
        if(saveAndLoad)
        {
            saveAndLoad.Save();
        }

        SceneManager.UnloadSceneAsync(sceneIndex);
    }
    public void PlaySound(AudioClip clip)
    {
        if(audioSource)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    //public void SetQuality(int qualityIndex)
    //{
    //    QualitySettings.SetQualityLevel(qualityIndex);
    //    optionsDataHandler.QualitySettingsIndex = qualityIndex;
    //}

    //public void SetFullscreen(bool isFullscreen)
    //{
    //    Screen.fullScreen = isFullscreen;
    //    optionsDataHandler.FullscreenValue = isFullscreen;
    //}

    //public void SetResolution(int resolutionIndex)
    //{
    //    Resolution resolution = resolutions[resolutionIndex];
    //    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    //    optionsDataHandler.ResolutionIndex = resolutionIndex;
    //}

    //IEnumerator ChangeDropdownValue(int newValue)
    //{
    //    resolutionDropdown.Select();
    //    yield return new WaitForEndOfFrame();
    //    resolutionDropdown.value = newValue;
    //    resolutionDropdown.RefreshShownValue();
    //}
    #endregion 
}
