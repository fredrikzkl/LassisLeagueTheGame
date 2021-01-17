using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void SetPlayerStats(PlayerStats p1Stats, PlayerStats p2Stats)
    {
        SetPlayerStats(player1stats, p1Stats);
        SetPlayerStats(player2stats, p2Stats);
    }

    public void SetPlayerStats(CanvasGroup statsGroup, PlayerStats stats)
    {
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
