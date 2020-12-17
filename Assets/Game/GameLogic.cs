﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    //Canvases
    public GameObject HUD;
    public GameObject EndScreen;

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
            //player.GetComponent<PlayerController>().ThrowBall(r.arc, r.angle, r.power);
            replays.Remove(r);
        }
    }

    public void AddThrowToReplay(Vector2 arc, float angle, float power, Transform originPosition)
    {
        replays.Add(new Replay(arc,angle,power,originPosition));
    }

    public GameObject GetOpponent(GameObject player)
    {
        if (player == player1)
            return player2;
        return player1;
    }

    public CupRack GetOpponentCupRack(GameObject player)
    {
        var opponent = GetOpponent(player);
        return opponent.GetComponent<PlayerRoundHandler>().cupRack.GetComponent<CupRack>();
    }

    public void CheckWinCondition(GameObject player)
    {
        if(GetOpponentCupRack(player).GetCupList().Count == 0)
        {
            Debug.Log("Player " + player + " won the match!");
            GameOver();
            InitiateWinningSequence(player);
        }
    }

    public void CheckWinCondition(GameObject player, GameObject cup)
    {
        if (GetOpponentCupRack(player).GetCupList().Count == 0)
        {
            Debug.Log("Player " + player + " won the match!");
            GameOver();
            InitiateWinningSequence(player);
        }
    }

    public void GameOver()
    {
        //Get the time
        
        EndScreen.SetActive(true);
        FindObjectOfType<SoundManager>().PlaySoundEffect("PartyHorn");
        FindObjectOfType<SoundManager>().PlaySoundEffect("CrowdCheer");
    }

    public void InitiateWinningSequence(GameObject player)
    {
        DisablePlayers();
        GameManager.Pause();
        HUD.SetActive(false);
    }

    void DisablePlayers()
    {
        player1.GetComponent<PlayerController>().Disable();
        player2.GetComponent<PlayerController>().Disable();
    }
    
}
