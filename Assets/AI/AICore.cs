using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Formations;

public enum DifficultyLevel
{
    Easy = 0, Standard = 1, Hard = 2
}

public class AICore : MonoBehaviour
{
    public DifficultyLevel difficulty;
    public bool DebugDecisions;
    System.Random rng = new System.Random();

    //Refs
    private PlayerRoundHandler roundHandler;
    private CupRack opponentRack;


    private void Start()
    {
        roundHandler = GetComponent<PlayerRoundHandler>();
        opponentRack = FindObjectOfType<GameLogic>().GetOpponentCupRack(gameObject);
    }

    public ThrowData CalculateShot(Transform origin, Transform target, int direction)
    {
        ThrowData shot = new ThrowData();
        //Henter ut XZ vinkelen, som er konstant
        shot.XZAngle =  GetXZAngleBetweenHandAndCup(origin.position, target.position);
        //TODO: Setter XY angle som 45 alltid
        shot.XYAngle = 0.1f;
        //TODO: Setter power random fra 60-90 (fordi hvorfor ikke)
        //shot.Power = rng.Next(75, 88);
        shot.Power = GetInitialVelocityXY(shot.XYAngle, origin, target);

        return shot;
    }

    public float GetInitialVelocityXY(float angle, Transform origin, Transform target)
    {
        float cupHeight = target.gameObject.GetComponent<Renderer>().bounds.size.y;

        float distance = Vector3.Distance(origin.position, target.position);
        float initialHeight = origin.position.y ;
        float endHeight = target.position.y + cupHeight/2;


        float teller = (0.5f * Physics.gravity.y * Mathf.Pow(distance, 2)) / Mathf.Pow(Mathf.Sin(angle), 2);
        float nevner = endHeight - initialHeight - (Mathf.Cos(angle) / Mathf.Sin(angle)) * distance;

        return Mathf.Sqrt(teller / nevner);

    }
    
    public bool DecidesIsland(List<GameObject> islandCups)
    {
        int pickedIslandCup = rng.Next(0, islandCups.Count);
        roundHandler.InvokeIsland(islandCups[pickedIslandCup]);
        return true;
    }
    
    
    public void DecideRestack()
    {
        int opponentCupCount = opponentRack.GetCupCount();
        var availableFormations = GetValidFormations(opponentCupCount);

        if (availableFormations.Count == 0)
            return;


        NeighbourTable nt = opponentRack.GetNeighbourTable();
        //Hard cups er kopper som er 1 eller mindre naboer
        int hardCups = 0;
        foreach(var cup in opponentRack.GetCupList())
        {
            if(nt.GetNeighbourCount(cup) < 2)
            {
                hardCups++;
            }
        }
        //Så om andelen av vanskligere kopper er over 30%, så restackes det
        float hardcupsThreshold = 0.28f;
        bool tooManyHardCups = (hardCups / opponentCupCount) > hardcupsThreshold;
        if (DebugDecisions && tooManyHardCups)
            Debug.Log(gameObject + " decides that there are too many hard cups [" + hardCups + "], and  " + (hardCups/opponentCupCount) + " of the cups are hard. Treshold: " + hardcupsThreshold);


        //Dersom han har bommet nok ganger, så restacker han
        int missStreakThreshold = 5;
        bool missedToMuch = missStreakThreshold < roundHandler.stats.missStreak;
        if (DebugDecisions && missedToMuch)
            Debug.Log(gameObject + " missed too much, and wants a restack (Missed " + roundHandler.stats.missStreak);
       

        if (tooManyHardCups || missedToMuch)
        {
            Formation pickedFormation = PickRandomFormation(availableFormations);
            opponentRack.Rerack(pickedFormation.FormationString);
            return;
        }
        return;
    }

    public Formation PickRandomFormation(List<Formation> availableFormations)
    {
        int selectedFormation = rng.Next(0, availableFormations.Count);
        return availableFormations[selectedFormation];
    }
    

    public GameObject PickACup()
    {
        if (opponentRack.GetCupCount() == 0)
            return null;
        //Sjekker om han har island
        if (gameObject.GetComponent<PlayerController>().nextThrowIsIsland)
        {
            foreach(var cup in opponentRack.GetCupList())
            {
                if (cup.GetComponent<CupController>().isIslandCup)
                    return cup;
            }
        }

        //Sjekker først om han har truffet noen baller
        if(difficulty > 0 && roundHandler.HitCups.Count > 0)
        {
            foreach(var hitCup in roundHandler.HitCups)
            {
                if(hitCup != null)
                {
                    if (DebugDecisions)
                        Debug.LogWarning(gameObject + " AI selectes previously hit cup " + hitCup.name);
                    return hitCup;
                }
                    
            }
        }
        //Velger utifra vansklighetsgrad
        if((int)difficulty > 1)
        {
            return PickOptimalCup(opponentRack);
        }

        return PickRandomCup(opponentRack); 
    }

    //Optimale koppen er den som har flest naboer
    public GameObject PickOptimalCup(CupRack opponentRack)
    {
        NeighbourTable nt = opponentRack.GetNeighbourTable();
        //Velger bare en tilfeldig kopp som den beste
        GameObject pickedCup = opponentRack.GetCupList()[0];
        int pickedCupNeighbourCount = nt.GetNeighbourCount(pickedCup);
        //Ser igjennom koppene for å finne en som har flere naboer
        foreach (var cup in opponentRack.GetCupList())
        {
            var tempNeighbourCount = nt.GetNeighbourCount(cup);
            if(tempNeighbourCount > pickedCupNeighbourCount)
            {
                pickedCupNeighbourCount = tempNeighbourCount;
                pickedCup = cup;
            }
        }

        return pickedCup;
                
    }

    public GameObject PickRandomCup(CupRack opponentRack)
    {
        int indexOfPickedCup = rng.Next(0, opponentRack.GetCupList().Count);
        var pickedCup = opponentRack.GetCupList()[indexOfPickedCup];
        if (DebugDecisions)
            Debug.Log(gameObject + " AI picked cup " + pickedCup.name);
        return pickedCup;
    }

    public float GetXZAngleBetweenHandAndCup(Vector3 origin, Vector3 cup)
    {
        float deltaX = cup.x - origin.x ;
        float deltaZ = cup.z - origin.z;

        float degree = Mathf.Atan2(deltaZ, deltaX);
        return degree;
    }

    public bool IsReadyToThrow()
    {
        if (roundHandler.ThrownBalls.Count == 0)
            return true;

        bool isReady = true;
        foreach (var ball in roundHandler.ThrownBalls)
        {
            if (ball == null)
                continue;
            var ballController = ball.GetComponent<BallController>();
            if (ballController.isInPlay || ballController.timeAlive > 1f)
            {
                isReady = false;
                break;
            }
        }
        return isReady;
    }
}
