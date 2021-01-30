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
    public SkinHandler skins;

    public CupController player1CupDisplay;
    public CupController player2CupDisplay;

    //Player1 Panel
    private Image player1PanelImage;
    private TMP_Text player1Header;

    private Button player1AIButton;
    private TMP_Text player1AIButtonText;

    private TMP_Text player1SkinText;

    //Player 2 Panel
    private Image player2PanelImage;
    private TMP_Text player2Header;

    private Button player2AIButton;
    private TMP_Text player2AIButtonText;

    private TMP_Text player2SkinText;


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

        //PanelImage
        player1PanelImage = Player1Panel.GetComponent<Image>();
        player2PanelImage = Player2Panel.GetComponent<Image>();

        //SkinText
        player1SkinText = Player1Panel.transform.Find("PlayerSkinButton/PlayerSkinText").GetComponent<TMP_Text>();
        player2SkinText = Player2Panel.transform.Find("PlayerSkinButton/PlayerSkinText").GetComponent<TMP_Text>();

        //Laster inn data og setter variabler
        settings = SaveSystem.LoadVsModeSettings();
        Init(settings);
    }

    public void Init(VSModeSettingsData settings)
    {
        player1Header.text = settings.player1Type.ToString();
        player2Header.text = settings.player2Type.ToString();

        player1AIButton.gameObject.SetActive(settings.player1Type == PlayerType.AI);
        player2AIButton.gameObject.SetActive(settings.player2Type == PlayerType.AI);

        player1AIButtonText.text = settings.player1AIDifficulty.ToString();
        player2AIButtonText.text = settings.player2AIDifficulty.ToString();

        //Skins
        var player1Skin = skins.GetSkin(settings.player1Skin);
        SetPanelColor(player1PanelImage, player1Skin.playerColor);
        player1CupDisplay.SetMaterials(player1Skin);
        player1SkinText.text = settings.player1Skin;

        var player2Skin = skins.GetSkin(settings.player2Skin);
        SetPanelColor(player2PanelImage, player2Skin.playerColor);
        player2CupDisplay.SetMaterials(player2Skin);
        player2SkinText.text = settings.player2Skin;

    }

    private void SetPanelColor(Image panelImage, Color color)
    {
        panelImage.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    public void StartGameOnClick()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        //Starter med å lagre settingsene
        SaveVsModeSettings();
        //Klargjør sessionen
        SessionData.ClearSession();
        //Laster inn spillet! Lets go!
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        //Level

        

        string level = "SanDiego";
        SessionData.CurrentMap = level;
        SceneManager.LoadScene(level, LoadSceneMode.Additive);

    }

    public void SkinToggleOnClick(int player)
    {
        switch (player)
        {
            case 1:
                CupSkin newSkin1 = skins.GetNextSkin(settings.player1Skin);
                player1SkinText.text = newSkin1.Name;
                player1CupDisplay.SetMaterials(newSkin1);
                SetPanelColor(player1PanelImage, newSkin1.playerColor);

                settings.player1Skin = newSkin1.Name;
                break;
            case 2:
                CupSkin newSkin2 = skins.GetNextSkin(settings.player2Skin);
                player2SkinText.text = newSkin2.Name;
                player2CupDisplay.SetMaterials(newSkin2);
                SetPanelColor(player2PanelImage, newSkin2.playerColor);

                settings.player2Skin = newSkin2.Name;
                break;
            default:
                Debug.LogWarning("Player number " + player + " does not exist (Toogle Skin)");
                break;
        }
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

    public void CloseMenu()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;

    }

    public void SaveVsModeSettings()
    {
        SaveSystem.SaveVSModeSettings(settings);
    }
    


}
