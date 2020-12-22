using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour
{
    public Animator cameraAnimator;

    public GameObject logo;
    public GameObject mainMenuButtons;
    public GameObject backButton;
    public GameObject rulesCanvas;
   
    private void Start()
    {

    }

    public void StartGameOnClick()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    public void ToSettingsClick()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        cameraAnimator.SetBool("toSettings", true);
        ToggleActive(mainMenuButtons);
        ToggleActive(backButton);
        ToggleActive(rulesCanvas);
        ToggleActive(logo);
    }

    public void BackToMainScreen()
    {
        FindObjectOfType<SoundManager>().PlaySoundEffect("Click");
        ToggleActive(backButton);
        ToggleActive(mainMenuButtons);
        rulesCanvas.SetActive(false);

        cameraAnimator.SetBool("toSettings", false);

    }

    void ToggleActive(GameObject obj)
    {
        if (obj.activeSelf)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }
}
