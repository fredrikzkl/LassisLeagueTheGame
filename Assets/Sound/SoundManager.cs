using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundFX[] sounds;

    public void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
        }
    }

    

    public void PlaySoundEffect(string name)
    {
        var sound = GetSound(name);
        if (sound == null) return;
        Debug.Log("Skal spille lyden: " + name);
        sound.source.Play();
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

}
