using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameCanvas : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    public GameObject menu;

    public void SetText(string score, float time)
    {
        scoreText.text = score;
        timeText.text = formatTimeNicely(time);
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
