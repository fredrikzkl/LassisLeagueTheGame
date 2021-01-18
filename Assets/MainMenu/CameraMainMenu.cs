using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    public MainMenuCanvas mainMenuCanvas;

    public void ShowRules()
    {
        mainMenuCanvas.GetComponent<MainMenuCanvas>().ShowRulesCanvas();
    }

    public void ShowVSMode()
    {
        mainMenuCanvas.GetComponent<MainMenuCanvas>().ShowVSModeCanvas();
    }
}
