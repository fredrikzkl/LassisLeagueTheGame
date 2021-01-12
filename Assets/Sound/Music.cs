using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Music : SoundManager
{
    public bool playMusic = true;
    public float musicInfoBarDisplayTime = 4f;

    public GameObject musicPlayingInfoBar;
    CanvasGroup musicCanvas;
    TMP_Text artistTitle;
    TMP_Text songTitle;

    List<string> playedTracks;
    string currentTrack;
    System.Random random;

    float musicBarTimeRemaining;

    private void Start()
    {
        currentTrack = "";
        playedTracks = new List<string>();
        random = new System.Random();

        if(musicPlayingInfoBar == null)
        {
            Debug.Log("Music does not an assigned canvas group");
        }
        else
        {
            musicCanvas = musicPlayingInfoBar.GetComponent<CanvasGroup>();
            songTitle = musicPlayingInfoBar.transform.Find("PanelGroup/SongTitle").GetComponent<TMP_Text>();
            artistTitle = musicPlayingInfoBar.transform.Find("PanelGroup/SongArtist").GetComponent<TMP_Text>();
        }

        if(playMusic)
            PlayRandomTrack();
    }

    public void PlayRandomTrack()
    {
        if (CheckIfAllTracksHaveBeenPlayed())
        {
            playedTracks.Clear();
        }

        string pickedTrack = "";
        int maxRandomPicks = 20;
        for(int i = 0; i < maxRandomPicks; i++)
        {
            //Dersom vi har prøvd 20 ganger, velger vi bare en helt random sang, ved å først klarere played songs
            if(i == maxRandomPicks)
            {
                playedTracks.Clear();
            }
            int nextTrack = random.Next(0, sounds.Length);
            pickedTrack = sounds[nextTrack].name;
            if (pickedTrack.Contains(pickedTrack))
                continue;
            break;
        }
        currentTrack = pickedTrack;
        UpdateTextMusicCanvas(pickedTrack);
        PlaySound(pickedTrack);
        FadeInMusicCanvas();
       
    }

    private void UpdateTextMusicCanvas(string name)
    {
        var decomposed = DecomposeName(name);
        artistTitle.text = decomposed[0];
        songTitle.text = decomposed[1];
    }

    private string[] DecomposeName(string name)
    {
        string[] artistTitle = name.Split('#');
        return artistTitle;
    }


    private bool CheckIfAllTracksHaveBeenPlayed()
    {
        return (playedTracks.Count == sounds.Length);
    }


    private void FixedUpdate()
    {
        float fadeRate = 0.025f;
        if (musicBarTimeRemaining <= 0)
            musicCanvas.alpha -= fadeRate;
        else
            musicCanvas.alpha += fadeRate;

        musicBarTimeRemaining -= Time.deltaTime;

        //Checks if song has stopped playin
        if (!IsPlaying(currentTrack))
        {
            PlayRandomTrack();
        }
    }

    private void FadeInMusicCanvas()
    {
        musicBarTimeRemaining = musicInfoBarDisplayTime;
    }

}
