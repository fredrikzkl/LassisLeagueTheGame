using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameCanvas : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    public CanvasGroup player1stats;
    public CanvasGroup player2stats;

    public GameObject menu;

    public void SetText(string score, float time)
    {
        scoreText.text = score;
        timeText.text = formatTimeNicely(time);
    }

    public void SetPlayerStats(GameObject player1, GameObject player2)
    {
        //Player1
        SetPlayerStats(player1stats, player1);
        //Player 2
        SetPlayerStats(player2stats, player2);

        UpdateWins();
    }

    

    public void UpdateWins()
    {
        player1stats.transform.Find("Wins").GetComponent<TMP_Text>().text = SessionData.Player1Wins.ToString();
        player2stats.transform.Find("Wins").GetComponent<TMP_Text>().text = SessionData.Player2Wins.ToString();
    }

    public void SetPlayerStats(CanvasGroup statsGroup, GameObject player)
    {
        var stats = player.GetComponent<PlayerRoundHandler>().stats;
        var playerController = player.GetComponent<PlayerController>();

        //Setter navn og farge
        var header = statsGroup.transform.Find("Header");
        header.GetComponent<TMP_Text>().text = player.name;

        var pc = playerController.GetPlayerColor();
        statsGroup.gameObject.GetComponent<Image>().color = new Color(pc.r, pc.g, pc.b, 0.5f);

        //Setter stats
        var hitRatingStats = statsGroup.transform.Find("HitRating_Value");
        hitRatingStats.GetComponent<TMP_Text>().text = Math.Round(stats.GetHitRate() * 100, 2) + "%";

        var bestStreak = statsGroup.transform.Find("Streak_Value");
        bestStreak.GetComponent<TMP_Text>().text = "" + stats.GetLongestHitStreak();
    }




    string formatTimeNicely(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;

    }

    public void ToggleMenu()
    {
        if (menu.activeSelf)
            menu.SetActive(false);
        else
            menu.SetActive(true);
    }

    public void RematchOnClick()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        FindObjectOfType<GameLogic>().Rematch();
    }

    public void ExitToMenu()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }


}
