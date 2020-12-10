using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Formations;

public class PlayerRoundHandler : MonoBehaviour
{

    public List<GameObject> ThrownBalls { get; set; }
    public List<GameObject> HitCups { get; set; }

    public GameObject cupRack;

    //Game variabels
    public int restacks { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        HitCups = new List<GameObject>();
        ThrownBalls = new List<GameObject>();

        restacks = FindObjectOfType<GameRules>().Restacks;
    }


    public void Restack(string formation)
    {
        var rack = cupRack.GetComponent<CupRack>();
        int cupsRemaining = rack.GetCupCount();
        //TODO: Lage slik at restack tar inn en formasjon
        rack.Rerack(formation);

    }

    public void EndRound()
    {
        if (!HasBallsAlive())
        {
            bool ballsBack = false;
            //Balls back
            if (HitCups.Count > 1)
            {
                if (HitCups[0] == HitCups[1])
                {
                    //TODO!: DEN FJERNER FRA EGEN GREIE!!!
                    foreach(var cup in cupRack.GetComponent<CupRack>().PickRandomCups(HitCups[0]))
                    {
                        //HitCups.Add(cup);
                    }
                    Debug.Log("TRIPPLE! Or? Actual number: " + HitCups.Count);
                }
                ballsBack = true;
            }

            RemoveHitCups();
            RemoveAllThrownBalls();

            if (ballsBack)
            {
                gameObject.GetComponent<PlayerController>().BallsBack();
            }
            else
            {

                FindObjectOfType<GameLogic>().RoundEnded(gameObject);
            }
        }
    }

    public void RemoveHitCups()
    {
        if (HitCups.Count == 0) return;
        foreach (var cup in HitCups)
        {
            cup.GetComponent<CupController>().Rack.GetComponent<CupRack>().RemoveFromCupList(cup);
            Destroy(cup);
        }
        HitCups.Clear();
    }

    public void RemoveAllThrownBalls()
    {
        if (ThrownBalls.Count == 0) return;
        foreach (var ball in ThrownBalls)
        {
            Destroy(ball);
        }

        ThrownBalls.Clear();
    }

    public bool HasBallsAlive()
    {
        if (ThrownBalls.Count == 0) return false;
        foreach (var ball in ThrownBalls)
        {
            if (ball.GetComponent<BallController>().isInPlay)
                return true;
        }
        return false;
    }

    //Henter ut alle formasjonene til spilleren sin rack
    public List<Formation> GetValidFormations()
    {
        return Formations.GetValidFormations(cupRack.GetComponent<CupRack>().GetCupCount());
    }


}
