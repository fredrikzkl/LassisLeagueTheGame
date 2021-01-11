using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SystemSettingsData 
{

    public int screenHeight { get; set; }
    public int screenWidth { get; set; }
    public bool fullscreen { get; set; }
    public int graphicsSetting { get; set; }
    

    public float masterVolume { get; set; }
    public float sfxVolume { get; set; }
    public float announcerVolume { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        var settings = SaveSystem.LoadSystemSettings();
        Screen.SetResolution(settings.screenWidth, settings.screenHeight, settings.fullscreen);
        QualitySettings.SetQualityLevel(settings.graphicsSetting);
    }


    public void ApplyStandardSettings()
    {
        SetResolution(Screen.currentResolution);
            
        fullscreen = true;
        graphicsSetting = QualitySettings.GetQualityLevel();

        masterVolume = 0f;
        sfxVolume = 0f;
        announcerVolume = 0f;
    }

    public void SetResolution(Resolution r)
    {
        screenHeight = r.height;
        screenWidth = r.width;
    }
}
