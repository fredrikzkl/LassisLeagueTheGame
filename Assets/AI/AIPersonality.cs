using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPersonality
{
    //Sjangsen for å treffe
    float HitRate;
    //Når starter en runde, om han velger den koppen med flest naboer, eller random
    bool PicksOptimalCup;
    //Hvor mange kopper han har bommet på før han bestemmer seg for å restacke
    int MissThreshold;
    //Hvor stor andel av vansklige kopper som må finnes før restack
    float HardCupsThreshold;
    //Hva han definerer som en vansklig kopp. Hvor mange naboer. Feks 2, 2 eller mindre naboer er vansklig
    int HardCupsDefintion;
    //Hvis noe er ansett som den optimale løsningen, hva er sjangsen for at han vil gjøre den
    float DoubtFactor;

    public AIPersonality Easy()
    {
        return new AIPersonality
        {
            HitRate = 0.4f,
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
            HitRate = 0.45f,
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
            HitRate = 0.55f,
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
