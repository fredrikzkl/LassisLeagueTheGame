using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundFX 
{
    public string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;

    [Range(0f,5f)]
    public float pitch;

    public SoundGroup group;
    
    [HideInInspector]
    public AudioSource source;
}

[System.Serializable]
public enum SoundGroup
{
    SFX, Announcer, Music
}