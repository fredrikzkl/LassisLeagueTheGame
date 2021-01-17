using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStage
{
    EyeToEye, Game
}

public class GameLogic : MonoBehaviour
{

    public GameStage gameStage;

    public GameObject player1;
    public GameObject player2;

    //Canvases
    public GameObject HUD;
    public GameObject EndScreen;

    public List<Replay> replays;

    public GameObject playerWithTheRound { get; set; }

    public float startTime;


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
        startTime = Time.time;
        StartPlayerRound(player1);
    }

    

    public void RoundEnded(GameObject player)
    {
        //Setter runden til ingenting i starten
        playerWithTheRound = null;

        if(gameStage == GameStage.EyeToEye && player == player2)
        {
            var eyeToEyeResult = EyeToEyeResults();
            if(eyeToEyeResult != null)
            {
                gameStage = GameStage.Game;
                StartPlayerRound(eyeToEyeResult);
                return;
            }
        }

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
        if(gameStage == GameStage.EyeToEye)
        {
            player.GetComponent<PlayerController>().StartRound(1);
        }
        else
        {
            player.GetComponent<PlayerController>().StartRound(FindObjectOfType<GameRules>().NumberOfBallsPrRound);
        }
    }

    //Sjekker om eye to eye har en vinner - returnerer vinneren
    public GameObject EyeToEyeResults()
    {
        int player1Score = player1.GetComponent<PlayerRoundHandler>().eyeToEyeScore;
        int player2Score = player2.GetComponent<PlayerRoundHandler>().eyeToEyeScore;

        if (player1Score > player2Score)
            return player1;
        if (player2Score > player1Score)
            return player2;

        player1.GetComponent<PlayerRoundHandler>().ResetEyeToEye();
        player2.GetComponent<PlayerRoundHandler>().ResetEyeToEye();
        //Om returnerer null, er det ingen som har vunnet
        return null;
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
            if(EndScreen.activeSelf == false)
            {
                GameOver(player);
            }
            Debug.Log("Player " + player + " won the match!");
            InitiateWinningSequence(player);
        }
    }

    public GameStage GetGameStage()
    {
        return gameStage;
    }

    public void GameOver(GameObject winner)
    {

        //Get the time
        EndScreen.SetActive(true);

        var elapsedTime = Time.time - startTime;
        Debug.Log("Elapsed Time: " + elapsedTime);
        
        var p1Cups = GetOpponentCupRack(player2).GetCupCount();
        var p2Cups = GetOpponentCupRack(player1).GetCupCount();

        if (winner == player1)
            p2Cups = 0;
        else
            p1Cups = 0;

        string score = p1Cups + " - " + p2Cups; 

        FindObjectOfType<EndGameCanvas>().SetText(score, elapsedTime);

        FindObjectOfType<EndGameCanvas>().SetPlayerStats(player1.GetComponent<PlayerRoundHandler>().stats, player2.GetComponent<PlayerRoundHandler>().stats);

        FindObjectOfType<SoundManager>().PlaySound("PartyHorn");
        FindObjectOfType<SoundManager>().PlaySound("CrowdCheer");

        FindObjectOfType<Announcer>().GG(elapsedTime, p1Cups, p2Cups);

    }

    public void InitiateWinningSequence(GameObject player)
    {
        DisablePlayers();
        //GameManager.Pause();
        HUD.SetActive(false);
        FindObjectOfType<EndGameCanvas>().ToggleMenu();

    }

    void DisablePlayers()
    {
        player1.GetComponent<PlayerController>().Disable();
        player2.GetComponent<PlayerController>().Disable();
    }

    public void Rematch()
    {
        Application.LoadLevel(Application.loadedLevel);
    }


}
