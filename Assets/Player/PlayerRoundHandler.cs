using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoundHandler : MonoBehaviour
{
    
    public List<GameObject> ThrownBalls { get; set; }
    public List<GameObject> HitCups { get; set; }

    public GameObject cupRack;

    // Start is called before the first frame update
    void Start()
    {
        HitCups = new List<GameObject>();
        ThrownBalls = new List<GameObject>();
    }

    public void EndRound()
    {
        if (!HasBallsAlive())
        {
            RemoveHitCups();
            RemoveAllThrownBalls();
            FindObjectOfType<GameLogic>().RoundEnded(gameObject);
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


}
