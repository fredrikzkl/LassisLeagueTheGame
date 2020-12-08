using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIMenu : MonoBehaviour
{
    public GameObject restackMenu;     

    public void OpenRestackMenu()
    {
        restackMenu.GetComponent<RestackMenu>().Initiate();
        GameManager.gameIsPaused = true;
        Time.timeScale = 0f;
    }
}
