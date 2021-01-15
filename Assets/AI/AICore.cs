using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICore : MonoBehaviour
{
    public bool DebugDecisions;
    System.Random rng = new System.Random();

    //Refs
    private PlayerRoundHandler roundHandler;


    private void Start()
    {
        roundHandler = GetComponent<PlayerRoundHandler>();
    }

    public ThrowData CalculateShot(Transform origin, Transform target, int direction)
    {
        ThrowData shot = new ThrowData();
        //Henter ut XZ vinkelen, som er konstant
        shot.XZAngle = Mathf.PI - GetXZAngleBetweenHandAndCup(origin.position, target.position);
        //TODO: Setter XY angle som 45 alltid
        shot.XYAngle = 0.78f;
        //TODO: Setter power random fra 60-90 (fordi hvorfor ikke)
        shot.Power = rng.Next(70, 90);

        return shot;
    }

    public bool IsReadyToThrow()
    {
        if (roundHandler.ThrownBalls.Count == 0)
            return true;

        bool isReady = true;
        foreach(var ball in roundHandler.ThrownBalls)
        {
            var ballController = ball.GetComponent<BallController>();
            if(ballController.isInPlay || ballController.timeAlive > 1f)
            {
                isReady = false;
                break;
            }
        }
        return isReady;
    }
    

    public GameObject PickACup()
    {
        var opponentRack = FindObjectOfType<GameLogic>().GetOpponentCupRack(gameObject);
        //Sjekker først om han har truffet noen baller
        if(roundHandler.HitCups.Count > 0)
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

        return PickRandomCup(opponentRack); 
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
        float deltaX = origin.x - cup.x;
        float deltaZ = origin.z - cup.z;

        float degree = Mathf.Atan2(deltaZ, deltaX);
        return degree;
    }
}
