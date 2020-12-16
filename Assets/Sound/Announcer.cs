using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Announcer : MonoBehaviour
{
    public SoundFX[] voicelines;

    public void Awake()
    {
        foreach (var s in voicelines)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.pitch = s.pitch;
            s.source.volume = s.volume;
            
        }
    }

    public void Dissapointed()
    {
        string[] dissapointedLines = { "MyMomWouldDoBetter" };

    }

    public void Say(string voiceline)
    {
        var sound = GetSound(voiceline);
        if (sound == null) return;
        Debug.Log("Skal finne voicelinen: " + voiceline);
        sound.source.Play();
    }

    public SoundFX GetSound(string name)
    {
        foreach (var s in voicelines)
        {
            if (s.name == name)
                return s;
        }
        Debug.LogError("Sound  [" + name + "] was not found!");
        return null;
    }

}
