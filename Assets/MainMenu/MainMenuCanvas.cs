using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    public Animator cameraAnimator;

    public GameObject logo;
    public GameObject mainMenuButtons;
    public GameObject backButton;
    public GameObject rulesCanvas;

    string currentWindow;
   

    public void StartGameOnClick()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    public void ToRulesClick()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        currentWindow = "rules";
        cameraAnimator.SetBool("toRules", true);
        mainMenuButtons.SetActive(false);
        logo.SetActive(false);

    }

    public void ShowRulesCanvas()
    {
        rulesCanvas.SetActive(true);
        backButton.SetActive(true);
        rulesCanvas.GetComponent<RulesCanvas>().Open();

    }

    public void BackToMainScreen()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        ToggleActive(backButton);
        ToggleActive(mainMenuButtons);
        rulesCanvas.SetActive(false);

        if(currentWindow == "rules")
        {
            rulesCanvas.GetComponent<RulesCanvas>().Close();
        }

        cameraAnimator.SetBool("toRules", false);

    }

    void ToggleActive(GameObject obj)
    {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }

   
}
