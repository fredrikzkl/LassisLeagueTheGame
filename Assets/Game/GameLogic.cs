using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public List<Replay> replays;

    public GameObject playerWithTheRound { get; set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartPlayerRound(player1);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartPlayerRound(player2);
        }
    }


    private void Awake()
    {
        replays = new List<Replay>();
    }

    public void StartGame()
    {
        Debug.Log("Game started!");
        StartPlayerRound(player1);
    }

    

    public void RoundEnded(GameObject player)
    {
        //Setter runden til ingenting i starten
        playerWithTheRound = null;
        //Sjekke wincondition for player
        Debug.Log("Round ended!");

        if (player == player1)
            StartPlayerRound(player2);
        else
            StartPlayerRound(player1);
    }

    public void StartPlayerRound(GameObject player)
    {
        if(playerWithTheRound != player)
        {
            if(playerWithTheRound != null) playerWithTheRound.GetComponent<PlayerController>().EndRound();
            playerWithTheRound = player;
        }
        player.GetComponent<PlayerController>().StartRound();
    }

    public void PlayReplay(GameObject player)
    {
        foreach (var r in replays)
        {
            player.GetComponent<PlayerController>().ThrowBall(r.arc, r.angle, r.power);
            replays.Remove(r);
        }
    }

    public void AddThrowToReplay(Vector2 arc, float angle, float power, Transform originPosition)
    {
        replays.Add(new Replay(arc,angle,power,originPosition));
    }

    


    
}
