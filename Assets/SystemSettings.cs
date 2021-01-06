using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SystemSettings : MonoBehaviour
{

    //Refs
    public TMP_Dropdown resolutionDropdown;
    //
    Resolution[] availableResolutions;
    

    void Start()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            var tempRes = availableResolutions[i];
            if (isCurrentResolution(tempRes))
                currentResolutionIndex = i;

            options.Add(tempRes.ToString());
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    bool isCurrentResolution(Resolution temp)
    {
        var currentResoltuon = Screen.currentResolution;
        bool sameWidth = temp.width == currentResoltuon.width;
        bool sameHeighht = temp.height == currentResoltuon.height;
        if (sameHeighht && sameHeighht) return true;
        return false;
    }


    public void SetResolution(int resolutionIndex)
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        Resolution res = availableResolutions[resolutionIndex]; 
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    
    public void SetQuality(int qualityIndex)
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullSCreen)
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        Screen.fullScreen = isFullSCreen;
    }

}
