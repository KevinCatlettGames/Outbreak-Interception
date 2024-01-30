using UnityEngine;

public class OptionsDataHandler : DataHandler
{
    public static OptionsDataHandler instance;

    int resolutionIndex;
    public int ResolutionIndex { get { return resolutionIndex; } set { resolutionIndex = value; } }

    int qualitySettingsIndex = 2;
    public int QualitySettingsIndex { get { return qualitySettingsIndex; } set { qualitySettingsIndex = value; } }

    bool fullscreen = true;
    public bool FullscreenValue { get { return fullscreen;} set { fullscreen = value; } }

    public bool valuesSet;

    void Awake()
    {
        // Setting the singleton.
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public override void RecieveData(SaveData saveData)
    {
       this.resolutionIndex = saveData.resolutionIndex;
       this.qualitySettingsIndex = saveData.qualitySettingsIndex;
       this.fullscreen = saveData.fullscreen;
       valuesSet = true;
    }

    public override void SendData(SaveData saveData)
    {
       saveData.resolutionIndex = resolutionIndex;
       saveData.qualitySettingsIndex = qualitySettingsIndex;
       saveData.fullscreen = fullscreen;
        valuesSet = true;
    }

}
