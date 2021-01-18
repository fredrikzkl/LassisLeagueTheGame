using System;
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
    public int islands { get; set; }

    //PlayerStats
    public PlayerStats stats { set; get; }

    //Eye To Eye - -1 betyr at spilleren ikke har truffet noe
    public int eyeToEyeScore { get; set; }

    void Start()
    {
        HitCups = new List<GameObject>();
        ThrownBalls = new List<GameObject>();

        restacks = FindObjectOfType<GameRules>().Restacks;
        islands = FindObjectOfType<GameRules>().Islands;

        stats = new PlayerStats();
        ResetEyeToEye();
    }


    public void Restack(string formation)
    {
        var rack = cupRack.GetComponent<CupRack>();
        int cupsRemaining = rack.GetCupCount();
        //TODO: Lage slik at restack tar inn en formasjon
        rack.Rerack(formation);

    }

    public void InvokeIsland(GameObject cup)
    {
        //Cup er motstanderen sin kopp som skal bli island
        var cc = cup.GetComponent<CupController>();
        cc.SetIsland(true);

        gameObject.GetComponent<PlayerController>().nextThrowIsIsland = true; ;

        islands--;
        FindObjectOfType<Announcer>().Say("Island");
    }

    public void EndRound()
    {
        if (!HasBallsAlive())
        {
            bool ballsBack = false;
            CupRack opponentRack = FindObjectOfType<GameLogic>().GetOpponentCupRack(gameObject);


            if (HitCups.Count > 0)
            {
                int bonusCups = 0;
                GameRules rules = FindObjectOfType<GameRules>();

                bool gotIsland = HitCups[0].GetComponent<CupController>().isSuccessfullyHitIsland();

                //Island
                if (gotIsland)
                {
                    bonusCups += rules.IslandsReward;
                }

                //Balls back
                if (HitCups.Count > 1)
                {
                    //Begge i samme kopp
                    if (HitCups[0] == HitCups[1])
                    {
                        HitCups.RemoveAt(1);
                        bonusCups += rules.SameCupReward;
                        FindObjectOfType<Announcer>().Say("DoubleHit");
                    }
                    else
                    {
                        FindObjectOfType<Announcer>().Say("BallsBack");
                    }
                    ballsBack = true;
                }

                if(bonusCups > 0)
                    opponentRack.PickRandomCups(HitCups, bonusCups).ForEach(c => HitCups.Add(c));
            }
            else if(opponentRack.GetCupCount() == 1)
            {
                //Brukt av announcer for reagere om det var nære
                CheckIfCloseCall();
            }

            if(FindObjectOfType<GameLogic>().GetGameStage() == GameStage.EyeToEye)
            {
                DetermineEyeToEyeScore();
                EndEyeToEyeRound();
            }
            else
            {
                RemoveHitCups();
            }


            RemoveAllThrownBalls();

            Debug.Log(stats.ToString());

            //Check for win condition
            FindObjectOfType<GameLogic>().CheckWinCondition(gameObject);

            if (stats.missStreak > 4)
            {
                int possibleCups = FindObjectOfType<GameLogic>().GetOpponentCupRack(gameObject).GetCupCount();
                FindObjectOfType<Announcer>().Dissapointed(stats.missStreak, possibleCups);
            }

            if (ballsBack)
                gameObject.GetComponent<PlayerController>().BallsBack();
            else

                FindObjectOfType<GameLogic>().RoundEnded(gameObject);
        }
    }

    private void CheckIfCloseCall()
    {
        foreach(var b in ThrownBalls)
        {
            var bc = b.GetComponent<BallController>();
            if (bc.didHitRim)
            {
                FindObjectOfType<Announcer>().CloseCall();
                break;
            }
        }
    }

    public void RemoveHitCups()
    {
        if (HitCups.Count == 0) return;
        foreach (var cup in HitCups)
        {
            var cc = cup.GetComponent<CupController>();
            var rack = cc.Rack.GetComponent<CupRack>();
            rack.UpdateNeighborTable(cup);
        }

        foreach (var cup in HitCups)
        {
            var cc = cup.GetComponent<CupController>();
            var rack = cc.Rack.GetComponent<CupRack>();

            cc.Rack.GetComponent<CupRack>().RemoveFromCupList(cup);
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
            if (ball == null) continue;
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

    public void DetermineEyeToEyeScore()
    {
        if (HitCups.Count >= 1)
        {
            var cupHit = HitCups[0];
            int row = int.Parse(cupHit.name.Split('-')[0]);
            if (row > eyeToEyeScore)
                eyeToEyeScore = row;
        }
    }

    public void ResetEyeToEye()
    {
        eyeToEyeScore = -1;
    }

    public void EndEyeToEyeRound()
    {
        foreach(var c in HitCups)
        {
            var cup = c.GetComponent<CupController>();
            cup.UpdateToStandardMaterial();
        }
        HitCups.Clear();
    }

  


}
