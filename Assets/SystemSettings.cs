using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SystemSettings : MonoBehaviour
{

    //Refs
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicQualityDropdown;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider announcerSlider;
    public Slider musicSlider;
    
    public AudioMixer audioMixer;
    //
    Resolution[] availableResolutions;
    //SaveFile
    SystemSettingsData saveData;



    void Start()
    {
        //Henter ut alle muligheten for skjermen
        availableResolutions = Screen.resolutions;
        //Laster inn data fra systemfilen
        saveData = SaveSystem.LoadSystemSettings();
        ApplySettingsToUI(saveData);
        
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            var tempRes = availableResolutions[i];
            if (isCurrentResolution(tempRes))
                currentResolutionIndex = i;

            options.Add(ResToString(tempRes));
          
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private string ResToString(Resolution r)
    {
        return r.width + " x " + r.height;
    }

    bool isCurrentResolution(Resolution temp)
    {
        var currentResoltuon = Screen.currentResolution;
        return isSameResolution(temp, currentResoltuon);
    }

    bool isSameResolution(Resolution r1, Resolution r2)
    {
        bool sameWidth = r1.width == r2.width;
        bool sameHeighht = r1.height == r2.height;
        if (sameHeighht && sameHeighht) return true;
        return false;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        saveData.masterVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        saveData.sfxVolume = volume;
    }

    public void SetAnnouncerVolume(float volume)
    {
        audioMixer.SetFloat("AnnouncerVolume", volume);
        saveData.announcerVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        saveData.musicVolume = volume;
    }



    public void SetResolution(int resolutionIndex)
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        Resolution res = availableResolutions[resolutionIndex]; 
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        saveData.SetResolution(res);
    }
    
    public void SetQuality(int qualityIndex)
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        QualitySettings.SetQualityLevel(qualityIndex);
        saveData.graphicsSetting = qualityIndex;
    }

    public void SetFullScreen(bool isFullSCreen)
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        Screen.fullScreen = isFullSCreen;
        saveData.fullscreen = isFullSCreen;
    }

    public void SaveSystemSettings()
    {
        SaveSystem.SaveSystemSettings(saveData);
    }

    public void ApplySettingsToUI(SystemSettingsData settings)
    {
        graphicQualityDropdown.value = settings.graphicsSetting;

        SetMasterVolume(settings.masterVolume);
        masterSlider.value = settings.masterVolume;

        SetSFXVolume(settings.sfxVolume);
        sfxSlider.value = settings.sfxVolume;

        SetAnnouncerVolume(settings.announcerVolume);
        announcerSlider.value = settings.announcerVolume;

        SetMusicVolume(settings.musicVolume);
        musicSlider.value = settings.musicVolume;
    }

}
