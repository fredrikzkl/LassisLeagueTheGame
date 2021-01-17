using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPersonality
{
    //Sjangsen for å treffe
    public float XZAimRate;
    public float PreferredXYAimAngle;
    public float XYAimRate;
    public float PowerRate;
    //Når starter en runde, om han velger den koppen med flest naboer, eller random
    public bool PicksOptimalCup;
    //Hvor mange kopper han har bommet på før han bestemmer seg for å restacke
    public int MissThreshold;
    //Hvor stor andel av vansklige kopper som må finnes før restack
    public float HardCupsThreshold;
    //Hva han definerer som en vansklig kopp. Hvor mange naboer. Feks 2, 2 eller mindre naboer er vansklig
    public int HardCupsDefintion;
    //Hvis noe er ansett som den optimale løsningen, hva er sjangsen for at han vil gjøre den
    public float DoubtFactor;

    public AIPersonality Easy()
    {
        return new AIPersonality
        {
            XZAimRate = 0.05f,
            PreferredXYAimAngle = 0.95f,
            XYAimRate = 0.1f,
            PowerRate = 0.05f,

            PicksOptimalCup = false,
            MissThreshold = 8,
            HardCupsThreshold = 0.4f,
            HardCupsDefintion = 3,
            DoubtFactor = 0.3f
        };
    }

    public AIPersonality Standard()
    {
        return new AIPersonality
        {
            XZAimRate = 0.025f,
            PreferredXYAimAngle = 0.78f,
            XYAimRate = 0.1f,
            PowerRate = 0.025f,

            
            PicksOptimalCup = false,
            MissThreshold = 6,
            HardCupsThreshold = 0.3f,
            HardCupsDefintion = 2,
            DoubtFactor = 0.25f
        };
    }

    public AIPersonality Hard()
    {
        return new AIPersonality
        {
            XZAimRate = 0.02f,

            PreferredXYAimAngle = 0.55f,
            XYAimRate = 0.1f,

            PowerRate = 0.02f,

            PicksOptimalCup = true,
            MissThreshold = 4,
            HardCupsThreshold = 0.25f,
            HardCupsDefintion = 2,
            DoubtFactor = 0.1f
        };
    }




    //Rng element som gjør at han noen ganger tviler, uten grunn
    public bool Doubt()
    {
        System.Random rng = new System.Random();
        int doubting = rng.Next(0, 101);
        int factor = (int)(DoubtFactor * 100);
        return doubting > factor;
    }

    public AIPersonality GetPersonality(DifficultyLevel level)
    {
        switch (level)
        {
            case DifficultyLevel.Easy:
                return Easy();

            case DifficultyLevel.Hard:
                return Hard();

            default:
                return Standard();
        }
    }
    
}
