using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool gameIsPaused;

    private void Start()
    {
        gameIsPaused = false;    
    }

    public static void Pause()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public static void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
}
