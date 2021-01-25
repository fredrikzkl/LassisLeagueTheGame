using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Window
{
    Main, Rules, VsMode
}

public class MainMenuCanvas : MonoBehaviour
{
    public Animator cameraAnimator;

    public CanvasGroup Logo;

    public GameObject mainMenuButtons;
    public GameObject backButton;
    public GameObject rulesCanvas;
    public GameObject settingsCanvas;

    public GameObject vsModeCanvas;

    Window currentWindow;

    CanvasGroup mainMenuButtonsGroup;
    bool showButtons;
    bool fadeInLogo = false;

    private void Start()
    {
        mainMenuButtonsGroup = mainMenuButtons.GetComponent<CanvasGroup>();
        mainMenuButtonsGroup.alpha = 1f;
        currentWindow = Window.Main;
    }

  

    public void ToVsMode()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        currentWindow = Window.VsMode;
        cameraAnimator.SetBool("toVSMode", true);

        HideMenuButtons();

    }

    public void ToRulesClick()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        currentWindow = Window.Rules;
        cameraAnimator.SetBool("toRules", true);

        HideMenuButtons();
    }

    public void ShowSettingsMenu() {
        FindObjectOfType<SoundManager>().PlaySound("Click");

        HideMenuButtons();
        settingsCanvas.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsCanvas.GetComponent<SystemSettings>().SaveSystemSettings();
        FindObjectOfType<SoundManager>().PlaySound("Click");
       
        settingsCanvas.SetActive(false);
        ShowMenuButtons(true);
    }
    
    public void ShowVSModeCanvas()
    {
        var canvasGroup = vsModeCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }

    public void HideVSModeCanvas()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        var canvasGroup = vsModeCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        cameraAnimator.SetBool("toVSMode", false);
        ShowMenuButtons(true);
        vsModeCanvas.GetComponent<VsModeController>().SaveVsModeSettings();
       
    }

    public void ShowRulesCanvas()
    {
        rulesCanvas.SetActive(true);
        backButton.SetActive(true);
        rulesCanvas.GetComponent<RulesCanvas>().Open();
    }

    public void FadeInLogo()
    {
        fadeInLogo = true;
    }

    public void BackToMainScreen()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        backButton.SetActive(false);
        ShowMenuButtons(true);
        rulesCanvas.SetActive(false);
        

        if(currentWindow == Window.Rules)
        {
            rulesCanvas.GetComponent<RulesCanvas>().Close();
        }

        cameraAnimator.SetBool("toRules", false);

    }

    void ShowMenuButtons(bool fadeInLogo)
    {
        CanvasGroup g = mainMenuButtons.GetComponent<CanvasGroup>();
        showButtons = true;
        g.blocksRaycasts = true; //this prevents the UI element to receive input events
        if (fadeInLogo)
        {
            FadeInLogo();
        }
    }

    void HideMenuButtons()
    {
        CanvasGroup g = mainMenuButtons.GetComponent<CanvasGroup>();
        g.alpha = 0f; //this makes everything transparent
        g.blocksRaycasts = false; //this prevents the UI element to receive input events
        showButtons = false;
        //Hides also logo
        Logo.alpha = 0f;
        fadeInLogo = false;
    }

    void ToggleActive(GameObject obj)
    {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }


    float fadeInFactor = 0.02f;
    private void FixedUpdate()
    {
        if (showButtons && mainMenuButtonsGroup.alpha  <= 1f)
        {
            mainMenuButtonsGroup.alpha += fadeInFactor;
        }
        if (fadeInLogo)
        {
            Logo.alpha += 0.02f;
        }
    }

}
