using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    //Strats
    public GameObject stratsButton;
    public GameObject stratsMenu;
    public Image stratsButtonColor;
    float stratsButtonOpacity = 0.75f;
    [Range(0,20)]
    public float stratsButtonColorChangeSpeed = 3.5f;
    [Range(0,8)]
    public float stratsButtonColorChangeInterval = 2f;
    [Range(0,1)]
    public float colorFadeFactor = 0.613f;


    //Referanser
    public Image ball1_indicator;
    public Image ball2_indicator;
    //Temp
    Color currentColor;
    //FPS STUFF
    public TextMeshProUGUI fpsText;
    private bool dislayFps = false;
    float avgFrameRate;


    private void Start()
    {
        if (FindObjectOfType<GameSettings>().ShowFps)
        {
            dislayFps = true;
            //fpsObject.GetComponent<TextMeshProUGUI>();
        }
    }

    public void OpenRestackMenu()
    {

        stratsMenu.GetComponent<StratsMenu>().Initiate();
        GameManager.gameIsPaused = true;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (dislayFps)
        {
            float current = 0;
            current = Time.frameCount / Time.time;
            avgFrameRate = (int)current;
            fpsText.text = avgFrameRate.ToString() + " FPS";
        }

        if (stratsButton.active == true)
        {
            var lerpColor = new Color(currentColor.r * colorFadeFactor, currentColor.g * colorFadeFactor, currentColor.b * colorFadeFactor);
            float t = (Mathf.Sin(Time.fixedTime * stratsButtonColorChangeSpeed) + 1)/ stratsButtonColorChangeInterval;
            SetStratsButtonColor(Color.Lerp(currentColor, lerpColor, t));
        }
    }


    public void UpdateBallIndicator(int ballCount)
    {
        Color temp = currentColor;

        if (ball1_indicator.color == temp)
        {
            ball1_indicator.color = Color.black;
            return;
        }
        else if (ball2_indicator.color == temp)
            ball2_indicator.color = Color.black;
    }

    public void UpdateColors(Color c, int ballCount)
    {
        currentColor = new Color(c.r, c.g, c.b);
        if (ballCount > 1)
        {
            ball1_indicator.color = currentColor;
        }
        ball2_indicator.color = currentColor;
        SetStratsButtonColor(currentColor);
    }

    void SetStratsButtonColor(Color c)
    {
        Color tempColor = new Color(c.r, c.g, c.b, stratsButtonOpacity);
        stratsButtonColor.color = tempColor;
    }

    public void ActivateStratsButton()
    {
        stratsButton.SetActive(true);

    }

    public void DeactivateStratsButton()
    {
        stratsButton.SetActive(false);
    }

}
