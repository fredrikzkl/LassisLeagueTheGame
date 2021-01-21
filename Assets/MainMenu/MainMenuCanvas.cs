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

    public GameObject logo;
    public GameObject mainMenuButtons;
    public GameObject backButton;
    public GameObject rulesCanvas;
    public GameObject settingsCanvas;

    public GameObject vsModeCanvas;

    Window currentWindow;

    CanvasGroup mainMenuButtonsGroup;
    bool showButtons;

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
        logo.SetActive(false);

    }

    public void ToRulesClick()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        currentWindow = Window.Rules;
        cameraAnimator.SetBool("toRules", true);

        HideMenuButtons();
        logo.SetActive(false);
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
        ShowMenuButtons();
    }
    
    public void ShowVSModeCanvas()
    {
        var canvasGroup = vsModeCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }

    public void HideVSModeCanvas()
    {
        var canvasGroup = vsModeCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
    }

    public void ShowRulesCanvas()
    {
        rulesCanvas.SetActive(true);
        backButton.SetActive(true);
        rulesCanvas.GetComponent<RulesCanvas>().Open();
    }

    public void BackToMainScreen()
    {
        FindObjectOfType<SoundManager>().PlaySound("Click");
        backButton.SetActive(false);
        ShowMenuButtons();
        rulesCanvas.SetActive(false);
        logo.SetActive(true);

        if(currentWindow == Window.Rules)
        {
            rulesCanvas.GetComponent<RulesCanvas>().Close();
        }

        cameraAnimator.SetBool("toRules", false);

    }

    void ShowMenuButtons()
    {
        CanvasGroup g = mainMenuButtons.GetComponent<CanvasGroup>();
        showButtons = true;
        g.blocksRaycasts = true; //this prevents the UI element to receive input events
    }

    void HideMenuButtons()
    {
        CanvasGroup g = mainMenuButtons.GetComponent<CanvasGroup>();
        g.alpha = 0f; //this makes everything transparent
        g.blocksRaycasts = false; //this prevents the UI element to receive input events
        showButtons = false;
    }

    void ToggleActive(GameObject obj)
    {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (showButtons && mainMenuButtonsGroup.alpha  <= 1f)
        {
            mainMenuButtonsGroup.alpha += 0.025f;
        }
    }

}
