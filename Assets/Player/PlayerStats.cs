using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats
{
    
    int hit;
    int miss;

    public int missStreak;
    public int hitStreak;

    public int longestHitStreak;
    public int longestMissStreak;

    public int bonusCups;

    public PlayerStats()
    {
        hit = 0;
        miss = 0;
        missStreak = 0;
        hitStreak = 0;
        longestHitStreak = 0;
        longestMissStreak = 0;
        bonusCups = 0;
    }

    public double GetHitRate()
    {
        return System.Math.Round(hit / (double)GetThrowCount(), 2);
    }

    public int GetThrowCount()
    {
        return hit + miss;
    }

    public void AddHit(int hits)
    {
        hit += hits;
        hitStreak += hits;
        if(missStreak > longestMissStreak)
        {
            longestMissStreak = missStreak;
        }
        missStreak = 0;
    }

    public void AddMiss(int misses)
    {
        miss += misses;
        missStreak += misses;
        if (hitStreak > longestHitStreak)
        {
            longestHitStreak = hitStreak;
        }
        hitStreak = 0;
    }

    public void AddBonusCups(int bonusCups)
    {
        bonusCups += bonusCups;
    }

    public int GetLongestHitStreak()
    {
        if (hitStreak > longestHitStreak) return hitStreak;
        return longestHitStreak;
    }

    public int GetLongestMissStreak()
    {
        if (missStreak > longestMissStreak) return missStreak;
        return longestMissStreak;
    }

    override
    public string ToString()
    {
        return "Hits: " + hit + "\nMiss: " + miss + " \nLongest hit streak: " + GetLongestHitStreak() + "\nLongest miss streak" + GetLongestMissStreak();
    }

    
}
