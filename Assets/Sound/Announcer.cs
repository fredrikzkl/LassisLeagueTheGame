using System;
using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Announcer : SoundManager
{
    System.Random rng = new System.Random();
    

    string[] worstDissapointments = { "MomWouldDoBetter", "Embarrassing" };
    string[] regularDissapointments = { "EvenTrying_1", "EvenTrying_2" };

  
    public void Say(string voiceline)
    {
   
        var sound = GetSound(voiceline);
        if (sound == null) return;
        Debug.Log("Skal finne voicelinen: " + voiceline);
        sound.source.Play();
    }

    public void Dissapointed(int missStreak, int possibleCups)
    {
        int commentChance = 10 - possibleCups;
        if(commentChance <= 0)
            commentChance = 1;

        int commentRng = rng.Next(0, commentChance);

        if(commentRng == 1 || commentChance == 1)
        {
            //Worst
            if (missStreak > 6)
            {
                string dissapointmentLine = worstDissapointments[rng.Next(0, worstDissapointments.Length)];
                Say(dissapointmentLine);
                return;
            }
            if(missStreak > 4)
            {
                string dissapointmentLine = regularDissapointments[rng.Next(0, regularDissapointments.Length)];
                Say(dissapointmentLine);
                return;
            }
        }

        
    }

    public void BitchCup()
    {
        switch(rng.Next(0, 5))
        {
            case 0:
                Say("BitchCupBitchLikeYou");
                break;
            default:
                Say("BitchCupNormal");
                break;
        }
    }

    public void CloseCall()
    {
        switch (rng.Next(0,2))
        {
            case 1:
                Say("HowCloseWasThat");
                break;
            default:
                Say("Shitsnitzel");
                break;
        }
    }

    //Island + begge i samme kopp
    public void PerfectRound()
    {
        Say("RingaDingDingDong");
    }

    public void DoubleHit()
    {
        Say("DoubleHit");
    }

    public void BallsBack()
    {
        Say("BallsBack");
    }


    internal void GG(float elapsedTime, int p1Cups, int p2Cups)
    {
       
        int random = rng.Next(0, 1);
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
                if (random == 0)
                    Say("gg_medium_1");
                else
                    Say("gg_medium_2");
                return;
            }
        }
        if (random == 0)
            Say("gg_standard_1");
        else
            Say("gg_standard_2");

    }
}
