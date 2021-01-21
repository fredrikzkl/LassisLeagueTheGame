using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VsModeController : MonoBehaviour
{
    public GameObject Player1Panel;
    public GameObject Player2Panel;

    private VSModeSettingsData settings;

    //Player1 Panel
    private TMP_Text player1Header;

    private Button player1AIButton;
    private TMP_Text player1AIButtonText;

    //Player 2 Panel
    private TMP_Text player2Header;

    private Button player2AIButton;
    private TMP_Text player2AIButtonText;

    private void Start()
    {
        //Headers
        player1Header = Player1Panel.transform.Find("Header").GetComponent<TMP_Text>();
        player2Header = Player2Panel.transform.Find("Header").GetComponent<TMP_Text>();

        //AI Button
        player1AIButton = Player1Panel.transform.Find("AIDifficultyButton").GetComponent<Button>();
        player2AIButton = Player2Panel.transform.Find("AIDifficultyButton").GetComponent<Button>();

        //AIButtonText
        player1AIButtonText = Player1Panel.transform.Find("AIDifficultyButton/AIDifficultyButton_Text").GetComponent<TMP_Text>();
        player2AIButtonText = Player2Panel.transform.Find("AIDifficultyButton/AIDifficultyButton_Text").GetComponent<TMP_Text>();

        //Laster inn data og setter variabler
        settings = SaveSystem.LoadVsModeSettings();
        Init(settings);
    }

    public void Init(VSModeSettingsData settings)
    {
        player1Header.text = settings.player1Type.ToString();
        player2Header.text = settings.player2Type.ToString();

        player1AIButtonText.text = settings.player1AIDifficulty.ToString();
        player2AIButtonText.text = settings.player2AIDifficulty.ToString();
    }

    public void StartGameOnClick()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        //Starter med å lagre settingsene
        SaveSystem.SaveVSModeSettings(settings);
        //Klargjør sessionen
        SessionData.ClearSession();
        //Laster inn spillet! Lets go!
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    public void PlayerTypeOnClick(int player)
    {
        switch (player)
        {
            case 1:
                settings.player1Type = TogglePlayerType(settings.player1Type, player1AIButton.gameObject);
                player1Header.text = settings.player1Type.ToString();
                break;
            case 2:

                settings.player2Type = TogglePlayerType(settings.player2Type, player2AIButton.gameObject);
                player2Header.text = settings.player2Type.ToString();
                break;
            default:
                Debug.LogWarning("Player number " + player + " does not exist");
                break;
        }
    }

    public PlayerType TogglePlayerType(PlayerType current, GameObject AIButton)
    {
        if(current == PlayerType.AI)
        {
            current = PlayerType.Player;
            AIButton.SetActive(false);
        }
        else
        {
            current = PlayerType.AI;
            AIButton.SetActive(true);
        }
        return current;
    }

    public void AIDifficultyChangeOnClick(int player)
    {
        switch (player)
        {
            case 1:
                var nextDifp1 = NextDifficultyLevel((int)settings.player1AIDifficulty);
                settings.player1AIDifficulty = nextDifp1;
                player1AIButtonText.text = nextDifp1.ToString();
                break;
            case 2:
                var nextDifp2 = NextDifficultyLevel((int)settings.player2AIDifficulty);
                settings.player2AIDifficulty = nextDifp2;
                player2AIButtonText.text = nextDifp2.ToString();
                break;
            default:
                Debug.LogWarning("Player number " + player + " does not exist");
                break;
        }
    }

    public DifficultyLevel NextDifficultyLevel(int current)
    {
        int difficultyCount = DifficultyLevel.GetNames(typeof(DifficultyLevel)).Length;
        int newLevel = current + 1;
        //-1, ignorer impossible
        if (newLevel >= difficultyCount-1)
            newLevel = 0;
        return (DifficultyLevel)newLevel;
    }

    

    


}
