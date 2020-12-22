﻿using System;
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

    internal void GG(float elapsedTime, int p1Cups, int p2Cups)
    {
        System.Random r = new System.Random();
        int rng = r.Next(0, 1);
        elapsedTime = elapsedTime / 60;
        //Dersom det har vært en epic fight
        if(p1Cups < 3 && p2Cups < 3)
        {
            if(elapsedTime < 3.5 )
            {
                Say("gg_long");
                return;
            }else if(elapsedTime < 4.5)
            {
                if (rng == 0)
                    Say("gg_medium_1");
                else
                    Say("gg_medium_2");
                return;
            }
        }
        if (rng == 0)
            Say("gg_standard_1");
        else
            Say("gg_standard_2");

    }
}
