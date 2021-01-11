using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public SoundFX[] sounds;

    public void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = audioMixer.FindMatchingGroups(s.group.ToString())[0];
        }
    }

    private void Start()
    {
        LoadMixerFromSettingsFile();
    }


    public void PlaySoundEffect(string name)
    {
        PlaySoundEffect(name, 1f);
    }

    public void PlaySoundEffect(string name, float volume)
    {
        var sound = GetSound(name);
        if (sound == null) return;
        sound.volume = volume;
        
        Debug.Log("Skal spille lyden: " + name + " med volum: " + sound.volume);
        
        sound.source.Play();
    }

    public void PlayerSoundEffects(string[] names)
    {
        foreach(var n in names)
        {
            PlaySoundEffect(n);
        }
    }

    public SoundFX GetSound(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
                return s;
        }
        Debug.LogError("Sound  [" + name + "] was not found!");
        return null;
    }

    /*
     *  MIXER
     */
     
    void LoadMixerFromSettingsFile()
    {
        var settings = SaveSystem.LoadSystemSettings();
        SetMasterVolume(settings.masterVolume);
        SetSFXVolume(settings.sfxVolume);
        SetAnnouncerVolume(settings.announcerVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
      
    }

    public void SetAnnouncerVolume(float volume)
    {
        audioMixer.SetFloat("AnnouncerVolume", volume);
        
    }

}
